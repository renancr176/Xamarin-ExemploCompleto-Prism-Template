using System;
using System.Threading.Tasks;
using TmsCollectorAndroid.ViewModels;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class BillOfLadingInformationsPage : ContentPage
    {
        private readonly BillOfLadingInformationsPageViewModel _viewModel;

        public BillOfLadingInformationsPage()
        {
            InitializeComponent();

            _viewModel = (BillOfLadingInformationsPageViewModel) BindingContext;

            NavigationPage.SetHasNavigationBar(this, false);

            BarcodeEntry.SetBinding(BarcodeEntry.TextProperty, new Binding("Model.Reading", source: BindingContext, mode: BindingMode.TwoWay));
            BarcodeEntry.SetBinding(BarcodeEntry.IsReadOnlyProperty, new Binding("Model.ReadingIsReadOnly", source: BindingContext));
            BarcodeEntry.TextChanged += Reading_OnTextChanged;
            BarcodeEntry.Unfocused += Reading_OnUnfocused;
            _viewModel.Model.ReadingFocus += BarcodeEntry.Focus;

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("ConfirmationCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.ClearCommandProperty, new Binding("ClearCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.GoBackCommandProperty, new Binding("GoBackCommand", source: BindingContext));
        }

        private void Reading_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.ConfirmationCommand?.Execute();
        }

        protected override void OnAppearing()
        {
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                BarcodeEntry.Focus();
            });
        }

        private void Reading_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(BarcodeEntry.Text))
            {
                ProcessDefaultButton.FirstButtonFocus();
            }
        }
    }
}
