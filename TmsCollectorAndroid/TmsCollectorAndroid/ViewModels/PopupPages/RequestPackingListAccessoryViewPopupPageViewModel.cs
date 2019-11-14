using System.Collections.Generic;
using Prism.Commands;
using Prism.Navigation;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.ViewModels.PopupPages
{
    public class RequestPackingListAccessoryViewPopupPageViewModel : ViewModelBase
    {
        public RequestPackingListAccessoryViewPopupPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("PackingList", out IEnumerable<PackingListViewInfoModel> packingList))
            {
                PackingList = packingList;
            }
        }

        private IEnumerable<PackingListViewInfoModel> _packingList;
        public IEnumerable<PackingListViewInfoModel> PackingList
        {
            get { return _packingList; }
            set { SetProperty(ref _packingList, value); }
        }

        #region Commands

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommandHandler));

        private DelegateCommand<PackingListViewInfoModel> _packingSelectedCommand;
        public DelegateCommand<PackingListViewInfoModel> PackingSelectedCommand =>
            _packingSelectedCommand ?? (_packingSelectedCommand = new DelegateCommand<PackingListViewInfoModel>(PackingSelectedCommandHandler));

        #endregion

        #region Command Handlers

        private async void CancelCommandHandler()
        {
            await NavigationService.GoBackAsync(new NavigationParameters()
            {
                { "RequestPackingListAccessoryViewConfirmed", false }
            });
        }

        private async void PackingSelectedCommandHandler(PackingListViewInfoModel packingListViewInfo)
        {
            await NavigationService.GoBackAsync(new NavigationParameters()
            {
                { "RequestPackingListAccessoryViewConfirmed", true },
                { "PackingListViewInfo", packingListViewInfo }
            });
        }

        #endregion
    }
}
