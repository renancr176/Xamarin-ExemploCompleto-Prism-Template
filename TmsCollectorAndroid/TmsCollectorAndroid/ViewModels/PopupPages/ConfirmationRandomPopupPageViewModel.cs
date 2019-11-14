using System;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Navigation;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.ViewModels.PopupPages
{
    public class ConfirmationRandomPopupPageViewModel : ViewModelBase
    {
        public ConfirmationRandomPopupPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            CallBackData = new Dictionary<string, object>();
            Text = "INFORME A SEQUÊNCIA ABAIXO PARA CONFIRMAR.";
            ConfirmationRandomValue = new Random().Next(1000, 9999).ToString();
        }

        public Dictionary<string, object> CallBackData { get; private set; }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("CallBackData", out Dictionary<string, object> callBackData))
                CallBackData = callBackData;

            if (parameters.TryGetValue("Text", out string text))
                Text = text;
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private string _confirmationRandomValue;
        public string ConfirmationRandomValue
        {
            get { return _confirmationRandomValue; }
            private set { SetProperty(ref _confirmationRandomValue, value); }
        }

        private string _confirmation;
        public string Confirmation
        {
            get { return _confirmation; }
            set { SetProperty(ref _confirmation, value); }
        }

        #region Commands

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommandHandler));

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand =>
            _confirmCommand ?? (_confirmCommand = new DelegateCommand(ConfirmCommandHandler));

        #endregion

        #region Comman Handlers

        private async void CancelCommandHandler()
        {
            await NavigationService.GoBackAsync(
                new NavigationParameters()
                {
                    { "ConfirmationRandomConfirmed", false },
                    { "CallBackData", CallBackData }
                });
        }

        private async void ConfirmCommandHandler()
        {
            if (Confirmation.Equals(ConfirmationRandomValue))
            {
                await NavigationService.GoBackAsync(
                    new NavigationParameters()
                    {
                        { "ConfirmationRandomConfirmed", true },
                        { "CallBackData", CallBackData }
                    });
            }
        }

        #endregion
    }
}
