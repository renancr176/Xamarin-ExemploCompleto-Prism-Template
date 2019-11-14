using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Responses;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.ViewModels
{
    public class ViewLackByProcessPageViewModel : ViewModelBase
    {
        public ViewLackByProcessPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            IPopupNavigation popupNavigation,
            IUserService userService,
            IBoardingService boardingService,
            ILandingService landingService
            ) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _popupNavigation = popupNavigation;
            _userService = userService;
            _boardingService = boardingService;
            _landingService = landingService;

            Model = new ViewLackByProcessModel();
        }

        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;
        private readonly IBoardingService _boardingService;
        private readonly ILandingService _landingService;

        private ViewLackByProcessModel _model;
        public ViewLackByProcessModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        #region Navigation Methods

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("ViewLackByProcessModel", out ViewLackByProcessModel model)
            && model.IsValidDefaultValues())
            {
                Model = model;
            }
            else
            {
                NavigationService.GoBackAsync();
            }
        }

        #endregion

        #region Private Methods

        private async Task<IEnumerable<PackingListDetailViewInfoModel>> GetViewLack()
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());

            switch (Model.LackType)
            {
                case LackTypeEnum.Boarding:
                    var getViewLackBoardingByTrafficScheduleDetail =
                        await _boardingService.GetViewLackBoardingByTrafficScheduleDetail(Model.PackingListId, _userService.User.Unit.Id, Model.VehicleViewInfo.TrafficScheduleDetailId);
                    await _popupNavigation.PopAllAsync();
                    return getViewLackBoardingByTrafficScheduleDetail.Response; 
                case LackTypeEnum.Landing:
                    var getViewLackLandingByTrafficScheduleDetail =
                        await _landingService.GetViewLackLandingByTrafficScheduleDetail(Model.VehicleViewInfo.TrafficScheduleDetailId);
                    await _popupNavigation.PopAllAsync();
                    return getViewLackLandingByTrafficScheduleDetail.Response;
                default:
                    await _popupNavigation.PopAllAsync();
                    return new List<PackingListDetailViewInfoModel>();
            }
        }

        #endregion

        #region Commands

        private DelegateCommand _consultCommand;
        public DelegateCommand ConsultCommand =>
            _consultCommand ?? (_consultCommand = new DelegateCommand(ConsultCommandHandler));

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearCommandHandler));

        #endregion

        #region Command Handlers

        private async void ConsultCommandHandler()
        {
            var lstPackingListDetailViewInfo = await GetViewLack();

            await NavigationService.NavigateAsync("ViewLackDetailPage", new NavigationParameters()
            {
                { "Title", "CONSULTAR FALTAS" },
                {"PackingListDetailViewInfo", lstPackingListDetailViewInfo}
            });
        }

        private void ClearCommandHandler()
        {
        }

        #endregion
    }
}
