using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class ViewLackDetailPage : ContentPage
    {
        public ViewLackDetailPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            Header.SetBinding(Header.TitleProperty, new Binding("Title", source: BindingContext));

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("GoBackCommand", source: BindingContext));
        }
    }
}
