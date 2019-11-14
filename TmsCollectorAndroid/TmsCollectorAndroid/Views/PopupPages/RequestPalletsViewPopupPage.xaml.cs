using Rg.Plugins.Popup.Pages;

namespace TmsCollectorAndroid.Views.PopupPages
{
    public partial class RequestPalletsViewPopupPage : PopupPage
    {
        public RequestPalletsViewPopupPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}
