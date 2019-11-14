using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Enums;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.ViewModels
{
    public class PackingListBoardingInputPageViewModel : ViewModelBase
    {
        public PackingListBoardingInputPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            IUserService userService,
            INotificationService notificationService,
            IPopupNavigation popupNavigation,
            ICommonService commonService,
            IBoardingService boardingService,
            IWifiService wifiService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _userService = userService;
            _notificationService = notificationService;
            _popupNavigation = popupNavigation;
            _commonService = commonService;
            _boardingService = boardingService;
            _wifiService = wifiService;

            Model = new PackingListBoardingInputModel();
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();

            CancelPackBoarding += ExecuteCancelPackBoarding;
            CancelBillOfLadingBoarding += ExecuteCancelBillOfLadingBoarding;
            PauseBoarding += ExecutePauseBoarding;
            EndingProcess += ExecuteEndingProcess;
        }

        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly ICommonService _commonService;
        private readonly IBoardingService _boardingService;
        private readonly IWifiService _wifiService;

        private PackingListBoardingInputModel _model;
        public PackingListBoardingInputModel Model
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

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("processo", out ProcessoEnum processo))
            {
                Model.Processo = processo;
                SetPageDefaultsByProcess();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("TrafficScheduleDateConfirmed", out bool confirmed))
            {
                TrafficScheduleDateConfirmed(confirmed);
            }

            if (parameters.TryGetValue("UnitInputConfirmed", out confirmed))
            {
                parameters.TryGetValue("UnitViewInfo", out UnitViewInfoModel unitDestinationViewInfo);

                RequestUnitInputConfirmed(confirmed, unitDestinationViewInfo);
            }

            if (parameters.TryGetValue("WarehousePasswordInputConfirmed", out confirmed))
            {
                parameters.TryGetValue("WarehousePassword", out string warehousePassword);
                parameters.TryGetValue("WarehousePasswordId", out int warehousePasswordId);

                WarehousePasswordInputConfirmed(confirmed, warehousePassword, warehousePasswordId);
            }

            if (parameters.TryGetValue("ConfirmationRandomConfirmed", out confirmed))
            {
                parameters.TryGetValue("CallBackData", out Dictionary<string, object> callBackData);
                ConfirmationRandomConfirmed(confirmed, callBackData);
            }
        }

        #endregion

        #region Private Methods

        private void SetPageDefaultsByProcess()
        {
            switch (Model.Processo)
            {
                case ProcessoEnum.LeituraUnica:
                    Model.CarNumberLineIsVisible = false;
                    Model.UnitLineIsVisible = true;
                    Title = "LEITURA ÚNICA";
                    break;
                default:
                    Model.CarNumberLineIsVisible = true;
                    Model.UnitLineIsVisible = false;
                    Title = "EMBARQUE";
                    break;
            }
        }

        private void AddMenuAdtionalBunttons()
        {
            if (Model.Processo != ProcessoEnum.LeituraUnica)
            {
                MenuAdtionalButtons = new Dictionary<string, DelegateCommand>()
                {
                    {"Faltas", ViewLackCommand},
                    {"Faltas (Processo)", ViewLackByProcessCommand},
                    {"Capacidade de Carga", ListBoardingWeightCommand},
                    {"Emitir Etiqueta", SendLabelCommand},
                    {"Pausa", PauseProcessCommand},
                    {"Término", EndingProcessCommand}
                };
            }
            else
            {
                MenuAdtionalButtons = new Dictionary<string, DelegateCommand>()
                {
                    {"Faltas", ViewLackCommand},
                    {"Emitir Etiqueta", SendLabelCommand},
                    {"Pausa", PauseProcessCommand},
                    {"Término", EndingProcessCommand}
                };
            }
        }

        private async Task SetPackingListBoarding()
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());

            var getPackingListIdBoarding = await _boardingService.GetPackingListIdBoarding(
                Model.VehicleViewInfo.TrafficScheduleId, _userService.User.Unit.Id,
                Model.VehicleViewInfo.UnitSendId, new MacAddressModel(_wifiService.MacAddress),
                _userService.User.Unit.IsJointUnit);

            await _popupNavigation.PopAllAsync();

            Model.PackingListViewInfo = getPackingListIdBoarding.Response;

            if (Model.PackingListViewInfo != null && Model.PackingListViewInfo.Valid)
            {
                Model.TeamIsReadOnly = Model.CarNumberIsReadOnly = true;
                Model.ReadingIsReadOnly = false;
                Model.Reading = String.Empty;
                Model.ReadingFocus();
                AddMenuAdtionalBunttons();
            }
            else
            {
                MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
                switch (Model.PackingListViewInfo?.ExceptionCode)
                {
                    case ExceptionCodeEnum.VehicleInBoardingPaused:
                        if (await _notificationService.AskQuestionAsync(
                            $"{Model.PackingListViewInfo?.ExceptionMessage}\r\nDeseja reiniciar o embarque?",
                            SoundEnum.Alert))
                        {
                            var startingBoarding = await _boardingService.StartingBoarding(new StartingBoardingModel(
                                Model.VehicleViewInfo.OperationalControlId,
                                _userService.User.Unit.Id,
                                Model.VehicleViewInfo.TrafficScheduleDetailId,
                                Model.TeamViewInfo.Code,
                                false));

                            if (startingBoarding.Response != null && startingBoarding.Response.Valid)
                            {
                                Model.VehicleViewInfo.OperationalControlId =
                                    startingBoarding.Response.OperationalControlId;
                            }
                            else
                            {
                                Model.ClearModelAfterTeam();
                                await _notificationService.NotifyAsync(startingBoarding.Response?.ExceptionMessage ??
                                                 startingBoarding.Error.ErrorDescription, SoundEnum.Erros);
                            }
                        }
                        break;
                    default:
                        Model.ClearModelAfterTeam();
                        await _notificationService.AskQuestionAsync(getPackingListIdBoarding.Response?.ExceptionMessage ??
                                               getPackingListIdBoarding.Error.ErrorDescription, SoundEnum.Erros);
                        break;
                }
            }
        }

        private async void ReadingPackBoarding(ReadingPackBoardingModel model)
        {
            var readingPackBoarding = await _boardingService.ReadingPackBoarding(model);

            if (readingPackBoarding.Response != null && readingPackBoarding.Response.Valid)
            {
                Model.PackAmount = readingPackBoarding.Response.TotalBillOfLading.ToString();
                Model.PackReading = readingPackBoarding.Response.TotalPack.ToString();
                Model.Reading = String.Empty;
                Model.ReadingFocus();
                Model.VehicleViewInfo.OperationalControlId = readingPackBoarding.Response.OperationalControlId;

                if (_userService.User.Unit.IsJointUnit)
                    Model.Line = Model.PackingListViewInfo.TransportAccessoriesAmount.ToString();
            }
            else if (readingPackBoarding.Response != null)
            {
                switch (readingPackBoarding.Response.ExceptionCode)
                {
                    case ExceptionCodeEnum.RequestWarehousePassword:
                        await NavigationService.NavigateAsync("RequestWarehousePasswordInputPopupPage");
                        break;
                    case ExceptionCodeEnum.RequestUnitDestination:
                        CallRequestUnitInputPopupPage();
                        break;
                    //case ExceptionCodeEnum.VehicleNotBoardingLanding:
                    //case ExceptionCodeEnum.VehicleInBoardingPaused:
                        //if (await ShowQuestionDialogSpeech($"{readingPackBoarding.Response?.ExceptionMessage}\r\nDeseja iniciar o embarque?"))
                        //{
                        //    var _startingBoarding = Accessor.WSTMS.StartingBoarding(GetEmployeeAuthenticated(), vehicleBoarding.OperationalControlId, SecuritySession.AuthenticatedUser.GetCurrentUnitId(), vehicleBoarding.TrafficScheduleDetailId, teamCode, false);
                        //    if (_startingBoarding.Valid)
                        //    {
                        //        vehicleBoarding.OperationalControlId = _startingBoarding.OperationalControlId;
                        //        if (!string.IsNullOrEmpty(textBoxBarCode.Text))
                        //        {
                        //            return ReadingPackBoarding(teamCode, trafficScheduleDetailId, trafficScheduleDetailVersionId, trafficScheduleDetailSequence, packingListId, unitLocal, unitDestinationId, textBoxBarCode, schedule, vehicle, packAmount, packAmountReading, teamId, ignoreWarehouse, isJointUnit, false, warehousePasswordId, true);
                        //        }
                        //        else
                        //            packingListViewInfo.Valid = true;
                        //    }
                        //    else
                        //    {
                        //        packingListViewInfo.Valid = false;
                        //        MsgBox.Show(_startingBoarding.ExceptionMessage, MsgType.Warning);
                        //    }
                        //}S
                        //break;
                    case ExceptionCodeEnum.PackRead:
                        if (await _notificationService.AskQuestionAsync( 
                            $"{readingPackBoarding.Response?.ExceptionMessage}\r\nDeseja estornar o volume registrado na leitura atual?", 
                            SoundEnum.Alert))
                        {
                            CallConfirmationRandomPopupPage(CancelPackBoarding,
                                new CancelPackBoardingModel(model.PackingListId, model.BarCode, model.MacAddress,
                                    model.IsJointUnit, false));
                        }
                        else if (!_userService.User.Unit.IsJointUnit
                            && await _notificationService.AskQuestionAsync(
                                $"{readingPackBoarding.Response.ExceptionMessage}"
                                 + (_userService.User.Unit.IsJointUnit ? string.Empty : $"\r\nEscala: {Model.TrafficScheduleDetailId}")
                                 + (_userService.User.Unit.IsJointUnit ? string.Empty : $"\r\nVeículo: {Model.CarNumber}")
                                 + "\r\nDeseja estornar todo o CTRC do volume registrado na leitura atual?",
                                SoundEnum.Alert))
                        {
                            CallConfirmationRandomPopupPage(CancelBillOfLadingBoarding,
                                new CancelBillOfLadingBoardingModel(model.PackingListId, model.BarCode,
                                    model.MacAddress, model.IsJointUnit));
                        }
                        else
                        {
                            Model.Reading = String.Empty;
                        }
                        break;
                    //case ExceptionCodeEnum.PackNotFoundWarehouse:
                    //    //if (await ShowQuestionDialogSpeech(readingPackBoarding.Response?.ExceptionMessage))
                    //    //    return ReadingPackBoarding(teamCode, trafficScheduleDetailId, trafficScheduleDetailVersionId, trafficScheduleDetailSequence, packingListId, unitLocal, unitDestinationId, textBoxBarCode, schedule, vehicle, packAmount, packAmountReading, teamId, true, isJointUnit, false, warehousePasswordId, false);
                    //    break;
                    //case ExceptionCodeEnum.IncompleteBoardingBol:
                    //    //if (await ShowQuestionDialogSpeech(readingPackBoarding.Response?.ExceptionMessage))
                    //    //{
                    //    //    var frmConfirm = new FrmConfirmationRandom();
                    //    //    frmConfirm.ShowDialog();
                    //    //    if (frmConfirm.DialogResult == DialogResult.OK)
                    //    //        return ReadingPackBoarding(teamCode, trafficScheduleDetailId, trafficScheduleDetailVersionId, trafficScheduleDetailSequence, packingListId, unitLocal, unitDestinationId, textBoxBarCode, schedule, vehicle, packAmount, packAmountReading, teamId, false, isJointUnit, true, warehousePasswordId, false);
                    //    //}
                    //    break;
                    //case ExceptionCodeEnum.CteUnitised:
                    //    //if (await ShowQuestionDialogSpeech(readingPackBoarding.Response?.ExceptionMessage))
                    //    //    return ReadingPackBoarding(teamCode, trafficScheduleDetailId, trafficScheduleDetailVersionId, trafficScheduleDetailSequence, packingListId, unitLocal, unitDestinationId, textBoxBarCode, schedule, vehicle, packAmount, packAmountReading, teamId, true, isJointUnit, false, warehousePasswordId, false);
                    //    break;
                    default:
                        var msg = readingPackBoarding.Response?.ExceptionMessage ??
                                  readingPackBoarding.Error.ErrorDescription;
                        if (!string.IsNullOrEmpty(msg.Trim()))
                            await _notificationService.NotifyAsync(msg, SoundEnum.Erros);

                        Model.Reading = String.Empty;
                        Model.ReadingFocus();
                        break;
                }
            }
            else
            {
                await _notificationService.NotifyAsync("Não foi possivel completar a requisição.", SoundEnum.Erros);
            }
        }

        private async void GetTrafficScheduleBoarding()
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());

            var getBoardingByTrafficScheduleId = await _boardingService.GetBoardingByTrafficScheduleId(
                _userService.User.Unit.Id, _userService.User.Unit.Code, Model.CarNumber, Model.TeamViewInfo.Id,
                _userService.User.Unit.IsJointUnit);

            Model.VehicleViewInfo = getBoardingByTrafficScheduleId.Response;

            await _popupNavigation.PopAllAsync();

            var success = Model.VehicleViewInfo != null && Model.VehicleViewInfo.Valid;

            if (success && !_userService.User.Unit.IsJointUnit && Model.VehicleViewInfo.Id > 0)
            {
                await NavigationService.NavigateAsync("RequestTrafficScheduleDateInputPopupPage",
                    new NavigationParameters()
                    {
                            { "Text", "Informar a data/hora da escala" },
                            { "TrafficScheduleDateTime", Model.VehicleViewInfo.TrafficScheduleDateTime }
                    });
            }
            else if (success && _userService.User.Unit.IsJointUnit
             && !await _notificationService.AskQuestionAsync(
                 $"Existe uma operação em andamento pelo usuário {Model.VehicleViewInfo?.UserLogin} " +
                    $"na data {Model.VehicleViewInfo?.PackingListDate.ToString("dd/MM/yy HH:mm")}.\n\n" +
                    $"Deseja continuar a operação?", 
                 SoundEnum.Alert))
            {
                Model.VehicleViewInfo = null;
                await _notificationService.NotifyAsync("Operação cancelada.", SoundEnum.Beep);
            }
            else if (success)
            {
                await SetPackingListBoarding();
                if (_userService.User.Unit.IsJointUnit)
                {
                    Model.VehicleDescription = Model.VehicleViewInfo?.UnitSendDescription;
                }
            }
            else
            {
                switch (Model.VehicleViewInfo?.ExceptionCode)
                {
                    case ExceptionCodeEnum.VehicleNotBoardingLanding:
                    case ExceptionCodeEnum.VehicleInBoardingPaused:
                    case ExceptionCodeEnum.LandingUnlicensed:
                        if (!await _notificationService.AskQuestionAsync(
                            $"{Model.VehicleViewInfo?.ExceptionMessage}\r\nDeseja iniciar embarque?",
                            SoundEnum.Alert))
                            break;

                        if (_userService.User.Unit.IsJointUnit)
                        {
                            var getJointTrafficSchedule = await _boardingService.GetJointTrafficSchedule(
                                _userService.User.Unit.Id);

                            if (getJointTrafficSchedule.Response != null && !getJointTrafficSchedule.Response.Valid)
                            {
                                await _notificationService.NotifyAsync("Falha ao criar controle operacional.",
                                    SoundEnum.Erros);
                                break;
                            }
                        }

                        var startingBoarding = await _boardingService.StartingBoarding(new StartingBoardingModel(
                            Model.VehicleViewInfo.OperationalControlId, _userService.User.Unit.Id,
                            Model.VehicleViewInfo.TrafficScheduleDetailId, Model.Team, _userService.User.Unit.IsJointUnit));

                        if (startingBoarding.Response != null && startingBoarding.Response.Valid)
                        {
                            Model.VehicleViewInfo.OperationalControlId = startingBoarding.Response.OperationalControlId;

                            if (_userService.User.Unit.IsJointUnit)
                            {
                                getBoardingByTrafficScheduleId = await _boardingService.GetBoardingByTrafficScheduleId(
                                    _userService.User.Unit.Id, _userService.User.Unit.Code, Model.CarNumber, Model.TeamViewInfo.Id,
                                    _userService.User.Unit.IsJointUnit);

                                if (getBoardingByTrafficScheduleId.Response == null ||
                                    !getBoardingByTrafficScheduleId.Response.Valid)
                                    break;
                            }

                            Model.VehicleViewInfo = getBoardingByTrafficScheduleId.Response;

                            Model.CarNumber = getBoardingByTrafficScheduleId.Response.CarNumber;

                            await SetPackingListBoarding();
                        }
                        else if (startingBoarding.Response != null)
                        {
                            await _notificationService.NotifyAsync(startingBoarding.Response.ExceptionMessage,
                                SoundEnum.Erros);
                        }
                        else
                        {
                            await _notificationService.NotifyAsync("Não foi possível completar a requsição.",
                                SoundEnum.Erros);
                        }
                        break;
                    default:
                        Model.ClearModelAfterTeam();
                        var msg = ((Model.VehicleViewInfo == null)
                            ? $"Frota {Model.CarNumber} inválida."
                            : Model.VehicleViewInfo.ExceptionMessage);

                        await _notificationService.NotifyAsync(msg, SoundEnum.Erros);
                        break;
                }
            }
        }

        #endregion

        #region Popups Calls
        private async void CallRequestUnitInputPopupPage()
        {
            await NavigationService.NavigateAsync("RequestUnitInputPopupPage", new NavigationParameters()
            {
                { "Text", "Unidade Próximo Destino" },
                { "ValidUnitType", ValidUnitTypeEnum.Destination }
            });
        }

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

        private async void TrafficScheduleDateConfirmed(bool confirmed)
        {
            if (confirmed)
            {
                await SetPackingListBoarding();
            }
            else
            {
                Model.ClearModelAfterTeam();
                Model.CarNumberFocus();
            }
        }

        private void WarehousePasswordInputConfirmed(bool confirmed, string warehousePassword, int warehousePasswordId)
        {
            if (confirmed)
            {
                Model.WarehousePassword = warehousePassword;
                Model.WarehousePasswordId = warehousePasswordId;
                CallRequestUnitInputPopupPage();
            }
            else
            {
                Model.ClearModelAfterTeam();
            }
        }

        private async void RequestUnitInputConfirmed(bool confirmed, UnitViewInfoModel unitDestinationViewInfo)
        {
            Model.UnitDestinationViewInfo = unitDestinationViewInfo;
            if (confirmed)
            {
                ReadingPackBoarding(new ReadingPackBoardingModel(false,
                    Model.TrafficScheduleDetailId, Model.TeamViewInfo.Id, Model.TrafficScheduleDetailVersionId, Model.TrafficScheduleDetailSequence,
                    Model.PackingListViewInfo.Id, _userService.User.Unit.Id, Model.UnitDestinationViewInfo.Id, Model.Reading,
                    _wifiService.MacAddress, _userService.User.Unit.IsJointUnit, false, Model.WarehousePasswordId, true));
            }
            else
            {
                Model.ClearModelAfterTeam();

                if (!string.IsNullOrEmpty(Model.WarehousePassword))
                    await _commonService.ReleaseWarehousePassword(Model.WarehousePassword);
            }
        }

        private void ConfirmationRandomConfirmed(bool confirmed, Dictionary<string, object> callBackData)
        {
            if (confirmed)
            {
                if (callBackData.TryGetValue("Action", out object action)
                    && callBackData.TryGetValue("Model", out object model))
                {
                    var cancelPackBoarding = action as Action<CancelPackBoardingModel>;
                    var cancelBillOfLadingBoarding = action as Action<CancelBillOfLadingBoardingModel>;
                    var endingProcess = action as Action<EndingBoardingModel>;
                    var defaultAction = action as Action;

                    if (cancelPackBoarding != null)
                    {
                        var cancelPackBoardingModel = (CancelPackBoardingModel) model;

                        cancelPackBoarding(cancelPackBoardingModel);
                    }
                    else if (cancelBillOfLadingBoarding != null)
                    {
                        var cancelBillOfLadingBoardingModel = (CancelBillOfLadingBoardingModel) model;

                        cancelBillOfLadingBoarding(cancelBillOfLadingBoardingModel);
                    }
                    else if (endingProcess != null)
                    {
                        var endingBoardingModel = (EndingBoardingModel) model;

                        endingProcess(endingBoardingModel);
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

        #endregion

        #region Actions

        public Action<CancelPackBoardingModel> CancelPackBoarding;

        private async void ExecuteCancelPackBoarding(CancelPackBoardingModel model)
        {
            var packingListViewInfo = await _boardingService.CancelPackBoarding(model);

            if (packingListViewInfo.Response != null && !packingListViewInfo.Response.Valid)
                await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage,
                    SoundEnum.Erros);
            else
                await SetPackingListBoarding();
        }

        public Action<CancelBillOfLadingBoardingModel> CancelBillOfLadingBoarding;

        private async void ExecuteCancelBillOfLadingBoarding(CancelBillOfLadingBoardingModel model)
        {
            var packingListViewInfoModel = await _boardingService.CancelBillOfLadingBoarding(model);

            if (packingListViewInfoModel.Response != null && !packingListViewInfoModel.Response.Valid)
                await _notificationService.NotifyAsync(packingListViewInfoModel.Response.ExceptionMessage,
                    SoundEnum.Erros);
            else
                await SetPackingListBoarding();
        }

        public Action PauseBoarding;

        private async void ExecutePauseBoarding()
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

        public Action<EndingBoardingModel> EndingProcess;

        private async void ExecuteEndingProcess(EndingBoardingModel model)
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());
            var packingListViewInfo = await _boardingService.EndingBoarding(model);
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
                switch (packingListViewInfo.Response.ExceptionCode)
                {
                    case ExceptionCodeEnum.IncompleteBoardingBol:
                        if (await _notificationService.AskQuestionAsync(packingListViewInfo.Response.ExceptionMessage,
                            SoundEnum.Alert))
                        {
                            EndingProcess(new EndingBoardingModel(model.PackingListId, model.OperationalControlId,
                                model.MacAddress, true));
                        }
                        break;
                    default:
                        await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
                        break;
                }
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

        private DelegateCommand _viewLackCommand;
        public DelegateCommand ViewLackCommand =>
            _viewLackCommand ?? (_viewLackCommand = new DelegateCommand(ViewLackCommandHandler));

        private DelegateCommand _viewLackByProcessCommand;
        public DelegateCommand ViewLackByProcessCommand =>
            _viewLackByProcessCommand ?? (_viewLackByProcessCommand = new DelegateCommand(ViewLackByProcessCommandHandler));

        private DelegateCommand _listBoardingWeightCommand;
        public DelegateCommand ListBoardingWeightCommand =>
            _listBoardingWeightCommand ?? (_listBoardingWeightCommand = new DelegateCommand(ListBoardingWeightCommandHandler));

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
                var team = await _commonService.ValidTeam(_userService.User.Unit.Id, Model.Team);

                Model.TeamViewInfo = team.Response;

                var success = Model.TeamViewInfo != null && Model.TeamViewInfo.Active && Model.TeamViewInfo.Valid;
                if (success)
                {
                    Model.TeamDescription = Model.TeamViewInfo.Description;
                    Model.ClearModelAfterTeam();

                    if (Model.Processo != ProcessoEnum.LeituraUnica)
                    {
                        Model.CarNumberFocus();
                    }
                    else
                    {
                        SetPageDefaultsByProcess();
                        GetTrafficScheduleBoarding();
                    }
                }
                else
                {
                    Model.Team = String.Empty;
                    await _notificationService.NotifyAsync(
                        Model.TeamViewInfo?.ExceptionMessage ?? "Equipe não encontrada.", SoundEnum.Erros);
                }
            }
        }

        private void CarChangedCommandHandler()
        {
            if (Model.TeamViewInfo != null && !string.IsNullOrEmpty(Model.CarNumber))
            {
                GetTrafficScheduleBoarding();
            }
        }

        private async void ConfirmationCommandHandler()
        {
            if (Model.IsValid())
            {
                if (Model.PackingListViewInfo.TrafficScheduleDetailId != 0)
                {
                    Model.TrafficScheduleDetailId = Model.PackingListViewInfo.TrafficScheduleDetailId;
                    Model.TrafficScheduleDetailVersionId = Model.PackingListViewInfo.TrafficScheduleDetailVersionId;
                    Model.TrafficScheduleDetailSequence = Model.PackingListViewInfo.TrafficScheduleDetailSequence;
                }
                else
                {
                    Model.TrafficScheduleDetailId = Model.VehicleViewInfo.TrafficScheduleDetailId;
                    Model.TrafficScheduleDetailVersionId = Model.VehicleViewInfo.TrafficScheduleDetailVersionId;
                    Model.TrafficScheduleDetailSequence = Model.VehicleViewInfo.TrafficScheduleDetailSequence;
                }

                ReadingPackBoarding(new ReadingPackBoardingModel(false,
                    Model.TrafficScheduleDetailId, Model.TeamViewInfo.Id, Model.TrafficScheduleDetailVersionId, Model.TrafficScheduleDetailSequence,
                    Model.PackingListViewInfo.Id, _userService.User.Unit.Id, -1, Model.Reading,
                    _wifiService.MacAddress, _userService.User.Unit.IsJointUnit, false, 0, true));
            }
            else
            {
                var msg = ((string.IsNullOrEmpty(Model.Reading))
                    ? "Necessário informar o código de barras."
                    : "Informe todos os campos obrigatórios antes de confirmar.");
                await _notificationService.NotifyAsync(msg, SoundEnum.Erros);
            }
        }

        private async void ClearCommandHandler()
        {
            if (Model.Processo != ProcessoEnum.LeituraUnica
            || string.IsNullOrEmpty(Model.TrafficSchedule)
            || await _notificationService.AskQuestionAsync(
                "O processo de leitura única não foi concluído. Deseja realmente sair?",
                SoundEnum.Alert))
            {
                Model.ClearModel();
                SetPageDefaultsByProcess();
                MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
            }
        }

        protected override async void GoBackCommandHandler()
        {
            if (Model.Processo != ProcessoEnum.LeituraUnica
                || string.IsNullOrEmpty(Model.TrafficSchedule)
                || await _notificationService.AskQuestionAsync(
                    "O processo de leitura única não foi concluído. Deseja realmente sair?",
                    SoundEnum.Alert))
            {
                await NavigationService.GoBackAsync();
            }
        }

        #region Menu Command Handleres

        private async void ViewLackCommandHandler()
        {
            if (Model.VehicleViewInfo != null 
            && Model.PackingListViewInfo != null)
            {
                var viewLackModel = new ViewLackModel()
                {
                    LackType = LackTypeEnum.Boarding,
                    PackingListId = Model.PackingListViewInfo.Id,
                    Vehicle = ((Model.VehicleViewInfo.Id > 0) ? Model.VehicleViewInfo.Id.ToString() : String.Empty),
                    VehicleDescription = Model.VehicleViewInfo.Plate,
                    UnitSent = _userService.User.Unit.Code,
                    UnitSentDescription = _userService.User.Unit.Description,
                    UnitDestination = Model.Unit,
                    UnitDestinationDescription = Model.UnitDestination,
                    Date = Model.VehicleViewInfo?.TrafficScheduleDateTime.ToString("dd/MM/yyyy HH:mm"),
                    TrafficSchedule = Model.VehicleViewInfo.TrafficScheduleId.ToString(),
                    Line = Model.VehicleViewInfo.LineCode
                };

                if (!_userService.User.Unit.IsJointUnit)
                {
                    viewLackModel.VehicleLineIsVisible = viewLackModel.LineIsVisible = true;
                }
                else
                {
                    viewLackModel.VehicleLineIsVisible = viewLackModel.LineIsVisible = false;
                }

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
                    LackType = LackTypeEnum.Boarding,
                    PackingListId = Model.PackingListViewInfo.Id,
                    VehicleViewInfo = Model.VehicleViewInfo
                };

                await NavigationService.NavigateAsync("ViewLackByProcessPage", new NavigationParameters()
                {
                    {"ViewLackByProcessModel", viewLackByProcessModel}
                });
            }
        }

        private async void ListBoardingWeightCommandHandler()
        {
            if (Model.VehicleViewInfo != null && Model.PackingListViewInfo != null)
            {
                var packingListBoardingWeightViewModel = new PackingListBoardingWeightViewModel()
                {
                    CarNumber = Model.VehicleViewInfo.CarNumber,
                    VehicleDescription = Model.VehicleViewInfo.VehicleType,
                    UnitDestination = Model.VehicleViewInfo.UnitSendCode,
                    TrafficSchedule = Model.VehicleViewInfo.TrafficScheduleId.ToString(),
                    Date = Model.VehicleViewInfo.CheckInDateView.ToString("dd/MM/yyyy HH:mm"),
                    Line = Model.VehicleViewInfo.LineCode,
                    PackAmount = Model.PackingListViewInfo.TotalBillOfLading.ToString(),
                    PackReading = Model.PackingListViewInfo.TotalPack.ToString(),
                    VehicleTotalCapacity = $"{Model.PackingListViewInfo.WeightCapacityVehicle:N2}",
                    PackinglistTotalProportionalWeight = $"{Model.PackingListViewInfo.WeightPackingList:N2}"
                };

                await NavigationService.NavigateAsync("PackingListBoardingWeightViewPage", new NavigationParameters()
                {
                    {"PackingListBoardingWeightViewModel", packingListBoardingWeightViewModel}
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
                && await _notificationService.AskQuestionAsync("Deseja pausar o processo de embarque?",
                    SoundEnum.Alert))
            {
                await NavigationService.NavigateAsync("ConfirmationRandomPopupPage", new NavigationParameters()
                {
                    {
                        "CallBackData",
                        new Dictionary<string, object>()
                        {
                            {"Action", PauseBoarding},
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
                && await _notificationService.AskQuestionAsync("Deseja terminar o processo de embarque?",
                    SoundEnum.Alert))
            {
                var model = new EndingBoardingModel(Model.PackingListViewInfo.PackingListId,
                    Model.VehicleViewInfo.OperationalControlId, _wifiService.MacAddress, false);

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
