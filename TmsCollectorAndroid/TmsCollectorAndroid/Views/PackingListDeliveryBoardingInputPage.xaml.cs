using System;
using System.Threading.Tasks;
using TmsCollectorAndroid.ViewModels;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class PackingListDeliveryBoardingInputPage : ContentPage
    {
        private readonly PackingListDeliveryBoardingInputPageViewModel _viewModel;

        public PackingListDeliveryBoardingInputPage()
        {
            InitializeComponent();

            _viewModel = (PackingListDeliveryBoardingInputPageViewModel) BindingContext;

            NavigationPage.SetHasNavigationBar(this, false);

            BarcodeEntry.SetBinding(BarcodeEntry.TextProperty, new Binding("Model.Reading", source: BindingContext, mode: BindingMode.TwoWay));
            BarcodeEntry.SetBinding(BarcodeEntry.IsReadOnlyProperty, new Binding("Model.ReadingIsReadOnly", source: BindingContext));
            BarcodeEntry.TextChanged += Reading_OnTextChanged;
            BarcodeEntry.Unfocused += Reading_OnUnfocused;
            _viewModel.Model.ReadingFocus += BarcodeEntry.Focus;

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding(nameof(_viewModel.ConfirmationCommand), source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.ClearCommandProperty, new Binding(nameof(_viewModel.ClearCommand), source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.GoBackCommandProperty, new Binding(nameof(_viewModel.GoBackCommand), source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.MenuAdtionalButtonsProperty, new Binding(nameof(_viewModel.MenuAdtionalButtons), source: BindingContext));
            
            Date.SetBinding(DateEntry.DateProperty, new Binding("Model.Date", source: BindingContext, mode: BindingMode.TwoWay));
            Date.SetBinding(DateEntry.IsReadOnlyProperty, new Binding("Model.DateIsReadOnly", source: BindingContext));
            Time.SetBinding(TimeEntry.TimeProperty, new Binding("Model.Time", source: BindingContext, mode: BindingMode.TwoWay));
            Time.SetBinding(TimeEntry.IsReadOnlyProperty, new Binding("Model.TimeIsReadOnly", source: BindingContext));


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
                Date.Focus();
            });
        }

        private void Date_OnUnfocused(object sender, FocusEventArgs e)
        {
            Time.Focus();
        }

        private void Time_OnUnfocused(object sender, FocusEventArgs e)
        {
            Line.Focus();
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
            if (!string.IsNullOrEmpty(BarcodeEntry.Text))
            {
                ProcessDefaultButton.FirstButtonFocus();
            }
        }
    }
}
