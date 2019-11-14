using Rg.Plugins.Popup.Pages;

namespace TmsCollectorAndroid.Views.PopupPages
{
    public partial class RequestWarehousePasswordInputPopupPage : PopupPage
    {
        public RequestWarehousePasswordInputPopupPage()
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
