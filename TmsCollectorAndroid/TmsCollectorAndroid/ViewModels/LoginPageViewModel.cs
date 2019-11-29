using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.Views.PopupPages;
using Xamarin.Forms;

namespace TmsCollectorAndroid.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
          public LoginPageViewModel(INavigationService navigationService, 
                IBarcodeReaderService barcodeReaderService,
                IStatusBarService statusBarService,
                IEnvironmentConfigurationService environmentConfigurationService,
                IUserService userService,
                INotificationService notificationService,
                IServiceBase tmsApiServiceBase,
                ICommonService tmsApiCommonService,
                IPopupNavigation popupNavigation,
                IWifiService wifiService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _environmentConfigurationService = environmentConfigurationService;
            _userService = userService;
            _notificationService = notificationService;
            _tmsApiServiceBase = tmsApiServiceBase;
            _tmsApiCommonService = tmsApiCommonService;
            _popupNavigation = popupNavigation;
            _wifiService = wifiService;

            Model = new LoginModel();

            EnableWifi();
        }
          
        private readonly IEnvironmentConfigurationService _environmentConfigurationService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IServiceBase _tmsApiServiceBase;
        private readonly ICommonService _tmsApiCommonService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IWifiService _wifiService;

        private LoginModel _model;
        public LoginModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        private string _webServiceVersion;
        public string WebServiceVersion
        {
            get { return _webServiceVersion; }
            set { SetProperty(ref _webServiceVersion, value); }
        }

        public string CollectorVersion => _environmentConfigurationService.Configuration.CollectorVersion;

        private void EnableWifi()
        {
            Task.Run(async () =>
            {
                if (!_wifiService.IsEnabled)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));

                    await _popupNavigation.PushAsync(new LoadingPopupPage());

                    _wifiService.Enable();

                    await Task.Delay(TimeSpan.FromSeconds(5));

                    _tmsApiServiceBase.SetApiUrl(_environmentConfigurationService.Configuration.ApiUrls);

                    await Task.Delay(TimeSpan.FromSeconds(2));

                    await _popupNavigation.PopAllAsync();
                }

                SetLabelsDescription();
            });
        }

        private async void SetLabelsDescription()
        {
            var webServiceVersion = await _tmsApiServiceBase.GetWebServiceVersion();

            Url = _tmsApiServiceBase.ApiUrl;
            WebServiceVersion = webServiceVersion;

            if (string.IsNullOrEmpty(Url))
                await _notificationService.NotifyAsync("Sem conexão com o servidor, realize a conexão de rede e reinicie este aplicativo.");
        }

        #region Commands

        private DelegateCommand _unitCompletedCommand;
        public DelegateCommand UnitCompletedCommand =>
            _unitCompletedCommand ?? (_unitCompletedCommand = new DelegateCommand(UnitCompletedCommandHandler));

      private DelegateCommand _exitCommand;
        public DelegateCommand ExitCommand =>
            _exitCommand ?? (_exitCommand = new DelegateCommand(ExitCommandHandler));

        private DelegateCommand _confirmationCommand;
        public DelegateCommand ConfirmationCommand =>
            _confirmationCommand ?? (_confirmationCommand = new DelegateCommand(ConfirmationCommandHandler));

        #endregion

        #region Command Handlers

        private async void UnitCompletedCommandHandler()
        {
            Model.UnitDescription = String.Empty;
            Model.UserNameIsEnabled = Model.UserPasswordIsEnabled = false;

            if (!string.IsNullOrEmpty(Model.Unit) && Model.Unit.IsInt())
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var httpResult = await _tmsApiCommonService.ValidUnit(Model.Unit);

                await _popupNavigation.PopAllAsync();

                if (httpResult.IsSuccessStatusCode && httpResult.Response?.Code == Model.Unit)
                {
                    Model.CompanyId = httpResult.Response?.CompanyId ?? 0;
                    Model.UnitDescription = httpResult.Response?.Description;
                    Model.UserNameIsEnabled = Model.UserPasswordIsEnabled = true;
                    Model.UserNameFocus();
                }
                else
                {
                    var errorMessage = ((!httpResult.IsSuccessStatusCode)
                        ? "Não foi possivel completar a requisição."
                        : "Unidade inválida.");
                    await _notificationService.NotifyAsync(errorMessage, SoundEnum.Alert);
                }
            }
        }

        private async void ExitCommandHandler()
        {
            if (await _notificationService.AskQuestionAsync("Deseja sair do sistema?", SoundEnum.Alert))
            {
                await _tmsApiServiceBase.FinalizeSession();
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
        }

        private async void ConfirmationCommandHandler()
        {
            if (Model.BtnConfirmationIsEnabled)
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var result = await _userService.LogIn(Model);

                await _popupNavigation.PopAllAsync();

                if (result.IsAuthenticated)
                {
                    Model = new LoginModel();
                    await NavigationService.NavigateAsync("ProcessTypeMenuPage");
                }
                else
                {
                    var msg = ((!string.IsNullOrEmpty(result.Message))? result.Message : "Usuário ou senha inválido.");
                    await _notificationService.NotifyAsync(msg, SoundEnum.Alert);
                }
            }
            else
            {
                var msg = "Informe a unidade, usuário e senha.";
                await _notificationService.NotifyAsync(msg, SoundEnum.Alert);
            }
        }

        #endregion
    }
}
