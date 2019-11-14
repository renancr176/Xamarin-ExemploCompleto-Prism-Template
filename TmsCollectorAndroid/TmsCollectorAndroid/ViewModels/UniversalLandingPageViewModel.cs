using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.Models.TmsApiExtendedModels;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.ViewModels
{
    public class UniversalLandingPageViewModel : ViewModelBase
    {
        public UniversalLandingPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService,
            IPopupNavigation popupNavigation,
            IUserService userService,
            ISorterService sorterService,
            IWifiService wifiService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;
            _popupNavigation = popupNavigation;
            _userService = userService;
            _sorterService = sorterService;
            _wifiService = wifiService;

            Model = new UniversalLandingModel();

            DefaultMenuAdtionalBunttons();
        }

        private readonly INotificationService _notificationService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;
        private readonly ISorterService _sorterService;
        private readonly IWifiService _wifiService;

        private UniversalLandingModel _model;
        public UniversalLandingModel Model
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

        #region Private Methods

        private void DefaultMenuAdtionalBunttons()
        {
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>()
            {
                {"Volumes Lidos", ViewReadPackCommand},
                {"Volumes Faltantes", ViewLackPackCommand},
                {"Emitir Etiqueta", SendLabelCommand}
            };
        }

        private async void ReadingByUniversalLanding()
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());

            var packingListDetailViewInfo = await _sorterService.ReadingByUniversalLanding(new ReadingByUniversalLandingModel(Model.Reading, _userService.User.Unit.Id, _wifiService.MacAddress));

            await _popupNavigation.PopAllAsync();

            if (packingListDetailViewInfo.Response != null && !packingListDetailViewInfo.Response.Valid)
            {
                await _notificationService.NotifyAsync(packingListDetailViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
            }
            else if (packingListDetailViewInfo.Response == null)
            {
                await _notificationService.NotifyAsync("Falha ao realizar processo de desembarque universal.", SoundEnum.Erros);
            }

            Model.ClearModel();
            Model.ReadingFocus();
        }

        #endregion

        #region Commands

        private DelegateCommand _readingChangedCommand;
        public DelegateCommand ReadingChangedCommand =>
            _readingChangedCommand ?? (_readingChangedCommand = new DelegateCommand(ReadingChangedCommandHandler));

        private DelegateCommand _confirmationCommand;
        public DelegateCommand ConfirmationCommand =>
            _confirmationCommand ?? (_confirmationCommand = new DelegateCommand(ConfirmationCommandHandler));

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearCommandHandler));

        #region Menu Commands

        private DelegateCommand _viewReadPackCommand;
        public DelegateCommand ViewReadPackCommand =>
            _viewReadPackCommand ?? (_viewReadPackCommand = new DelegateCommand(ViewReadPackCommandHandler));

        private DelegateCommand _viewLackPackCommand;
        public DelegateCommand ViewLackPackCommand =>
            _viewLackPackCommand ?? (_viewLackPackCommand = new DelegateCommand(ViewLackPackCommandHandler));

        private DelegateCommand _sendLabelCommand;
        public DelegateCommand SendLabelCommand =>
            _sendLabelCommand ?? (_sendLabelCommand = new DelegateCommand(SendLabelCommandHandler));

        #endregion

        #endregion

        #region Command Handlers

        private void ReadingChangedCommandHandler()
        {
            if (!string.IsNullOrEmpty(Model.Reading))
            {
                ReadingByUniversalLanding();
            }
        }

        private async void ConfirmationCommandHandler()
        {
            if (!Model.CtrcIsReadOnly)
            {
                if (Model.CtrcIsValid())
                {
                    await _popupNavigation.PushAsync(new LoadingPopupPage());

                    var getViewLackByUniversalLanding = await _sorterService.GetViewLackByUniversalLanding(
                        Model.Number.ToInt(), Model.Digit,
                        Model.UnitEmission, _userService.User.Unit.Id);

                    await _popupNavigation.PopAllAsync();

                    if (getViewLackByUniversalLanding.Response != null && getViewLackByUniversalLanding.Response.Valid)
                    {
                        if (getViewLackByUniversalLanding.Response.Packs.Any())
                        {
                            var lstPackingListDetailViewInfo = new List<PackingListDetailViewInfoModel>();
                            lstPackingListDetailViewInfo.Add(getViewLackByUniversalLanding.Response.ToDerivedClass());
                            Model.PackingListDetailViewInfo = lstPackingListDetailViewInfo;
                            Model.TreeIsEnabled = true;
                        }
                        else
                        {
                            Model.ClearModel();
                            Model.ReadingIsReadOnly = true;
                            Model.CtrcIsReadOnly = false;
                            Model.NumberFocus();
                        }
                    }
                    else if (getViewLackByUniversalLanding.Response != null &&
                             !getViewLackByUniversalLanding.Response.Valid)
                    {
                        await _notificationService.NotifyAsync(getViewLackByUniversalLanding.Response.ExceptionMessage, SoundEnum.Erros);
                        Model.ClearModel();
                        Model.ReadingIsReadOnly = true;
                        Model.CtrcIsReadOnly = false;
                        Model.NumberFocus();
                    }
                    else
                    {
                        await _notificationService.NotifyAsync("A requisição não pode ser completada.", SoundEnum.Erros);
                    }
                }
                else
                {
                    await _notificationService.NotifyAsync("Necessário informar o CTRC.", SoundEnum.Erros);
                }
            }
        }

        private void ClearCommandHandler()
        {
            Model.ClearModel();
        }

        #region Menu Command Handlers

        private async void ViewReadPackCommandHandler()
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());

            var lstPackingListDetailViewInfo = await _sorterService.GetViewLandedByUniversalLanding(_userService.User.Unit.Id);

            await _popupNavigation.PopAllAsync();

            if (lstPackingListDetailViewInfo.Response != null && lstPackingListDetailViewInfo.Response.Any())
            {
                Model.PackingListDetailViewInfo = lstPackingListDetailViewInfo.Response.Select(p => p.ToDerivedClass());
                Model.ReadingIsReadOnly = true;
                Model.CtrcIsReadOnly = true;
                Model.TreeIsEnabled = true;
            }
            else
            {
                Model.ClearModel();

                await _notificationService.NotifyAsync("Nenhum volume foi desembarcado.", SoundEnum.Erros);
            }
        }

        private void ViewLackPackCommandHandler()
        {
            Model.ClearModel();
            Model.ReadingIsReadOnly = true;
            Model.CtrcIsReadOnly = false;
            Model.NumberFocus();
        }

        private async void SendLabelCommandHandler()
        {
            await NavigationService.NavigateAsync("SendLabelInputPage");
        }

        #endregion

        #endregion
    }
}
