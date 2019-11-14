using System;
using System.Threading.Tasks;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class ViewLackPage : ContentPage
    {
        public ViewLackPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("ConsultCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.ClearCommandProperty, new Binding("ClearCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.GoBackCommandProperty, new Binding("GoBackCommand", source: BindingContext));
        }

        protected override void OnAppearing()
        {
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                Number.Focus();
            });
        }

        private void Number_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(Number.Text))
            {
                Digit.Focus();
            }
        }

        private void Digit_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(Digit.Text))
            {
                UnitEmission.Focus();
            }
        }

        private void UnitEmission_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(UnitEmission.Text))
            {
                ProcessDefaultButton.FirstButtonFocus();
            }
        }
    }
}
