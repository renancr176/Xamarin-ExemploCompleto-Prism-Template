using Rg.Plugins.Popup.Pages;

namespace TmsCollectorAndroid.Views.PopupPages
{
    public partial class RequestPackingListAccessoryViewPopupPage : PopupPage
    {
        public RequestPackingListAccessoryViewPopupPage()
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
