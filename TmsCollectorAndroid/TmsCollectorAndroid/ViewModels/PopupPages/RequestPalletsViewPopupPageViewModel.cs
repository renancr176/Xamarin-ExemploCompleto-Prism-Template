using System.Collections.Generic;
using Prism.Commands;
using Prism.Navigation;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.ViewModels.PopupPages
{
    public class RequestPalletsViewPopupPageViewModel : ViewModelBase
    {
        public RequestPalletsViewPopupPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            IUserService userService,
            IBoardingAccessoryService boardingAccessoryService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _userService = userService;
            _boardingAccessoryService = boardingAccessoryService;
        }

        private readonly IUserService _userService;
        private readonly IBoardingAccessoryService _boardingAccessoryService;

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("Pallets", out IEnumerable<TransportAccessoryViewInfoModel> pallets))
            {
                Pallets = pallets;
            }
        }

        private IEnumerable<TransportAccessoryViewInfoModel> _pallets;
        public IEnumerable<TransportAccessoryViewInfoModel> Pallets
        {
            get { return _pallets; }
            set { SetProperty(ref _pallets, value); }
        }

        #region Commands

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommandHandler));

        private DelegateCommand<TransportAccessoryViewInfoModel> _palletSelectedCommand;
        public DelegateCommand<TransportAccessoryViewInfoModel> PalletSelectedCommand =>
            _palletSelectedCommand ?? (_palletSelectedCommand = new DelegateCommand<TransportAccessoryViewInfoModel>(PalletSelectedCommandHandler));

        #endregion

        #region Command Handlers

        private async void CancelCommandHandler()
        {
            await NavigationService.GoBackAsync(
                new NavigationParameters() { { "RequestPalletsViewConfirmed", false } });
        }

        private async void PalletSelectedCommandHandler(TransportAccessoryViewInfoModel pallet)
        {
            var transportAccessoryViewInfo = await _boardingAccessoryService.ValidTransportAccessory(pallet.CobolNumber, _userService.User.Unit.Id);

            if (transportAccessoryViewInfo.Response != null)
            {
                await NavigationService.GoBackAsync(
                    new NavigationParameters()
                    {
                        {"RequestPalletsViewConfirmed", true},
                        {"TransportAccessoryViewInfo", transportAccessoryViewInfo.Response}
                    });
            }
        }

        #endregion
    }
}
