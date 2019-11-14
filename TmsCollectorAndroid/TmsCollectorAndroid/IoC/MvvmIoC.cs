using Prism.Ioc;
using TmsCollectorAndroid.ViewModels;
using TmsCollectorAndroid.ViewModels.PopupPages;
using TmsCollectorAndroid.Views;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.IoC
{
    public class MvvmIoC
    {
        public static void Register(IContainerRegistry containerRegistry)
        {
            #region Pages
            
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ProcessTypeMenuPage, ProcessTypeMenuPageViewModel>();
            containerRegistry.RegisterForNavigation<PackingListBoardingInputPage, PackingListBoardingInputPageViewModel>();
            containerRegistry.RegisterForNavigation<PackingListLandingInputPage, PackingListLandingInputPageViewModel>();
            containerRegistry.RegisterForNavigation<PackingListAccessoriesInputPage, PackingListAccessoriesInputPageViewModel>();
            containerRegistry.RegisterForNavigation<BillOfLadingInformationsPage, BillOfLadingInformationsPageViewModel>();
            containerRegistry.RegisterForNavigation<PackingListDeliveryBoardingInputPage, PackingListDeliveryBoardingInputPageViewModel>();
            containerRegistry.RegisterForNavigation<PackingListDeliveryReturnInputPage, PackingListDeliveryReturnInputPageViewModel>();
            containerRegistry.RegisterForNavigation<UniversalLandingPage, UniversalLandingPageViewModel>();
            containerRegistry.RegisterForNavigation<PackingListBoardingWeightViewPage, PackingListBoardingWeightViewPageViewModel>();
            containerRegistry.RegisterForNavigation<PacksDetailViewPage, PacksDetailViewPageViewModel>();
            containerRegistry.RegisterForNavigation<SendLabelInputPage, SendLabelInputPageViewModel>();
            containerRegistry.RegisterForNavigation<ViewLackPage, ViewLackPageViewModel>();
            containerRegistry.RegisterForNavigation<ViewLackAmountPage, ViewLackAmountPageViewModel>();
            containerRegistry.RegisterForNavigation<ViewLackByProcessPage, ViewLackByProcessPageViewModel>();
            containerRegistry.RegisterForNavigation<ViewLackDetailPage, ViewLackDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<MaintenanceSealsInputPage, MaintenanceSealsInputPageViewModel>();

            #endregion

            #region Popup Pages

            containerRegistry.RegisterForNavigation<LoadingPopupPage, LoadingPopupPageViewModel>();
            containerRegistry.RegisterForNavigation<RequestTrafficScheduleDateInputPopupPage, RequestTrafficScheduleDateInputPopupPageViewModel>();
            containerRegistry.RegisterForNavigation<RequestWarehousePasswordInputPopupPage, RequestWarehousePasswordInputPopupPageViewModel>();
            containerRegistry.RegisterForNavigation<RequestUnitInputPopupPage, RequestUnitInputPopupPageViewModel>();
            containerRegistry.RegisterForNavigation<ConfirmationRandomPopupPage, ConfirmationRandomPopupPageViewModel>();
            containerRegistry.RegisterForNavigation<RequestPalletsViewPopupPage, RequestPalletsViewPopupPageViewModel>();
            containerRegistry.RegisterForNavigation<RequestPackingListAccessoryViewPopupPage, RequestPackingListAccessoryViewPopupPageViewModel>();

            #endregion
        }
    }
}