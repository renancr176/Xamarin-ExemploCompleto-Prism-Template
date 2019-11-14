using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class PackingListBoardingWeightViewPage : ContentPage
    {
        public PackingListBoardingWeightViewPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("GoBackCommand", source: BindingContext));
        }
    }
}
