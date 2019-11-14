using System;
using System.Threading.Tasks;
using TmsCollectorAndroid.ViewModels;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class PackingListAccessoriesInputPage : ContentPage
    {
        private readonly PackingListAccessoriesInputPageViewModel _viewModel;

        public PackingListAccessoriesInputPage()
        {
            InitializeComponent();

            _viewModel = (PackingListAccessoriesInputPageViewModel) BindingContext;

            NavigationPage.SetHasNavigationBar(this, false);

            BarcodeEntry.SetBinding(BarcodeEntry.TextProperty, new Binding("Model.Reading", source: BindingContext, mode: BindingMode.TwoWay));
            BarcodeEntry.SetBinding(BarcodeEntry.IsReadOnlyProperty, new Binding("Model.ReadingIsReadOnly", source: BindingContext));
            BarcodeEntry.Unfocused += Reading_OnUnfocused;
            _viewModel.Model.ReadingFocus += BarcodeEntry.Focus;

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("ConfirmationCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.ClearCommandProperty, new Binding("ClearCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.GoBackCommandProperty, new Binding("GoBackCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.MenuAdtionalButtonsProperty, new Binding("MenuAdtionalButtons", source: BindingContext));

            _viewModel.Model.DestinationFocus += ExecuteDestinationFocus;
            _viewModel.Model.AccessoryFocus += ExeceuteAccessoryFocus;
        }

        protected override void OnAppearing()
        {
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                Destination.Focus();
            });
        }

        private void ExecuteDestinationFocus()
        {
            Destination.Focus();
        }

        private void ExeceuteAccessoryFocus()
        {
            Accessory.Focus();
        }

        private void Reading_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(BarcodeEntry.Text))
                ProcessDefaultButton.FirstButtonFocus();
        }
    }
}
