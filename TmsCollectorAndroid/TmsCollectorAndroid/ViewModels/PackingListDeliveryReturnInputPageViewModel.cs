using System;
using System.Linq;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.ViewModels
{
    public class PackingListDeliveryReturnInputPageViewModel : ViewModelBase
    {
        public PackingListDeliveryReturnInputPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            IUserService userService,
            INotificationService notificationService,
            IPopupNavigation popupNavigation,
            ICommonService commonService,
            IBoardingDeliveryPackService boardingDeliveryPackService,
            ILandingDeliveryPackService landingDeliveryPackService,
            IWifiService wifiService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _userService = userService;
            _notificationService = notificationService;
            _popupNavigation = popupNavigation;
            _commonService = commonService;
            _boardingDeliveryPackService = boardingDeliveryPackService;
            _landingDeliveryPackService = landingDeliveryPackService;
            _wifiService = wifiService;

            Model = new PackingListDeliveryReturnInputModel();
        }

        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly ICommonService _commonService;
        private readonly IBoardingDeliveryPackService _boardingDeliveryPackService;
        private readonly ILandingDeliveryPackService _landingDeliveryPackService;
        private readonly IWifiService _wifiService;

        private PackingListDeliveryReturnInputModel _model;
        public PackingListDeliveryReturnInputModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        #region Navigation Methods

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("RequestPackingListAccessoryViewConfirmed", out bool confirmed))
            {
                parameters.TryGetValue("PackingListViewInfo", out PackingListViewInfoModel packingListViewInfo);
                RequestPackingListAccessoryViewConfirmed(confirmed, packingListViewInfo);
            }
        }

        #endregion

        #region Private Methods

        private async void SetPackingListView(PackingListViewInfoModel packingListViewInfo)
        {
            Model.PackingListViewInfo = packingListViewInfo;

            if (packingListViewInfo != null && packingListViewInfo.Valid)
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                Model.BolAmountView = packingListViewInfo.TotalBillOfLading.ToString();
                Model.PacksAmountView = packingListViewInfo.TotalPack.ToString();
                Model.PackingListNumber = packingListViewInfo.Number.ToString();
                Model.PackingListDigit = packingListViewInfo.Digit.ToString();
                if (DateTime.TryParse(packingListViewInfo.TrafficScheduleDetailCheckoutDate, out DateTime checkOutDate))
                {
                    Model.Date = checkOutDate.ToString("dd/MM/yyyy");
                    Model.Time = checkOutDate.ToString("HH:mm");
                }
               
                var driverAndVehicle =
                    await _landingDeliveryPackService.GetDriverAndVehicle(packingListViewInfo.Id);

                if (driverAndVehicle.Response?.DriverAndVehicle != null
                    && driverAndVehicle.Response.DriverAndVehicle.Count() == 2)
                {
                    Model.Driver = driverAndVehicle.Response.DriverAndVehicle.FirstOrDefault();
                    Model.Vehicle = driverAndVehicle.Response.DriverAndVehicle.LastOrDefault();

                    var driverInfoView = await _boardingDeliveryPackService.ValidDriver(Model.Driver.ToInt());
                    var vehicleViewInfo = await _commonService.ValidVehicle(Model.Vehicle);

                    Model.DriverInfoView = driverInfoView.Response;
                    Model.VehicleViewInfo = vehicleViewInfo.Response;

                    if (driverInfoView.Response != null)
                        Model.DriverDescription = driverInfoView.Response.Descritpion;

                    if (vehicleViewInfo.Response != null)
                        Model.VehicleView = vehicleViewInfo.Response.Plate;
                }

                await _popupNavigation.PopAllAsync();

                if (packingListViewInfo.TotalBillOfLading == 0)
                    await _notificationService.NotifyAsync("Não houve nenhum motivo de não entrega para esse romaneio. Por isso não há retornos.", SoundEnum.Alert);

                Model.PackingListNumberIsReadOnly = Model.PackingListDigitIsReadOnly = Model.DateIsReadOnly =
                    Model.TimeIsReadOnly = Model.DriverIsReadOnly = Model.VehicleIsReadOnly = true;

                Model.Reading = String.Empty;
                Model.ReadingIsReadOnly = false;
                Model.ReadingFocus();
            }
            else if (packingListViewInfo != null)
            {
                await _notificationService.NotifyAsync(packingListViewInfo.ExceptionMessage, SoundEnum.Erros);
            }
        }

        #endregion

        #region Popup Return Handlers

        private void RequestPackingListAccessoryViewConfirmed(bool confirmed, PackingListViewInfoModel packingListViewInfo)
        {
            if (confirmed)
            {
                SetPackingListView(packingListViewInfo);
            }
            else
            {
                Model.ClearModel();
            }
        }

        #endregion

        #region Commands

        private DelegateCommand _packingListNumberChangedCommand;
        public DelegateCommand PackingListNumberChangedCommand =>
            _packingListNumberChangedCommand ?? (_packingListNumberChangedCommand = new DelegateCommand(PackingListNumberChangedCommandHandler));

        private DelegateCommand _packingListDigitChangedCommand;
        public DelegateCommand PackingListDigitChangedCommand =>
            _packingListDigitChangedCommand ?? (_packingListDigitChangedCommand = new DelegateCommand(PackingListDigitChangedCommandHandler));

        private DelegateCommand _timeChangedCommand;
        public DelegateCommand TimeChangedCommand =>
            _timeChangedCommand ?? (_timeChangedCommand = new DelegateCommand(TimeChangedCommandHandler));

        private DelegateCommand _driverChangedCommand;
        public DelegateCommand DriverChangedCommand =>
            _driverChangedCommand ?? (_driverChangedCommand = new DelegateCommand(DriverChangedCommandHandler));

        private DelegateCommand _vehicleChangedCommand;
        public DelegateCommand VehicleChangedCommand =>
            _vehicleChangedCommand ?? (_vehicleChangedCommand = new DelegateCommand(VehicleChangedCommandHandler));

        private DelegateCommand _confirmationCommand;
        public DelegateCommand ConfirmationCommand =>
            _confirmationCommand ?? (_confirmationCommand = new DelegateCommand(ConfirmationCommandHandler));

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearCommandHandler));

        #endregion

        #region Command Handlers

        private void PackingListNumberChangedCommandHandler()
        {
            var packingListNumber = Model.PackingListNumber;

            Model.ClearModel();

            Model.PackingListNumber = packingListNumber;

            Model.PackingListDigitFocus();
        }

        private async void PackingListDigitChangedCommandHandler()
        {
            if (!string.IsNullOrEmpty(Model.PackingListNumber) 
            && !string.IsNullOrEmpty(Model.PackingListDigit))
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var lstPackingListDelivery = await _landingDeliveryPackService.GetListPackingListDeliveryByReturn(
                    _userService.User.Unit.Id, Model.PackingListNumber.ToInt(), Model.PackingListDigit.ToInt());

                await _popupNavigation.PopAllAsync();

                if (lstPackingListDelivery.Response != null && lstPackingListDelivery.Response.Any())
                {
                    if (lstPackingListDelivery.Response.Count() > 1)
                    {
                        await NavigationService.NavigateAsync("RequestPackingListAccessoryViewPopupPage",
                            new NavigationParameters() {{"PackingList", lstPackingListDelivery.Response}});
                    }
                    else
                    {
                        SetPackingListView(lstPackingListDelivery.Response.FirstOrDefault());
                    }
                }
                else
                    await _notificationService.NotifyAsync("Nenhum registro encontrado.", SoundEnum.Alert);
            }
            else
            {
                Model.DateFocus();
            }
        }

        private void TimeChangedCommandHandler()
        {
            if (string.IsNullOrEmpty(Model.PackingListNumber) 
            && string.IsNullOrEmpty(Model.PackingListDigit)
            && !string.IsNullOrEmpty(Model.Date)
            && !string.IsNullOrEmpty(Model.Time))
            {
                Model.DriverIsReadOnly = false;
                Model.DriverFocus();
            }
        }

        private async void DriverChangedCommandHandler()
        {
            Model.DriverDescription = String.Empty;

            if (!string.IsNullOrEmpty(Model.Driver))
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var driverInfoView = await _boardingDeliveryPackService.ValidDriver(Model.Driver.ToInt());

                await _popupNavigation.PopAllAsync();

                Model.DriverInfoView = driverInfoView.Response;

                if (Model.DriverInfoView != null && Model.DriverInfoView.Valid)
                {
                    Model.DriverDescription = Model.DriverInfoView.Descritpion;
                    Model.VehicleIsReadOnly = false;
                    Model.VehicleFocus();
                }
                else
                {
                    await _notificationService.NotifyAsync(
                        ((Model.DriverInfoView != null)
                            ? Model.DriverInfoView.ExceptionMessage
                            : $"Motorista {Model.Driver} inválido."), SoundEnum.Erros);
                }
            }
        }

        private async void VehicleChangedCommandHandler()
        {
            Model.VehicleView = String.Empty;

            if (!string.IsNullOrEmpty(Model.Vehicle))
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var vehicleViewInfo = await _commonService.ValidVehicle(Model.Vehicle);

                Model.VehicleViewInfo = vehicleViewInfo.Response;

                if (Model.VehicleViewInfo != null && Model.VehicleViewInfo.Valid)
                {
                    Model.VehicleView = Model.VehicleViewInfo.Plate;

                    var lstPackingListDelivery = await _landingDeliveryPackService.GetListPackingListDeliveryByReturn(
                        _userService.User.Unit.Id, Model.CheckOutDate.Value, Model.DriverInfoView.Id, Model.VehicleViewInfo.Id);

                    await _popupNavigation.PopAllAsync();

                    if (lstPackingListDelivery.Response != null && lstPackingListDelivery.Response.Any())
                    {
                        if (lstPackingListDelivery.Response.Count() > 1)
                        {
                            await NavigationService.NavigateAsync("RequestPackingListAccessoryViewPopupPage",
                                new NavigationParameters() { { "PackingList", lstPackingListDelivery.Response } });
                        }
                        else
                        {
                            SetPackingListView(lstPackingListDelivery.Response.FirstOrDefault());
                        }
                    }
                    else
                        await _notificationService.NotifyAsync("Nenhum registro encontrado.", SoundEnum.Alert);
                }
                else
                {
                    await _popupNavigation.PopAllAsync();
                    await _notificationService.NotifyAsync(((Model.VehicleViewInfo != null)
                        ? Model.VehicleViewInfo.ExceptionMessage
                        : $"Veículo {Model.Vehicle} inválido."));
                }
            }
        }

        private async void ConfirmationCommandHandler()
        {
            if (Model.IsValid())
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var packingListViewInfo = await _landingDeliveryPackService.ReadingPackDeliveryReturn(
                    new ReadingPackDeliveryReturnModel(Model.PackingListViewInfo.Id, Model.Reading,
                        _wifiService.MacAddress));

                await _popupNavigation.PopAllAsync();

                if (packingListViewInfo.Response != null)
                {
                    if (packingListViewInfo.Response.TotalBillOfLading == 0)
                        await _notificationService.NotifyAsync(
                            "Não houve nenhum motivo de não entrega para esse romaneio. Por isso não há retornos.",
                            SoundEnum.Alert);

                    if (packingListViewInfo.Response.Valid)
                    {
                        Model.BolAmountView = packingListViewInfo.Response.TotalBillOfLading.ToString();
                        Model.PacksAmountView = packingListViewInfo.Response.TotalPack.ToString();
                        if (!string.IsNullOrEmpty(packingListViewInfo.Response.InformationMessage))
                            await _notificationService.NotifyAsync(packingListViewInfo.Response.InformationMessage, SoundEnum.Alert);
                    }
                    else if (!packingListViewInfo.Response.Valid)
                    {
                        await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
                    }

                    Model.Reading = String.Empty;
                    Model.ReadingFocus();
                }
                else
                {
                    await _notificationService.NotifyAsync("Não foi possível completar a requisição.", SoundEnum.Erros);
                }
            }
        }

        private void ClearCommandHandler()
        {
            Model.ClearModel();
        }

        #endregion
    }
}
