using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Enums;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.ViewModels
{
    public class PackingListDeliveryBoardingInputPageViewModel : ViewModelBase
    {
        public PackingListDeliveryBoardingInputPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService,
            IPopupNavigation popupNavigation,
            IUserService userService,
            ICommonService commonService,
            IBoardingDeliveryPackService boardingDeliveryPackService,
            IWifiService wifiService,
            IBoardingService boardingService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;
            _popupNavigation = popupNavigation;
            _userService = userService;
            _commonService = commonService;
            _boardingDeliveryPackService = boardingDeliveryPackService;
            _wifiService = wifiService;
            _boardingService = boardingService;

            Model = new PackingListDeliveryBoardingInputModel();

            CancelPackBoarding += ExecuteCancelPackBoarding;
            CancelBillOfLadingBoarding += ExecuteCancelBillOfLadingBoarding;
            EndingProcess += ExecuteEndingProcess;

            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
        }

        private readonly INotificationService _notificationService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;
        private readonly IBoardingDeliveryPackService _boardingDeliveryPackService;
        private readonly IWifiService _wifiService;
        private readonly IBoardingService _boardingService;

        private PackingListDeliveryBoardingInputModel _model;
        public PackingListDeliveryBoardingInputModel Model
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
            if (parameters.TryGetValue("ConfirmationRandomConfirmed", out bool confirmed))
            {
                parameters.TryGetValue("CallBackData", out Dictionary<string, object> callBackData);
                ConfirmationRandomConfirmed(confirmed, callBackData);
            }
        }

        #endregion

        #region Private Methods

        private void AddMenuAdtionalBunttons()
        {
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>()
            {
                { "Consulta Faltas", FindLackCommand },
                { "Término", EndingProcessCommand }
            };
        }

        private async void GetPackingListDelivery()
        {
            if (Model.CheckOutDate.HasValue
            && !string.IsNullOrEmpty(Model.Line) 
            && !string.IsNullOrEmpty(Model.Driver) 
            && !string.IsNullOrEmpty(Model.Vehicle))
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var packingListViewInfo = await _boardingDeliveryPackService.GetPackingListDelivery(
                    new GetPackingListDeliveryModel(_userService.User.Unit.Id, Model.CheckOutDate.Value.ToString("dd/MM/yyyy HH:mm"),
                        Model.LineInfoView.Id, Model.VehicleViewInfo.Id, Model.DriverInfoView.Id,
                        _wifiService.MacAddress, true));

                await _popupNavigation.PopAllAsync();

                if (packingListViewInfo.Response != null 
                && packingListViewInfo.Response.Id > 0 
                && packingListViewInfo.Response.Valid
                && packingListViewInfo.Response.ExceptionCode == ExceptionCodeEnum.NoError)
                {
                    SetPackingListDelivery(packingListViewInfo.Response);
                }
                else if (packingListViewInfo.Response != null)
                {
                    switch (packingListViewInfo.Response.ExceptionCode)
                    {
                        case ExceptionCodeEnum.ExistsPackinglistDelivery:
                            if (await _notificationService.AskQuestionAsync(
                                "Existe outro romaneio aberto com essas configurações.\nDeseja utilizá-lo?", SoundEnum.Alert))
                            {
                                SetPackingListDelivery(packingListViewInfo.Response);
                            }
                            else
                            {
                                Model.PackingListViewInfo = null;
                                Model.ClearModelAfterDriver();
                                await _notificationService.NotifyAsync("Operação cancelada.", SoundEnum.Alert);
                            }
                            break;
                        default:
                            Model.PackingListViewInfo = null;
                            Model.ClearModelAfterDriver();
                            await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
                            break;
                    }
                }
            }
            else
            {
                Model.PackingListViewInfo = null;
                Model.ClearModelAfterVehicle();
            }
        }

        private void SetPackingListDelivery(PackingListViewInfoModel packingListViewInfo)
        {
            Model.PackingListViewInfo = packingListViewInfo;

            Model.DateIsReadOnly = Model.TimeIsReadOnly =
                Model.LineIsReadOnly = Model.DriverIsReadOnly = Model.VehicleIsReadOnly = true;

            Model.ReadingIsReadOnly = false;
            Model.Reading = String.Empty;
            Model.ReadingFocus();

            if (MenuAdtionalButtons == null || !MenuAdtionalButtons.Any())
            {
                AddMenuAdtionalBunttons();
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

        private void ConfirmationRandomConfirmed(bool confirmed, Dictionary<string, object> callBackData)
        {
            if (confirmed)
            {
                if (callBackData.TryGetValue("Action", out object action)
                    && callBackData.TryGetValue("Model", out object model))
                {
                    var cancelPackBoarding = action as Action<CancelPackBoardingModel>;
                    var cancelBillOfLadingBoarding = action as Action<CancelBillOfLadingBoardingModel>;
                    var endingProcess = action as Action;

                    if (cancelPackBoarding != null)
                    {
                        var cancelPackBoardingModel = (CancelPackBoardingModel)model;

                        cancelPackBoarding(cancelPackBoardingModel);
                    }
                    else if (cancelBillOfLadingBoarding != null)
                    {
                        var cancelBillOfLadingBoardingModel = (CancelBillOfLadingBoardingModel)model;

                        cancelBillOfLadingBoarding(cancelBillOfLadingBoardingModel);
                    }
                    else if (endingProcess != null)
                    {
                        endingProcess();
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
            var cancelPackLanding = await _boardingService.CancelPackBoarding(model);

            if (cancelPackLanding.Response != null && cancelPackLanding.Response.Valid)
            {
                SetPackingListDelivery(cancelPackLanding.Response);
            }
            else
            {
                await _notificationService.NotifyAsync(cancelPackLanding.Response?.ExceptionMessage, SoundEnum.Erros);
            }
        }

        public Action<CancelBillOfLadingBoardingModel> CancelBillOfLadingBoarding;

        private async void ExecuteCancelBillOfLadingBoarding(CancelBillOfLadingBoardingModel model)
        {
            var cancelBillOfLadingLanding = await _boardingService.CancelBillOfLadingBoarding(model);

            if (cancelBillOfLadingLanding.Response != null && cancelBillOfLadingLanding.Response.Valid)
            {
                SetPackingListDelivery(cancelBillOfLadingLanding.Response);
            }
            else
            {
                await _notificationService.NotifyAsync(cancelBillOfLadingLanding.Response?.ExceptionMessage, SoundEnum.Erros);
            }
        }

        public Action EndingProcess;

        private async void ExecuteEndingProcess()
        {
            var packingListViewInfoModel = await _boardingDeliveryPackService.EndingDeliveryBoarding(
                new EndingDeliveryBoardingModel(Model.PackingListViewInfo.Id, _wifiService.MacAddress));

            if (packingListViewInfoModel.Response != null
                && packingListViewInfoModel.Response.Valid
                && packingListViewInfoModel.Response.ExceptionCode == ExceptionCodeEnum.NoError)
            {
                Model.ClearModel();
                await _notificationService.NotifyAsync("Processo finalizado.", SoundEnum.Alert);
            }
            else if (packingListViewInfoModel.Response != null)
            {
                await _notificationService.NotifyAsync(packingListViewInfoModel.Response.ExceptionMessage, SoundEnum.Erros);
            }
            else
            {
                await _notificationService.NotifyAsync("A requisição não pode ser completada.", SoundEnum.Erros);
            }
        }

        #endregion

        #region Commands

        private DelegateCommand _lineChangedCommand;
        public DelegateCommand LineChangedCommand =>
            _lineChangedCommand ?? (_lineChangedCommand = new DelegateCommand(LineChangedCommandHandler));

        private DelegateCommand _driverChangedCommand;
        public DelegateCommand DriverChangedCommand =>
            _driverChangedCommand ?? (_driverChangedCommand = new DelegateCommand(DriverChangedCommandHandler));

        private DelegateCommand _vhicleChangedCommand;
        public DelegateCommand VhicleChangedCommand =>
            _vhicleChangedCommand ?? (_vhicleChangedCommand = new DelegateCommand(VhicleChangedCommandHandler));

        private DelegateCommand _confirmationCommand;
        public DelegateCommand ConfirmationCommand =>
            _confirmationCommand ?? (_confirmationCommand = new DelegateCommand(ConfirmationCommandHandler));

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearCommandHandler));

        #region Menu Commands

        private DelegateCommand _findLackCommand;
        public DelegateCommand FindLackCommand =>
            _findLackCommand ?? (_findLackCommand = new DelegateCommand(FindLackCommandHandler));

        private DelegateCommand _endingProcessCommand;
        public DelegateCommand EndingProcessCommand =>
            _endingProcessCommand ?? (_endingProcessCommand = new DelegateCommand(EndingProcessCommandHandler));

        #endregion

        #endregion

        #region Command Handlers

        private async void LineChangedCommandHandler()
        {
            Model.LineInfoView = null;
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();

            if (!string.IsNullOrEmpty(Model.Line) && Model.CheckOutDate.HasValue)
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var lineInfoView =
                    await _boardingDeliveryPackService.ValidLine(_userService.User.Unit.Id, Model.Line,
                        Model.CheckOutDate.Value);

                await _popupNavigation.PopAllAsync();

                if (lineInfoView.Response != null && lineInfoView.Response.Valid && lineInfoView.Response.Active)
                {
                    Model.LineInfoView = lineInfoView.Response;

                    if (string.IsNullOrEmpty(Model.Driver))
                    {
                        Model.DriverFocus();
                    }
                    else
                    {
                        GetPackingListDelivery();
                    }
                }
                else if (lineInfoView.Response != null)
                {
                    if (lineInfoView.Response.Id > 0  && !lineInfoView.Response.Active)
                    {
                        await _notificationService.NotifyAsync($"Linha {Model.Line} inativa.", SoundEnum.Erros);
                    }
                    else
                    {
                        if (lineInfoView.Response.ExceptionMessage.Trim().ToUpper().Equals("DATA INFORMADA É ANTERIOR A DATA ATUAL!"))
                        {
                            Model.Date = Model.Time = String.Empty;
                        }
                        await _notificationService.NotifyAsync(lineInfoView.Response.ExceptionMessage, SoundEnum.Erros);
                    }
                    Model.Line = String.Empty;
                }
                else
                {
                    await _notificationService.NotifyAsync($"Linha {Model.Line} inválida.", SoundEnum.Erros);
                    Model.Line = String.Empty;
                }
            }
        }

        private async void DriverChangedCommandHandler()
        {
            Model.DriverInfoView = null;
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();

            if (!string.IsNullOrEmpty(Model.Driver))
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var driverInfoView = await _boardingDeliveryPackService.ValidDriver(Model.Driver.ToInt());

                await _popupNavigation.PopAllAsync();

                if (driverInfoView.Response != null && driverInfoView.Response.Valid)
                {
                    Model.DriverInfoView = driverInfoView.Response;

                    if (string.IsNullOrEmpty(Model.Vehicle))
                    {
                        Model.VehicleFocus();
                    }
                    else
                    {
                        GetPackingListDelivery();
                    }
                }
                else if (driverInfoView.Response != null)
                {
                    await _notificationService.NotifyAsync(driverInfoView.Response.ExceptionMessage, SoundEnum.Erros);
                    Model.Driver = String.Empty;
                }
                else
                {
                    await _notificationService.NotifyAsync($"Motorista {Model.Driver} inválido.", SoundEnum.Erros);
                    Model.Driver = String.Empty;
                }
            }
        }

        private async void VhicleChangedCommandHandler()
        {
            Model.VehicleViewInfo = null;
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();

            if (!string.IsNullOrEmpty(Model.Vehicle))
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var vehicleViewInfo = await _commonService.ValidVehicle(Model.Vehicle);

                await _popupNavigation.PopAllAsync();

                Model.VehicleViewInfo = vehicleViewInfo.Response;

                if (vehicleViewInfo.Response != null && vehicleViewInfo.Response.Valid)
                {
                    Model.VehicleViewInfo = vehicleViewInfo.Response;

                    GetPackingListDelivery();
                }
                else if(vehicleViewInfo.Response != null)
                {
                    await _notificationService.NotifyAsync(vehicleViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
                    Model.ClearModelAfterDriver();
                }
                else
                {
                    await _notificationService.NotifyAsync($"Veículo {Model.Vehicle} inválido.", SoundEnum.Erros);
                    Model.ClearModelAfterDriver();
                }
            }
        }

        private async void ConfirmationCommandHandler()
        {
            if (Model.IsValid())
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var packingListViewInfo = await _boardingDeliveryPackService.ReadingPackDelivery(
                    new ReadingPackDeliveryModel(Model.PackingListViewInfo.Id,
                        Model.PackingListViewInfo.TrafficScheduleDetailId, _userService.User.Unit.Id, Model.Reading,
                        _wifiService.MacAddress, false));

                await _popupNavigation.PopAllAsync();

                if (packingListViewInfo.Response != null && packingListViewInfo.Response.Valid)
                {
                    Model.VehicleViewInfo.OperationalControlId = packingListViewInfo.Response.OperationalControlId;
                    
                    SetPackingListDelivery(packingListViewInfo.Response);
                }
                else if (packingListViewInfo.Response != null)
                {
                    switch (packingListViewInfo.Response.ExceptionCode)
                    {
                        case ExceptionCodeEnum.PackRead:
                            if (await _notificationService.AskQuestionAsync(
                                $"{packingListViewInfo.Response.ExceptionMessage}\r\nDeseja estornar o volume registrado na leitura atual?",
                                SoundEnum.Alert))
                            {
                                CallConfirmationRandomPopupPage(CancelPackBoarding,
                                    new CancelPackBoardingModel(Model.PackingListViewInfo.Id, Model.Reading,
                                        _wifiService.MacAddress, false, true));
                            }
                            else if (await _notificationService.AskQuestionAsync(
                                $"{packingListViewInfo.Response.ExceptionMessage}\r\nVeículo{ Model.VehicleViewInfo.CarNumber}" +
                                $"\r\nDeseja estornar todo o CTRC do volume registrado na leitura atual?",
                                SoundEnum.Alert))
                            {
                                CallConfirmationRandomPopupPage(CancelBillOfLadingBoarding,
                                    new CancelBillOfLadingBoardingModel(Model.PackingListViewInfo.Id, Model.Reading,
                                        _wifiService.MacAddress, false));
                            }
                            break;
                        //case ExceptionCodeEnum.PackNotFoundWarehouse:
                        //    if (await ShowQuestionDialogSpeech(packingListViewInfo.Response.ExceptionMessage))
                        //        return ReadingPackDeliveryBoarding(trafficScheduleDetailId, packingListId, unitLocal, textBoxBarCode, vehicle, packAmount, packAmountReading, true);
                        //    break;

                        default:
                            await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
                            break;
                    }
                }
            }
        }

        private void ClearCommandHandler()
        {
            Model.ClearModel();
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
        }

        #region Menu Command Handlers

        private async void FindLackCommandHandler()
        {
            if (Model.PackingListViewInfo != null)
            {
                var packingListDetailViewInfo = await _boardingDeliveryPackService.GetViewLackDeliveryBoardingByTrafficScheduleDetail(
                    Model.PackingListViewInfo.PackingListId, _userService.User.Unit.Id,
                    Model.PackingListViewInfo.TrafficScheduleDetailId);

                if (packingListDetailViewInfo.Response != null && packingListDetailViewInfo.Response.Any())
                {
                    await NavigationService.NavigateAsync("ViewLackDetailPage", new NavigationParameters()
                    {
                        { "Title", "VOLUMES FALTANTES" },
                        { "PackingListDetailViewInfo", packingListDetailViewInfo.Response }
                    });
                }
                else
                {
                    await _notificationService.NotifyAsync("Não foi possível completar a requisição.", SoundEnum.Erros);
                }
            }
        }

        private async void EndingProcessCommandHandler()
        {
            if (await _notificationService.AskQuestionAsync("Deseja terminar o processo de embarque", SoundEnum.Alert))
            {
                await NavigationService.NavigateAsync("ConfirmationRandomPopupPage", new NavigationParameters()
                {
                    {
                        "CallBackData",
                        new Dictionary<string, object>()
                        {
                            { "Action", EndingProcess },
                            { "Model", new object() }
                        }
                    }
                });
            }
        }

        #endregion

        #endregion
    }
}
