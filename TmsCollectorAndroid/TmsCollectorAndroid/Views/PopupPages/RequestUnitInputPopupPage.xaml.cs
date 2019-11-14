using Rg.Plugins.Popup.Pages;
using TmsCollectorAndroid.ViewModels.PopupPages;

namespace TmsCollectorAndroid.Views.PopupPages
{
    public partial class RequestUnitInputPopupPage : PopupPage
    {
        private readonly RequestUnitInputPopupPageViewModel _viewModel;
        public RequestUnitInputPopupPage()
        {
            InitializeComponent();

            _viewModel = (RequestUnitInputPopupPageViewModel) BindingContext;

            _viewModel.BtnConfirmationFocus += BtnConfirmationFocus;
        }

        protected override void OnAppearingAnimationEnd()
        {
            Unit.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }

        private void BtnConfirmationFocus()
        {
            BtnConfirmation.Focus();
        }
    }
}
