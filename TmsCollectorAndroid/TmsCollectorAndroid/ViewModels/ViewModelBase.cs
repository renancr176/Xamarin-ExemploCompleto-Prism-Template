using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using TmsCollectorAndroid.Interfaces.Services;
using Xamarin.Forms;

namespace TmsCollectorAndroid.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        protected IBarcodeReaderService BarcodeReaderService { get; private set; }
        protected IStatusBarService StatusBarService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService)
        {
            NavigationService = navigationService;
            BarcodeReaderService = barcodeReaderService;
            StatusBarService = statusBarService;
            
            StatusBarService.HideStatusBar();

            DisableBarcodeReader();
        }

        private void DisableBarcodeReader()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!BarcodeReaderService.IsReaderOpened)
                    await BarcodeReaderService.OpenAsync();

                if (BarcodeReaderService.IsReaderEnabled)
                    await BarcodeReaderService.EnableAsync(false);
            });
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {
            StatusBarService.ResetToDefaultStatusBar();
        }

        private DelegateCommand _goBackCommand;
        public DelegateCommand GoBackCommand =>
            _goBackCommand ?? (_goBackCommand = new DelegateCommand(GoBackCommandHandler));

        protected virtual async void GoBackCommandHandler()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
