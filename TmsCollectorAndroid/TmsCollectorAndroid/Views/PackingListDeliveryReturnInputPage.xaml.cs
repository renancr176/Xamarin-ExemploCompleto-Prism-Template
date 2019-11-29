using System;
using System.Threading.Tasks;
using TmsCollectorAndroid.ViewModels;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class PackingListDeliveryReturnInputPage : ContentPage
    {
        private readonly PackingListDeliveryReturnInputPageViewModel _viewModel;
        public PackingListDeliveryReturnInputPage()
        {
            InitializeComponent();

            _viewModel = (PackingListDeliveryReturnInputPageViewModel) BindingContext;

            NavigationPage.SetHasNavigationBar(this, false);

            BarcodeEntry.SetBinding(BarcodeEntry.TextProperty, new Binding("Model.Reading", source: BindingContext, mode: BindingMode.TwoWay));
            BarcodeEntry.SetBinding(BarcodeEntry.IsReadOnlyProperty, new Binding("Model.ReadingIsReadOnly", source: BindingContext));
            BarcodeEntry.TextChanged += Reading_OnTextChanged;
            BarcodeEntry.Unfocused += Reading_OnUnfocused;
            _viewModel.Model.ReadingFocus += BarcodeEntry.Focus;

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("ConfirmationCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.ClearCommandProperty, new Binding("ClearCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.GoBackCommandProperty, new Binding("GoBackCommand", source: BindingContext));

            Date.SetBinding(DateEntry.DateProperty, new Binding("Model.Date", source: BindingContext, mode: BindingMode.TwoWay));
            Date.SetBinding(DateEntry.IsReadOnlyProperty, new Binding("Model.DateIsReadOnly", source: BindingContext));
            Time.SetBinding(TimeEntry.TimeProperty, new Binding("Model.Time", source: BindingContext, mode: BindingMode.TwoWay));
            Time.SetBinding(TimeEntry.IsReadOnlyProperty, new Binding("Model.TimeIsReadOnly", source: BindingContext));

            _viewModel.Model.PackingListDigitFocus += ExecutePackingListDigitFocus;
            _viewModel.Model.DateFocus += ExecuteDateFocus;
            _viewModel.Model.DriverFocus += ExecuteDriverFocus;
            _viewModel.Model.VehicleFocus += ExecuteVehicleFocus;
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
                PackingListNumber.Focus();
            });
        }

        private void ExecutePackingListDigitFocus()
        {
            PackingListDigit.Focus();
        }

        private void ExecuteDateFocus()
        {
            Date.Focus();
        }

        private void Date_OnUnfocused(object sender, FocusEventArgs e)
        {
            Time.Focus();
        }

        private void Time_OnUnfocused(object sender, FocusEventArgs e)
        {
            _viewModel.TimeChangedCommand.Execute();
        }

        private void ExecuteDriverFocus()
        {
            Driver.Focus();
        }

        private void ExecuteVehicleFocus()
        {
            Vehicle.Focus();
        }

        private void Reading_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BarcodeEntry.Text))
            {
                ProcessDefaultButton.FirstButtonFocus();
            }
        }
    }
}
