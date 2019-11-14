using System.Collections.Generic;
using Prism.Commands;
using Prism.Navigation;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.ViewModels
{
    public class ViewLackAmountPageViewModel : ViewModelBase
    {
        public ViewLackAmountPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            Model = new ViewLackAmountModel();
        }

        private ViewLackAmountModel _model;
        public ViewLackAmountModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        #region Navigation Methods

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("ViewLackAmountModel", out ViewLackAmountModel model))
            {
                Model = model;
            }
            else
            {
                NavigationService.GoBackAsync();
            }
        }

        #endregion

        #region Commands

        private DelegateCommand _viewPacksCommand;
        public DelegateCommand ViewPacksCommand =>
            _viewPacksCommand ?? (_viewPacksCommand = new DelegateCommand(ViewPacksCommandHandler));

        #endregion

        #region Command Handlers

        private async void ViewPacksCommandHandler()
        {
            await NavigationService.NavigateAsync("ViewLackDetailPage", new NavigationParameters()
            {
                { "Title", "VOLUMES FALTANTES" },
                {"PackingListDetailViewInfo", new List<PackingListDetailViewInfoModel>() {Model.PackingListDetailViewInfo}}
            });
        }

        #endregion
    }
}
