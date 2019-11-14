using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class ViewLackByProcessPage : ContentPage
    {
        public ViewLackByProcessPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("ConsultCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.ClearCommandProperty, new Binding("ClearCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.GoBackCommandProperty, new Binding("GoBackCommand", source: BindingContext));
        }
    }
}
