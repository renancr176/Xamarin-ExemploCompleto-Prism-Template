using Rg.Plugins.Popup.Pages;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views.PopupPages
{
    public partial class RequestTrafficScheduleDateInputPopupPage : PopupPage
    {
        public RequestTrafficScheduleDateInputPopupPage()
        {
            InitializeComponent();

            Date.SetBinding(DateEntry.DateProperty, new Binding("Date", source: BindingContext, mode: BindingMode.TwoWay));
            Time.SetBinding(TimeEntry.TimeProperty, new Binding("Time", source: BindingContext, mode: BindingMode.TwoWay));
        }

        protected override void OnAppearingAnimationEnd()
        {
            Date.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }

        private void DateOnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(Date.Date))
                Time.Focus();
        }

        private void TimeOnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(Time.Time))
                BtnConfirmation.Focus();
        }
    }
}
