using Prism.Commands;
using Prism.Mvvm;
using TmsCollectorAndroid.Interfaces.Services;
using Xamarin.Forms;

namespace TmsCollectorAndroid.ViewModels.Controls
{
    public class BarcodeEntryViewModel : BindableBase
    {
        public BarcodeEntryViewModel(IBarcodeReaderService barcodeReaderService)
        {
            _barcodeReaderService = barcodeReaderService;

            DisableBarcodeReader();
        }

        private readonly IBarcodeReaderService _barcodeReaderService;

        #region Private Methods

        private void EnableBarcodeReader()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!_barcodeReaderService.IsReaderEnabled)
                    await _barcodeReaderService.EnableAsync(true);

                if (_barcodeReaderService.IsReaderOpened)
                    await _barcodeReaderService.CloseAsync();
            });
        }

        private void DisableBarcodeReader()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!_barcodeReaderService.IsReaderOpened)
                    await _barcodeReaderService.OpenAsync();

                if (_barcodeReaderService.IsReaderEnabled)
                    await _barcodeReaderService.EnableAsync(false);
            });
        }

        #endregion

        #region Commands

        private DelegateCommand _enableBarcodeReaderCommand;
        public DelegateCommand EnableBarcodeReaderCommand =>
            _enableBarcodeReaderCommand ?? (_enableBarcodeReaderCommand = new DelegateCommand(EnableBarcodeReaderCommandHandler));

        private DelegateCommand _disableBarcodeReaderCommand;
        public DelegateCommand DisableBarcodeReaderCommand =>
            _disableBarcodeReaderCommand ?? (_disableBarcodeReaderCommand = new DelegateCommand(DisableBarcodeReaderCommandHandler));

        #endregion

        #region Command Handlers

        private void EnableBarcodeReaderCommandHandler()
        {
            EnableBarcodeReader();
        }

        private void DisableBarcodeReaderCommandHandler()
        {
            DisableBarcodeReader();
        }

        #endregion
    }
}
