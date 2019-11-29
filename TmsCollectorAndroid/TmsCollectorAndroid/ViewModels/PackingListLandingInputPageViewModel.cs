using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Common;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Enums;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.ViewModels
{
    public class PackingListLandingInputPageViewModel : ViewModelBase
    {
        public PackingListLandingInputPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService,
            IPopupNavigation popupNavigation,
            IUserService userService,
            ICommonService commonService,
            ILandingService landingService,
            IWifiService wifiService,
            ILabelValidationService labelValidationService)
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;
            _popupNavigation = popupNavigation;
            _userService = userService;
            _commonService = commonService;
            _landingService = landingService;
            _wifiService = wifiService;
            _labelValidationService = labelValidationService;

            Model = new PackingListLandingInputModel();
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();

            CancelPackLanding += ExecuteCancelPackLanding;
            CancelBillOfLadingLanding += ExecuteCancelBillOfLadingLanding;
            PauseLanding += ExecutePauseLanding;
            EndingProcess += ExecuteEndingProcess;
        }

        private readonly INotificationService _notificationService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;
        private readonly ILandingService _landingService;
        private readonly IWifiService _wifiService;
        private readonly ILabelValidationService _labelValidationService;

        private PackingListLandingInputModel _model;
        public PackingListLandingInputModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        private Dictionary<string, DelegateCommand> _menuAdtionalButtons;
        public Dictionary<string, DelegateCommand> MenuAdtionalButtons
        {
            get { return _menuAdtionalButtons; }
            set { SetProperty(ref _menuAdtionalButtons, value); }
        }

        #region Navigation Methods

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("TrafficScheduleDateConfirmed", out bool confirmed))
            {
                TrafficScheduleDateConfirmed(confirmed);
            }

            if (parameters.TryGetValue("ConfirmationRandomConfirmed", out confirmed))
            {
                parameters.TryGetValue("CallBackData", out Dictionary<string, object> callBackData);
                ConfirmationRandomConfirmed(confirmed, callBackData);
            }

            if (parameters.TryGetValue("MaintenanceSealsInputConfirmed", out confirmed))
            {
                parameters.TryGetValue("CallBackData", out Dictionary<string, object> callBackData);

                MaintenanceSealsInputConfirmed(confirmed, callBackData);
            }
        }

        #endregion

        #region Private Methods

        private void AddMenuAdtionalBunttons()
        {
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>()
            {
                { "Faltas", ViewLackCommand },
                { "Faltas (Processo)", ViewLackByProcessCommand },
                { "Emitir Etiqueta", SendLabelCommand },
                { "Pausa", PauseProcessCommand },
                { "Término", EndingProcessCommand }
            };
        }

        private async void SetVehicleLanding()
        {
            AddMenuAdtionalBunttons();

            Model.VehicleDescription = Model.VehicleViewInfo.Plate;
            Model.TrafficSchedule = Model.VehicleViewInfo.TrafficScheduleId.ToString();
            Model.Line = Model.VehicleViewInfo.LineCode;
            Model.PackAmount = Model.VehicleViewInfo.PackAmount.ToString();
            Model.PackReading = Model.VehicleViewInfo.PackAmountReading.ToString();

            Model.TeamIsReadOnly = Model.CarIsReadOnly = true;
            Model.ReadingIsReadOnly = false;
            Model.Reading = String.Empty;
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            Model.ReadingFocus();
        }

        private async Task SetPackingListLanding()
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());

            var getLandingByTrafficScheduleId = await _landingService.GetLandingByTrafficScheduleId(
                _userService.User.Unit.Id, _userService.User.Unit.Code, Model.CarNumber, Model.TeamViewInfo.Id);

            await _popupNavigation.PopAllAsync();

            if (getLandingByTrafficScheduleId.Response != null && getLandingByTrafficScheduleId.Response.Valid)
            {
                Model.VehicleViewInfo = getLandingByTrafficScheduleId.Response;
                SetVehicleLanding();
            }
            else
            {
                MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
            }
        }

        private async Task ReadingPackLanding(ReadingPackLandingModel model)
        {
            var readingPackLanding = await _landingService.ReadingPackLanding(model);

            if (readingPackLanding.Response != null && readingPackLanding.Response.Valid)
            {
                Model.VehicleViewInfo.OperationalControlId = readingPackLanding.Response.OperationalControlId;
                Model.PackAmount = readingPackLanding.Response.PackAmount.ToString();
                Model.PackReading = readingPackLanding.Response.PackAmountReading.ToString();
            }
            else if (readingPackLanding.Response != null)
            {
                switch (readingPackLanding.Response.ExceptionCode)
                {
                    case ExceptionCodeEnum.VehicleNotBoardingLanding:
                    case ExceptionCodeEnum.VehicleInLandingPaused:
                        if (!await _notificationService.AskQuestionAsync(
                            $"{readingPackLanding.Response?.ExceptionMessage}\r\nDeseja iniciar desembarque?", SoundEnum.Alert))
                        {
                            var startingLanding = await _landingService.StartingLanding(new StartingLandingModel(
                                Model.VehicleViewInfo.OperationalControlId, _userService.User.Unit.Id,
                                Model.VehicleViewInfo.TrafficScheduleDetailId, Model.Team));

                            if (startingLanding.Response != null && startingLanding.Response.Valid)
                                Model.VehicleViewInfo.OperationalControlId = startingLanding.Response.OperationalControlId;
                            else
                            {
                                await _notificationService.NotifyAsync(startingLanding.Response?.ExceptionMessage ?? "Não houve resposta da API.");
                            }
                        }
                        break;
                    case ExceptionCodeEnum.PackRead:
                        if (await _notificationService.AskQuestionAsync(
                            $"{readingPackLanding.Response?.ExceptionMessage}\r\nDeseja estornar o volume registrado na leitura atual?", SoundEnum.Alert))
                        {
                            CallConfirmationRandomPopupPage(CancelPackLanding,
                                new CancelPackLandingModel(model.PackingListId, model.TrafficScheduleDetailId,
                                    model.BarCode, model.UnitLanding, model.MacAddress));
                        }
                        else if (await _notificationService.AskQuestionAsync(
                            $"{readingPackLanding.Response?.ExceptionMessage}\r\nDeseja estornar todo o CTRC do volume registrado na leitura atual?", SoundEnum.Alert))
                        {
                            CallConfirmationRandomPopupPage(CancelBillOfLadingLanding,
                                new CancelBillOfLadingLandingModel(model.PackingListId, model.TrafficScheduleDetailId,
                                    model.UnitLanding, model.BarCode, model.MacAddress));
                        }
                        break;
                    case ExceptionCodeEnum.PackNotLoading:
                        await _notificationService.NotifyAsync(readingPackLanding.Response.ExceptionMessage, SoundEnum.Alert);
                        await ReadingPackLanding(new ReadingPackLandingModel(model.TrafficScheduleDetailId,
                            model.TeamId, model.PackingListId, model.UnitLanding, model.BarCode, model.MacAddress,
                            true));
                        break;
                    default:
                        var msg = readingPackLanding.Response?.ExceptionMessage ??
                                  readingPackLanding.Error.ErrorDescription;

                        if (!string.IsNullOrEmpty(msg))
                            await _notificationService.NotifyAsync(msg, SoundEnum.Erros);

                        break;
                }
            }
            else
            {
                await _notificationService.NotifyAsync("Não foi possivel completar a requisição.", SoundEnum.Erros);
            }
        }

        private async Task ReadingTransportAccessoryLanding(ReadingTransportAccessoryLandingModel model)
        {
            var packingListViewInfo = await _landingService.ReadingTransportAccessoryLanding(model);

            if (packingListViewInfo.Response != null && packingListViewInfo.Response.Valid)
            {
                if (!string.IsNullOrEmpty(packingListViewInfo.Response.InformationMessage))
                    await _notificationService.NotifyAsync(packingListViewInfo.Response.InformationMessage,
                        SoundEnum.Alert, "Informação");

                Model.PackAmount = packingListViewInfo.Response.PackAmount.ToString();
                Model.PackReading = packingListViewInfo.Response.PackAmountReading.ToString();
            }
            else if (packingListViewInfo.Response != null)
            {
                switch (packingListViewInfo.Response.ExceptionCode)
                {
                    case ExceptionCodeEnum.VehicleNotBoardingLanding:
                    case ExceptionCodeEnum.VehicleInLandingPaused:
                        if (await _notificationService.AskQuestionAsync(
                            packingListViewInfo.Response.ExceptionMessage + "\r\nDeseja iniciar desembarque?",
                            SoundEnum.Alert))
                        {
                            var startingLanding = await _landingService.StartingLanding(
                                new StartingLandingModel(Model.VehicleViewInfo.OperationalControlId, model.UnitLanding,
                                    Model.VehicleViewInfo.TrafficScheduleDetailId, Model.TeamViewInfo.Code));

                            if (startingLanding.Response != null && startingLanding.Response.Valid)
                                Model.VehicleViewInfo.OperationalControlId =
                                    startingLanding.Response.OperationalControlId;
                            else
                                await _notificationService.NotifyAsync(startingLanding.Response.ExceptionMessage,
                                    SoundEnum.Erros);
                        }
                        break;
                    case ExceptionCodeEnum.PackRead:
                        if (await _notificationService.AskQuestionAsync(
                            packingListViewInfo.Response.ExceptionMessage +
                            "\r\nDeseja estornar o romaneio de acessório registrado na leitura atual?",
                            SoundEnum.Alert))
                        {
                            CallConfirmationRandomPopupPage(CancelPackingListTransportAccessoryLanding,
                                new CancelPackingListTransportAccessoryLandingModel(model.PackingListId,
                                    model.TrafficScheduleDetailId, model.PackingListTransportAccessoryCode,
                                    model.UnitLanding, model.MacAddress));
                        }
                        break;
                    case ExceptionCodeEnum.SealWarning:
                        await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Alert);

                        await NavigationService.NavigateAsync("MaintenanceSealsInputPage",
                            new NavigationParameters()
                            {
                                { "Title", "Solicitação" },
                                {
                                    "MaintenanceSealsInputModel",
                                    new MaintenanceSealsInputModel()
                                    {
                                        OnlyConference = true,
                                        PackingListAccessoryId = packingListViewInfo.Response.TransportAccessoryId,
                                        TransportAccessoryDoors = packingListViewInfo.Response.TransportAccessoryDoors
                                    }
                                },
                                { "CallBackData", new Dictionary<string, object>() {{ "Model", model }} }
                            }
                        );
                        break;
                    default:
                        await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Erros);

                        if (!string.IsNullOrEmpty(packingListViewInfo.Response.InformationMessage))
                            await _notificationService.NotifyAsync(packingListViewInfo.Response.InformationMessage, SoundEnum.Alert);
                        break;
                }
            }
            else
            {
                await _notificationService.NotifyAsync("Não foi possivel completar a requisição.", SoundEnum.Erros);
            }
        }

        #endregion

        #region Popups Calls

        private async void CallConfirmationRandomPopupPage<TModel>(Action<TModel> action, TModel model)
        {
            await NavigationService.NavigateAsync("ConfirmationRandomPopupPage", new NavigationParameters()
            {
                {
                    "CallBackData",
                    new Dictionary<string, object>()
                    {
                        { "Action", action },
                        { "Model", model }
                    }
                }
            });
        }

        #endregion

        #region Popups Return Handlers

        private void TrafficScheduleDateConfirmed(bool confirmed)
        {
            if (confirmed)
            {
                Model.Date = Model.VehicleViewInfo.TrafficScheduleDateTime.ToString("dd/MM/yyyy HH:mm");

                CarChangedCommand.Execute();
            }
            else
            {
                Model.ClearModelAfterTeam();
                Model.CarNumberFocus();
            }
        }

        private void ConfirmationRandomConfirmed(bool confirmed, Dictionary<string, object> callBackData)
        {
            if (confirmed)
            {
                if (callBackData.TryGetValue("Action", out object action)
                    && callBackData.TryGetValue("Model", out object model))
                {
                    var cancelPackLanding = action as Action<CancelPackLandingModel>;
                    var cancelBillOfLadingBoarding = action as Action<CancelBillOfLadingLandingModel>;
                    var endingProcess = action as Action<EndingLandingModel>;
                    var defaultAction = action as Action;

                    if (cancelPackLanding != null)
                    {
                        var cancelPackLandingModel = (CancelPackLandingModel)model;

                        cancelPackLanding(cancelPackLandingModel);
                    }
                    else if (cancelBillOfLadingBoarding != null)
                    {
                        var cancelBillOfLadingBoardingModel = (CancelBillOfLadingLandingModel)model;

                        cancelBillOfLadingBoarding(cancelBillOfLadingBoardingModel);
                    }
                    else if (endingProcess != null)
                    {
                        var endingLandingModel = (EndingLandingModel)model;

                        endingProcess(endingLandingModel);
                    }
                    else
                    {
                        defaultAction?.Invoke();
                    }
                }
            }
            else
            {
                Model.Reading = String.Empty;
                Model.ReadingFocus();
            }
        }

        private void MaintenanceSealsInputConfirmed(bool confirmed, Dictionary<string, object> callBackData)
        {
            if (confirmed
                && callBackData != null
                && callBackData.TryGetValue("Model", out ReadingTransportAccessoryLandingModel model))
            {
                ReadingTransportAccessoryLanding(new ReadingTransportAccessoryLandingModel(
                    model.TrafficScheduleDetailId, model.TeamId, model.PackingListId, model.UnitLanding,
                    model.PackingListTransportAccessoryCode, model.MacAddress, model.IgnoreLoaded, false));
            }
        }

        #endregion

        #region Actions

        public Action<CancelPackLandingModel> CancelPackLanding;

        private async void ExecuteCancelPackLanding(CancelPackLandingModel model)
        {
            var cancelPackLanding = await _landingService.CancelPackLanding(model);

            if (cancelPackLanding.Response != null && cancelPackLanding.Response.Valid)
            {
                await SetPackingListLanding();
            }
            else
            {
                await _notificationService.NotifyAsync(cancelPackLanding.Response?.ExceptionMessage, SoundEnum.Erros);
            }
        }

        public Action<CancelBillOfLadingLandingModel> CancelBillOfLadingLanding;

        private async void ExecuteCancelBillOfLadingLanding(CancelBillOfLadingLandingModel model)
        {
            var cancelBillOfLadingLanding = await _landingService.CancelBillOfLadingLanding(model);

            if (cancelBillOfLadingLanding.Response != null && cancelBillOfLadingLanding.Response.Valid)
            {
                await SetPackingListLanding();
            }
            else
            {
                await _notificationService.NotifyAsync(cancelBillOfLadingLanding.Response?.ExceptionMessage,
                    SoundEnum.Erros);
            }
        }

        public Action<CancelPackingListTransportAccessoryLandingModel> CancelPackingListTransportAccessoryLanding;

        public Action PauseLanding;

        private async void ExecutePauseLanding()
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());
            var packingListViewInfo = await _commonService.PauseBoardingOrLanding(Model.VehicleViewInfo.OperationalControlId);
            await _popupNavigation.PopAllAsync();

            if (packingListViewInfo.Response != null && packingListViewInfo.Response.Valid)
                await _notificationService.NotifyAsync("Processo parado.", SoundEnum.Alert);
            else if (packingListViewInfo.Response != null)
                await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage,
                    SoundEnum.Erros);
            else
                await _notificationService.NotifyAsync("A requisição não pode ser completada.",
                    SoundEnum.Erros);
        }

        public Action<EndingLandingModel> EndingProcess;

        private async void ExecuteEndingProcess(EndingLandingModel model)
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());
            var packingListViewInfo = await _landingService.EndingLanding(model);
            await _popupNavigation.PopAllAsync();

            if (packingListViewInfo.Response != null && packingListViewInfo.Response.Valid)
            {
                Model.Finalized = true;

                await _notificationService.NotifyAsync(
                    ((_userService.User.Unit.IsJointUnit)
                        ? "CT-e(s) transferido(s) com sucesso."
                        : "Processo finalizado."), SoundEnum.Alert);
            }
            else if (packingListViewInfo.Response != null)
            {
                await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
            }
            else
            {
                await _notificationService.NotifyAsync("A requisição não pode ser completada.", SoundEnum.Erros);
            }
        }

        #endregion

        #region Commands

        private DelegateCommand _teamChangedCommand;
        public DelegateCommand TeamChangedCommand =>
            _teamChangedCommand ?? (_teamChangedCommand = new DelegateCommand(TeamChangedCommandHandler));

        private DelegateCommand _carChangedCommand;
        public DelegateCommand CarChangedCommand =>
            _carChangedCommand ?? (_carChangedCommand = new DelegateCommand(CarChangedCommandHandler));

        private DelegateCommand _confirmationCommand;
        public DelegateCommand ConfirmationCommand =>
            _confirmationCommand ?? (_confirmationCommand = new DelegateCommand(ConfirmationCommandHandler));

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearCommandHandler));

        #region Menu Commands

        private DelegateCommand _packingListAccessoriesInputCommand;
        public DelegateCommand PackingListAccessoriesInputCommand =>
            _packingListAccessoriesInputCommand ?? (_packingListAccessoriesInputCommand = new DelegateCommand(PackingListAccessoriesInputCommandHandler));

        private DelegateCommand _viewLackCommand;
        public DelegateCommand ViewLackCommand =>
            _viewLackCommand ?? (_viewLackCommand = new DelegateCommand(ViewLackCommandHandler));

        private DelegateCommand _viewLackByProcessCommand;
        public DelegateCommand ViewLackByProcessCommand =>
            _viewLackByProcessCommand ?? (_viewLackByProcessCommand = new DelegateCommand(ViewLackByProcessCommandHandler));

        private DelegateCommand _sendLabelCommand;
        public DelegateCommand SendLabelCommand =>
            _sendLabelCommand ?? (_sendLabelCommand = new DelegateCommand(SendLabelCommandHandler));

        private DelegateCommand _pauseProcessCommand;
        public DelegateCommand PauseProcessCommand =>
            _pauseProcessCommand ?? (_pauseProcessCommand = new DelegateCommand(PauseProcessCommandHandler));

        private DelegateCommand _endingProcessCommand;
        public DelegateCommand EndingProcessCommand =>
            _endingProcessCommand ?? (_endingProcessCommand = new DelegateCommand(EndingProcessCommandHandler));

        #endregion

        #endregion

        #region Command Handlers

        private async void TeamChangedCommandHandler()
        {
            Model.TeamDescription = String.Empty;

            if (!string.IsNullOrEmpty(Model.Team))
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var team = await _commonService.ValidTeam(_userService.User.Unit.Id, Model.Team);

                await _popupNavigation.PopAllAsync();

                Model.TeamViewInfo = team.Response;

                var success = Model.TeamViewInfo != null && Model.TeamViewInfo.Active && Model.TeamViewInfo.Valid;
                if (success)
                {
                    Model.TeamDescription = Model.TeamViewInfo.Description;
                    Model.ClearModelAfterTeam();

                    Model.CarNumberFocus();
                }
                else
                {
                    Model.Team = String.Empty;
                    await _notificationService.NotifyAsync(Model.TeamViewInfo?.ExceptionMessage ?? "Equipe não encontrada.", SoundEnum.Erros);
                }
            }
        }

        private async void CarChangedCommandHandler()
        {
            if (Model.TeamViewInfo != null && !string.IsNullOrEmpty(Model.CarNumber))
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var getLandingByTrafficScheduleId = await _landingService.GetLandingByTrafficScheduleId(
                    _userService.User.Unit.Id, _userService.User.Unit.Code, Model.CarNumber, Model.TeamViewInfo.Id);

                Model.VehicleViewInfo = getLandingByTrafficScheduleId.Response;

                await _popupNavigation.PopAllAsync();

                if (Model.VehicleViewInfo != null && Model.VehicleViewInfo.Id > 0 && string.IsNullOrEmpty(Model.Date.Trim()))
                {
                    await NavigationService.NavigateAsync("RequestTrafficScheduleDateInputPopupPage",
                        new NavigationParameters()
                        {
                            { "Text", "Informar a data/hora da escala" },
                            { "TrafficScheduleDateTime", Model.VehicleViewInfo.TrafficScheduleDateTime }
                        });
                }
                else if (Model.VehicleViewInfo != null && Model.VehicleViewInfo.Valid)
                {
                    SetVehicleLanding();
                }
                else
                {
                    switch (Model.VehicleViewInfo?.ExceptionCode)
                    {
                        case ExceptionCodeEnum.VehicleNotBoardingLanding:
                        case ExceptionCodeEnum.VehicleInLandingPaused:
                            if (!await _notificationService.AskQuestionAsync(
                                $"{Model.VehicleViewInfo.ExceptionMessage}\r\nDeseja iniciar desembarque?", SoundEnum.Alert))
                            {
                                Model.ClearModelAfterTeam();
                                Model.CarNumberFocus();
                                break;
                            }

                            var startingLanding = await _landingService.StartingLanding(new StartingLandingModel(
                                Model.VehicleViewInfo.OperationalControlId, _userService.User.Unit.Id,
                                Model.VehicleViewInfo.TrafficScheduleDetailId, Model.Team));

                            if (startingLanding.Response != null && startingLanding.Response.Valid)
                            {
                                Model.VehicleViewInfo.OperationalControlId = startingLanding.Response.OperationalControlId;
                                CarChangedCommand.Execute();
                            }
                            else
                            {
                                await _notificationService.NotifyAsync(
                                    startingLanding.Response?.ExceptionMessage ??
                                    "Não foi possivel obter resposta da API.", SoundEnum.Erros);
                            }
                            break;

                        default:
                            Model.CarNumber = String.Empty;
                            await _notificationService.NotifyAsync(
                                Model.VehicleViewInfo?.ExceptionMessage ?? $"Frota {Model.CarNumber} inválida.",
                                SoundEnum.Erros);
                            break;
                    }
                }
            }
        }

        private async void ConfirmationCommandHandler()
        {
            if (Model.IsValid())
            {
                if (_labelValidationService.ValidateCommonLabel(Model.Reading))
                {
                    ReadingPackLanding(new ReadingPackLandingModel(Model.VehicleViewInfo.TrafficScheduleDetailId,
                        Model.TeamViewInfo.Id, Model.VehicleViewInfo.PackingListId, _userService.User.Unit.Id,
                        Model.Reading, _wifiService.MacAddress, false));
                }
                else if (_labelValidationService.ValidateMotherLabel(Model.Reading))
                {
                    ReadingTransportAccessoryLanding(new ReadingTransportAccessoryLandingModel(
                        Model.VehicleViewInfo.TrafficScheduleDetailId, Model.TeamViewInfo.Id,
                        Model.VehicleViewInfo.PackingListId, _userService.User.Unit.Id, Model.Reading,
                        _wifiService.MacAddress, false, true));
                }
                else
                {
                    await _notificationService.NotifyAsync("Tipo de código de barras não identificado.", SoundEnum.Alert);
                }

                Model.Reading = String.Empty;
                Model.ReadingFocus();
            }
            else
            {
                var msg = ((string.IsNullOrEmpty(Model.Reading))
                    ? "Necessário informar o código de barras."
                    : "Informe todos os campos obrigatórios antes de confirmar.");
                await _notificationService.NotifyAsync(msg, SoundEnum.Erros);
            }
        }

        private void ClearCommandHandler()
        {
            Model.ClearModel();
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
        }

        #region Menu Command Handlers

        private async void PackingListAccessoriesInputCommandHandler()
        {
            await NavigationService.NavigateAsync("PackingListAccessoriesInputPage");
        }

        private async void ViewLackCommandHandler()
        {
            if (Model.VehicleViewInfo != null)
            {
                var viewLackModel = new ViewLackModel()
                {
                    LackType = LackTypeEnum.Landing,
                    TrafficScheduleDetailId = Model.VehicleViewInfo.TrafficScheduleDetailId,
                    Vehicle = Model.VehicleViewInfo.CarNumber,
                    VehicleDescription = Model.VehicleViewInfo.Plate,
                    Date = Model.VehicleViewInfo?.TrafficScheduleDateTime.ToString("dd/MM/yyyy HH:mm"),
                    TrafficSchedule = Model.VehicleViewInfo.TrafficScheduleId.ToString(),
                    Line = Model.VehicleViewInfo.LineCode,
                    VehicleLineIsVisible = true,
                    LineIsVisible = true
                };

                await NavigationService.NavigateAsync("ViewLackPage", new NavigationParameters()
                {
                    {"ViewLackModel", viewLackModel}
                });
            }
        }

        private async void ViewLackByProcessCommandHandler()
        {
            if (Model.VehicleViewInfo != null)
            {
                var viewLackByProcessModel = new ViewLackByProcessModel()
                {
                    LackType = LackTypeEnum.Landing,
                    VehicleViewInfo = Model.VehicleViewInfo
                };

                await NavigationService.NavigateAsync("ViewLackByProcessPage", new NavigationParameters()
                {
                    {"ViewLackByProcessModel", viewLackByProcessModel}
                });
            }
        }

        private async void SendLabelCommandHandler()
        {
            if (Model.VehicleViewInfo != null)
            {
                var sendLabelInputModel = new SendLabelInputModel()
                {
                    Vehicle = Model.CarNumber,
                    VehicleDescription = Model.VehicleDescription,
                    UnitDestination = Model.VehicleViewInfo.UnitSendCode,
                    UnitDescription = Model.VehicleViewInfo.UnitSendDescription,
                    Date = Model.Date,
                    Line = Model.Line,
                    TrafficSchedule = Model.TrafficSchedule
                };

                await NavigationService.NavigateAsync("SendLabelInputPage", new NavigationParameters()
                {
                    {"SendLabelInputModel", sendLabelInputModel}
                });
            }
        }

        private async void PauseProcessCommandHandler()
        {
            if (!Model.Finalized
                && Model.VehicleViewInfo != null
                && await _notificationService.AskQuestionAsync("Deseja pausar o processo de desembarque?",
                    SoundEnum.Alert))
            {
                await NavigationService.NavigateAsync("ConfirmationRandomPopupPage", new NavigationParameters()
                {
                    {
                        "CallBackData",
                        new Dictionary<string, object>()
                        {
                            {"Action", PauseLanding},
                            {"Model", new object()}
                        }
                    }
                });
            }
            else if (Model.Finalized)
            {
                await _notificationService.NotifyAsync("Processo está finalizado.", SoundEnum.Alert);
            }
        }

        private async void EndingProcessCommandHandler()
        {
            if (!Model.Finalized
                && await _notificationService.AskQuestionAsync("Deseja terminar o processo de desembarque?",
                    SoundEnum.Alert))
            {
                var model = new EndingLandingModel(Model.VehicleViewInfo.PackingListId, Model.VehicleViewInfo.UnitSendId,
                    Model.VehicleViewInfo.OperationalControlId, _userService.User.Unit.Id);

                await NavigationService.NavigateAsync("ConfirmationRandomPopupPage", new NavigationParameters()
                {
                    {
                        "CallBackData",
                        new Dictionary<string, object>()
                        {
                            {"Action", EndingProcess},
                            {"Model", model}
                        }
                    }
                });
            }
            else if (Model.Finalized)
            {
                await _notificationService.NotifyAsync("Processo está finalizado.", SoundEnum.Alert);
            }
        }

        #endregion

        #endregion
    }
}
