using System;
using System.Threading.Tasks;
using TmsCollectorAndroid.ViewModels;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class SendLabelInputPage : ContentPage
    {
        private readonly SendLabelInputPageViewModel _viewModel;

        public SendLabelInputPage()
        {
            InitializeComponent();

            _viewModel = (SendLabelInputPageViewModel) BindingContext;

            NavigationPage.SetHasNavigationBar(this, false);

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("ConfirmationCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.ClearCommandProperty, new Binding("ClearCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.GoBackCommandProperty, new Binding("GoBackCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.MenuAdtionalButtonsProperty, new Binding("MenuAdtionalButtons", source: BindingContext));

            _viewModel.Model.PackFocus += ExecutePackFocus;
            _viewModel.Model.BtnConfirmFocus += ExecuteBtnConfirmFocus;
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

        private void ExecutePackFocus()
        {
            Pack.Focus();
        }

        private void ExecuteBtnConfirmFocus()
        {
            ProcessDefaultButton.FirstButtonFocus();
        }
    }
}
