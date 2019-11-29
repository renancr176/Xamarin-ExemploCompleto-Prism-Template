using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
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
    public class PackingListAccessoriesInputPageViewModel : ViewModelBase
    {
        public PackingListAccessoriesInputPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            IUserService userService,
            INotificationService notificationService,
            IPopupNavigation popupNavigation,
            ICommonService commonService,
            IBoardingAccessoryService boardingAccessoryService,
            IWifiService wifiService,
            IBoardingService boardingService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _userService = userService;
            _notificationService = notificationService;
            _popupNavigation = popupNavigation;
            _commonService = commonService;
            _boardingAccessoryService = boardingAccessoryService;
            _wifiService = wifiService;
            _boardingService = boardingService;

            Model = new PackingListAccessoriesInputModel();

            ClosePackingListAccessory += ExecuteClosePackingListAccessory;
            CancelPackBoarding += ExecuteCancelPackBoarding;
            CancelBillOfLadingBoarding += ExecuteCancelBillOfLadingBoarding;
        }

        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly ICommonService _commonService;
        private readonly IBoardingAccessoryService _boardingAccessoryService;
        private readonly IWifiService _wifiService;
        private readonly IBoardingService _boardingService;

        private PackingListAccessoriesInputModel _model;
        public PackingListAccessoriesInputModel Model
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
            if (parameters.TryGetValue("RequestPalletsViewConfirmed", out bool confirmed))
            {
                parameters.TryGetValue("TransportAccessoryViewInfo", out TransportAccessoryViewInfoModel transportAccessoryViewInfo);
                RequestPalletsViewConfirmed(confirmed, transportAccessoryViewInfo);
            }

            if (parameters.TryGetValue("RequestPackingListAccessoryViewConfirmed", out confirmed))
            {
                parameters.TryGetValue("PackingListViewInfo", out PackingListViewInfoModel packingListViewInfo);
                RequestPackingListAccessoryViewConfirmed(confirmed, packingListViewInfo);
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

        #region Actions

        public Action<ValidClosePackingListAccessoryModel> ClosePackingListAccessory;

        private async void ExecuteClosePackingListAccessory(ValidClosePackingListAccessoryModel model)
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());

            var packingListViewInfo = await _boardingAccessoryService.ValidClosePackingListAccessory(model);

            if (packingListViewInfo.Response != null && packingListViewInfo.Response.Valid)
            {
                packingListViewInfo =
                    await _boardingAccessoryService.ClosePackingListAccessory(Model.PackingListViewInfo.Id,
                        new MacAddressModel(_wifiService.MacAddress));

                await _popupNavigation.PopAllAsync();

                if (packingListViewInfo.Response != null && packingListViewInfo.Response.Valid)
                {
                    await _notificationService.NotifyAsync("Romaneio fechado.", SoundEnum.Alert);
                    await NavigationService.GoBackAsync();
                }
                else
                {
                    var msg = ((packingListViewInfo.Response != null && !packingListViewInfo.Response.Valid)
                        ? packingListViewInfo.Response.ExceptionMessage
                        : "Não foi possivel completar a requisição.");

                    await _notificationService.NotifyAsync(msg, SoundEnum.Erros);
                }
            }
            else if (packingListViewInfo.Response != null && !packingListViewInfo.Response.Valid)
            {
                await _popupNavigation.PopAllAsync();

                switch (packingListViewInfo.Response.ExceptionCode)
                {
                    case ExceptionCodeEnum.ExistsPacksUnread:
                        await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Alert);
                        var ignoreBillOfLadings = model.IgnoreBillOfLadings.ToList();
                        ignoreBillOfLadings.Add(packingListViewInfo.Response.BillOfLadingId);
                        ClosePackingListAccessory(new ValidClosePackingListAccessoryModel(model.PackingListAccessoryId,
                            model.IgnoreAmountSealsSuperior, ignoreBillOfLadings, model.MacAddress));
                        break;
                    case ExceptionCodeEnum.AmountSealsInferior:
                        if (await _notificationService.AskQuestionAsync(packingListViewInfo.Response.ExceptionMessage,
                            SoundEnum.Alert))
                        {
                            await NavigationService.NavigateAsync("MaintenanceSealsInputPage",
                                new NavigationParameters()
                                {
                                    { "Title", "Solicitação" },
                                    {
                                        "MaintenanceSealsInputModel",
                                        new MaintenanceSealsInputModel()
                                        {
                                            OnlyConference = false,
                                            PackingListAccessoryId = model.PackingListAccessoryId,
                                            TransportAccessoryDoors = packingListViewInfo.Response.TransportAccessoryDoors
                                        }
                                    },
                                    { "CallBackData", new Dictionary<string, object>() {{ "Model", model }} }
                                }
                            );
                        }
                        break;
                    case ExceptionCodeEnum.PackinglistAccessoryEmpty:
                        await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Alert);

                        packingListViewInfo = await _boardingAccessoryService.DeletePackingListAccessory(Model.PackingListViewInfo.Id);

                        if (packingListViewInfo.Response != null && !packingListViewInfo.Response.Valid)
                        {
                            await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
                        }
                        break;
                    default:
                        await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
                        break;
                }
            }
            else
            {
                await _popupNavigation.PopAllAsync();
                await _notificationService.NotifyAsync("A requisição não pode ser completada.", SoundEnum.Erros);
            }
        }

        public Action<CancelPackBoardingModel> CancelPackBoarding;

        private async void ExecuteCancelPackBoarding(CancelPackBoardingModel model)
        {
            var packingListViewInfo = await _boardingService.CancelPackBoarding(model);
            await SetPackingListViewInfo(packingListViewInfo.Response);
        }

        public Action<CancelBillOfLadingBoardingModel> CancelBillOfLadingBoarding;

        private async void ExecuteCancelBillOfLadingBoarding(CancelBillOfLadingBoardingModel model)
        {
            var packingListViewInfo = await _boardingService.CancelBillOfLadingBoarding(model);
            await SetPackingListViewInfo(packingListViewInfo.Response);
        }

        #endregion

        #region Private Methods

        private void AddMenuAdtionalBunttons()
        {
            if (MenuAdtionalButtons == null || !MenuAdtionalButtons.Any())
            {
                MenuAdtionalButtons = new Dictionary<string, DelegateCommand>()
                {
                    {"Fechar Acessório", ClosePackingListAccessoryCommand}
                };
            }
        }

        private async void GetPackingListAccessory()
        {
            var isPallet = (Model.TransportAccessoryViewInfo?.AccessoryGroupKind != null &&
                            Model.TransportAccessoryViewInfo.AccessoryGroupKind.Equals("PLT"));

            if (isPallet)
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var packingListAccessories = await _boardingAccessoryService.GetListPackingListAccessoryByPallet(
                    new GetListPackingListAccessoryByPalletModel(_userService.User.Unit.Code, _userService.User.Unit.Id,
                        Model.UnitViewInfo.Id, Model.TransportAccessoryViewInfo.Id, _wifiService.MacAddress));

                await _popupNavigation.PopAllAsync();

                if (packingListAccessories.Response != null && packingListAccessories.Response.Any()
                && await _notificationService.AskQuestionAsync(
                    "Existe outro romaneio aberto com essas configurações.\nDeseja utilizá-lo?",
                    SoundEnum.Alert))
                {
                    await NavigationService.NavigateAsync("RequestPackingListAccessoryViewPopupPage",
                        new NavigationParameters() { { "PackingList", packingListAccessories.Response } });
                }
                else
                {
                    await _popupNavigation.PushAsync(new LoadingPopupPage());

                    var packingListView = await _boardingAccessoryService.GetPackingListAccessory(
                        new GetPackingListAccessoryModel(_userService.User.Unit.Code, _userService.User.Unit.Id,
                            Model.Destination, Model.UnitViewInfo.Id, Model.Accessory.ToInt(),
                            Model.TransportAccessoryViewInfo.Id, _wifiService.MacAddress, true));

                    await _popupNavigation.PopAllAsync();

                    await SetPackingListViewInfo(packingListView.Response);
                }
            }
            else
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var packingListView = await _boardingAccessoryService.GetPackingListAccessory(
                    new GetPackingListAccessoryModel(_userService.User.Unit.Code, _userService.User.Unit.Id,
                        Model.Destination, Model.UnitViewInfo.Id, Model.Accessory.ToInt(),
                        Model.TransportAccessoryViewInfo.Id, _wifiService.MacAddress, false));

                await _popupNavigation.PopAllAsync();

                await SetPackingListViewInfo(packingListView.Response);
            }
        }

        private async Task SetPackingListViewInfo(PackingListViewInfoModel packingListViewInfo)
        {
            Model.PackingListViewInfo = packingListViewInfo;

            if (Model.PackingListViewInfo != null && Model.PackingListViewInfo.Id >= 0 && Model.PackingListViewInfo.Valid)
            {
                AddMenuAdtionalBunttons();

                Model.Ctrc = Model.PackingListViewInfo.TotalBillOfLading.ToString();
                Model.Pack = Model.PackingListViewInfo.TotalPack.ToString();
                Model.CobolNumber = Model.PackingListViewInfo.CobolNumber;
                Model.DestinationIsReadOnly = Model.AccessoryIsReadOnly = true;
                Model.ReadingIsReadOnly = false;
            }
            else
            {
                MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
                var msg = ((packingListViewInfo?.ExceptionMessage != null) ? packingListViewInfo.ExceptionMessage : "A requisição não pode ser completada.");
                await _notificationService.NotifyAsync(msg, SoundEnum.Erros);
                Model.AccessoryFocus();
            }
        }

        private async Task ReadingPackAccessory(ReadingPackAccessoryModel model)
        {
            var packingListViewInfo = await _boardingAccessoryService.ReadingPackAccessory(model);

            if (packingListViewInfo.Response != null && packingListViewInfo.Response.Valid)
            {
                await SetPackingListViewInfo(packingListViewInfo.Response);
            }
            else if (packingListViewInfo.Response != null)
            {
                switch (packingListViewInfo.Response.ExceptionCode)
                {
                    case ExceptionCodeEnum.RequestUnitDestination:
                        await _notificationService.NotifyAsync("Opetação não implementada.", SoundEnum.Alert);
                        if (await _notificationService.AskQuestionAsync(packingListViewInfo.Response.ExceptionMessage, 
                            SoundEnum.Alert))
                            await ReadingPackAccessory(new ReadingPackAccessoryModel(model.IgnoreWarehouse,
                                model.PackingListAccessoryId, model.UnitLocal, _userService.User.Unit.Id, model.BarCode, model.MacAddress));
                        break;
                    case ExceptionCodeEnum.PackRead:
                        if (await _notificationService.AskQuestionAsync(
                                $"{packingListViewInfo.Response.ExceptionMessage}\r\nDeseja estornar o volume registrado na leitura atual?",
                                SoundEnum.Alert))
                        {
                            CallConfirmationRandomPopupPage(CancelPackBoarding,
                                new CancelPackBoardingModel(model.PackingListAccessoryId, model.BarCode,
                                    model.MacAddress, false, false));
                        }
                        else if (await _notificationService.AskQuestionAsync(
                            $"{packingListViewInfo.Response.ExceptionMessage}\r\nDeseja estornar todo o CTRC do volume registrado na leitura atual?",
                            SoundEnum.Alert)
                        )
                        {
                            CallConfirmationRandomPopupPage(CancelBillOfLadingBoarding,
                                new CancelBillOfLadingBoardingModel(model.PackingListAccessoryId, model.BarCode,
                                    model.MacAddress, false));
                        }
                        break;
                    case ExceptionCodeEnum.PackNotFoundWarehouse:
                        if (await _notificationService.AskQuestionAsync(packingListViewInfo.Response.ExceptionMessage, 
                            SoundEnum.Alert))
                            await ReadingPackAccessory(new ReadingPackAccessoryModel(true, model.PackingListAccessoryId,
                                model.UnitLocal, model.UnitDestination, model.BarCode, model.MacAddress));
                        break;
                    default:
                        await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
                        break;
                }
            }
            else
            {
                await _notificationService.NotifyAsync("Não foi possivel concluir a requisição.", SoundEnum.Erros);
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

        private void RequestPalletsViewConfirmed(bool confirmed, TransportAccessoryViewInfoModel transportAccessoryViewInfo)
        {
            if (confirmed)
            {
                Model.TransportAccessoryViewInfo = transportAccessoryViewInfo;

                GetPackingListAccessory();
            }
            else
            {
                Model.ClearModelAfterDetination();
                Model.AccessoryFocus();
                MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
            }
        }

        private async void RequestPackingListAccessoryViewConfirmed(bool confirmed, PackingListViewInfoModel packingListViewInfo)
        {
            if (confirmed)
            {
                await SetPackingListViewInfo(packingListViewInfo);
            }
            else
            {
                Model.ClearModelAfterDetination();
                Model.AccessoryFocus();
                MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
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
            if (confirmed && callBackData != null)
            {
                callBackData.TryGetValue("Model", out object objModel);
                var model = objModel as ValidClosePackingListAccessoryModel;
                if (model != null)
                    ClosePackingListAccessory(model);
            }
        }

        #endregion

        #region Commands

        private DelegateCommand _destinationChangedCommand;
        public DelegateCommand DestinationChangedCommand =>
            _destinationChangedCommand ?? (_destinationChangedCommand = new DelegateCommand(DestinationChangedCommandHandler));

        private DelegateCommand _accessoryChangedCommand;
        public DelegateCommand AccessoryChangedCommand =>
            _accessoryChangedCommand ?? (_accessoryChangedCommand = new DelegateCommand(AccessoryChangedCommandHandler));

        private DelegateCommand _confirmationCommand;
        public DelegateCommand ConfirmationCommand =>
            _confirmationCommand ?? (_confirmationCommand = new DelegateCommand(ConfirmationCommandHandler));

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearCommandHandler));

        #region Menu Commands

        private DelegateCommand _closePackingListAccessoryCommand;
        public DelegateCommand ClosePackingListAccessoryCommand =>
            _closePackingListAccessoryCommand ?? (_closePackingListAccessoryCommand = new DelegateCommand(ClosePackingListAccessoryCommandHandler));

        #endregion

        #endregion

        #region Command Handlers

        private async void DestinationChangedCommandHandler()
        {
            if (!string.IsNullOrEmpty(Model.Destination))
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var validUnit = await _commonService.ValidUnit(Model.Destination);

                await _popupNavigation.PopAllAsync();

                if (validUnit.Response != null 
                && validUnit.Response.Code == Model.Destination
                && validUnit.Response.Id != _userService.User.Unit.Id)
                {
                    Model.UnitViewInfo = validUnit.Response;
                    Model.AccessoryFocus();
                }
                else
                {
                    Model.UnitViewInfo = null;
                    Model.ClearModel();
                    await _notificationService.NotifyAsync(
                        ((validUnit.Response != null && validUnit.Response.Id == _userService.User.Unit.Id)
                            ? "Unidade de destino deve ser diferente da unidade local."
                            : $"Unidade {Model.Destination} inválida."),
                        SoundEnum.Erros);
                }
            }
        }

        private async void AccessoryChangedCommandHandler()
        {
            Model.AccessoryDescription = String.Empty;
            if (!string.IsNullOrEmpty(Model.Accessory))
            {
                if (Model.Accessory.Equals("PALLET"))
                {
                    await _popupNavigation.PushAsync(new LoadingPopupPage());

                    var getPallets = await _boardingAccessoryService.GetPallets();

                    await _popupNavigation.PopAllAsync();

                    if (getPallets.Response != null && getPallets.Response.Any())
                    {
                        await NavigationService.NavigateAsync("RequestPalletsViewPopupPage",
                            new NavigationParameters() {{"Pallets", getPallets.Response}});
                    }
                    else
                    {
                        await _notificationService.NotifyAsync("Pallet não encontrado.", SoundEnum.Erros);
                        Model.ClearModelAfterDetination();
                        MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
                    }
                }
                else if(!string.IsNullOrEmpty(Model.Accessory) && Model.Accessory.IsInt())
                {
                    await _popupNavigation.PushAsync(new LoadingPopupPage());

                    var validTransportAccessory = await _boardingAccessoryService.ValidTransportAccessory(Model.Accessory, _userService.User.Unit.Id);

                    await _popupNavigation.PopAllAsync();

                    Model.TransportAccessoryViewInfo = validTransportAccessory.Response;

                    if (Model.TransportAccessoryViewInfo == null || !Model.TransportAccessoryViewInfo.Valid)
                    {
                        await _notificationService.NotifyAsync(
                            validTransportAccessory.Response?.ExceptionMessage ?? "Acessório não encontrado.",
                            SoundEnum.Erros);

                        Model.ClearModelAfterDetination();
                        MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
                    }
                    else
                    {
                        GetPackingListAccessory();
                    }
                }
                else
                {
                    Model.ClearModelAfterDetination();
                    MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
                }
            }
        }

        private async void ConfirmationCommandHandler()
        {
            if (Model.IsValid())
            {
                ReadingPackAccessory(new ReadingPackAccessoryModel(false, Model.PackingListViewInfo.Id, 
                    _userService.User.Unit.Id, Model.UnitViewInfo.Id, Model.Reading, 
                    _wifiService.MacAddress));

                Model.Reading = String.Empty;
                Model.ReadingFocus();
            }
            else
            {
                await _notificationService.NotifyAsync("Informe todos os campos obrigatórios.",
                    SoundEnum.Alert);
            }
        }

        private void ClearCommandHandler()
        {
            Model.ClearModel();
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
        }

        #region Menu Command Handlers

        private void ClosePackingListAccessoryCommandHandler()
        {
            ClosePackingListAccessory(new ValidClosePackingListAccessoryModel(Model.PackingListViewInfo.Id, true,
                new List<int>(), _wifiService.MacAddress));
        }

        #endregion

        #endregion
    }
}
