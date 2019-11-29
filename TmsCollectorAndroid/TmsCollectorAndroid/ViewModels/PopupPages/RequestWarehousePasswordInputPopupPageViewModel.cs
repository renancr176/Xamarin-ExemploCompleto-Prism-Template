using System.Collections.Generic;
using Prism.Commands;
using Prism.Navigation;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;

namespace TmsCollectorAndroid.ViewModels.PopupPages
{
    public class RequestWarehousePasswordInputPopupPageViewModel : ViewModelBase
    {
        public RequestWarehousePasswordInputPopupPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService,
            ICommonService commonService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;
            _commonService = commonService;

            CallBackData = new Dictionary<string, object>();
        }

        private readonly INotificationService _notificationService;
        private readonly ICommonService _commonService;

        public Dictionary<string, object> CallBackData { get; private set; }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("CallBackData", out Dictionary<string, object> callBackData))
                CallBackData = callBackData;
        }

        private string _warehousePassword;
        public string WarehousePassword
        {
            get { return _warehousePassword; }
            set { SetProperty(ref _warehousePassword, value); }
        }

        #region Commands

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommandHandler));

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand =>
            _confirmCommand ?? (_confirmCommand = new DelegateCommand(ConfirmCommandHandler));

        #endregion

        #region Command Handlers

        private async void CancelCommandHandler()
        {
            await NavigationService.GoBackAsync(
                new NavigationParameters()
                {
                    { "WarehousePasswordInputConfirmed", false },
                    { "CallBackData", CallBackData }
                });
        }

        private async void ConfirmCommandHandler()
        {
            if (!string.IsNullOrEmpty(WarehousePassword))
            {
                var getWarehousePassword = await _commonService.GetWarehousePasswordId(WarehousePassword);

                if (getWarehousePassword.Response != null && getWarehousePassword.Response.Valid)
                {
                    await NavigationService.GoBackAsync(
                        new NavigationParameters()
                        {
                            { "WarehousePasswordInputConfirmed", true },
                            { "WarehousePassword", WarehousePassword },
                            { "WarehousePasswordId", getWarehousePassword.Response.WarehousePasswordId },
                            { "CallBackData", CallBackData }
                        });
                }
                else
                {
                    var msg = ((getWarehousePassword.Response != null)
                        ? getWarehousePassword.Response.ExceptionMessage
                        : "Não foi possível validar a senha.");

                    await _notificationService.NotifyAsync(msg, SoundEnum.Erros);
                }
            }
            else
            {
                await _notificationService.NotifyAsync("Informe a Senha do Armazém.", SoundEnum.Erros);
            }
        }

        #endregion

    }
}
