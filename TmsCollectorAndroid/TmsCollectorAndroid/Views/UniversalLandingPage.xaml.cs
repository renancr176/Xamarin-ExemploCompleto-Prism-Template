using System;
using System.Threading.Tasks;
using TmsCollectorAndroid.ViewModels;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class UniversalLandingPage : ContentPage
    {
        private readonly UniversalLandingPageViewModel _viewModel;

        public UniversalLandingPage()
        {
            InitializeComponent();

            _viewModel = (UniversalLandingPageViewModel) BindingContext;

            NavigationPage.SetHasNavigationBar(this, false);

            BarcodeEntry.SetBinding(BarcodeEntry.TextProperty, new Binding("Model.Reading", source: BindingContext, mode: BindingMode.TwoWay));
            BarcodeEntry.SetBinding(BarcodeEntry.IsReadOnlyProperty, new Binding("Model.ReadingIsReadOnly", source: BindingContext));
            _viewModel.Model.ReadingFocus += BarcodeEntry.Focus;

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("ConfirmationCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.ClearCommandProperty, new Binding("ClearCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.GoBackCommandProperty, new Binding("GoBackCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.MenuAdtionalButtonsProperty, new Binding("MenuAdtionalButtons", source: BindingContext));

            _viewModel.Model.NumberFocus += ExecuteNumberFocus;
            _viewModel.Model.BtnConfirmFocus += ExecuteBtnConfirmFocus;
        }

        protected override void OnAppearing()
        {
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                BarcodeEntry.Focus();
            });
        }

        private void ExecuteNumberFocus()
        {
            Number.Focus();
        }

        private void Number_OnUnfocused(object sender, FocusEventArgs e)
        {
            Digit.Text = UnitEmission.Text = String.Empty;
            Digit.Focus();
        }

        private void Digit_OnUnfocused(object sender, FocusEventArgs e)
        {
            UnitEmission.Focus();
        }

        private void ExecuteBtnConfirmFocus()
        {
            ProcessDefaultButton.FirstButtonFocus();
        }

        private void UnitEmission_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(Number.Text)
            && !string.IsNullOrEmpty(Digit.Text)
            && !string.IsNullOrEmpty(UnitEmission.Text))
            {
                ProcessDefaultButton.FirstButtonFocus();
            }
        }

        private void BarcodeEntry_OnUnfocused(object sender, FocusEventArgs e)
        {
            _viewModel.ReadingChangedCommand.Execute();
        }
    }
}
