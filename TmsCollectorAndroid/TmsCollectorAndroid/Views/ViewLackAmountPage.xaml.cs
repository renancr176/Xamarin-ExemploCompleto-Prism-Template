using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class ViewLackAmountPage : ContentPage
    {
        public ViewLackAmountPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("ViewPacksCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.SecondButtonCommandProperty, new Binding("GoBackCommand", source: BindingContext));
        }
    }
}
