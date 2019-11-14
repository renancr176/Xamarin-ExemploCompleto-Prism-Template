using Prism.Navigation;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;

namespace TmsCollectorAndroid.ViewModels
{
    public class PackingListBoardingWeightViewPageViewModel : ViewModelBase
    {
        public PackingListBoardingWeightViewPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            Model = new PackingListBoardingWeightViewModel();
        }

        private PackingListBoardingWeightViewModel _model;
        public PackingListBoardingWeightViewModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        #region Navigation Methods

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("PackingListBoardingWeightViewModel", out PackingListBoardingWeightViewModel model))
            {
                Model = model;
            }
            else
            {
                NavigationService.GoBackAsync();
            }
        }

        #endregion
    }
}
