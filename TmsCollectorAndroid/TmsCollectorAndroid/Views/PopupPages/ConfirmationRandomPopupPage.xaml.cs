using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views.PopupPages
{
    public partial class ConfirmationRandomPopupPage : PopupPage
    {
        public ConfirmationRandomPopupPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearingAnimationEnd()
        {
            Confirmation.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }

        private void Confirmation_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(Confirmation.Text))
            {
                BtnConfirmation.Focus();
            }
        }
    }
}
