using System;
using System.Threading.Tasks;
using TmsCollectorAndroid.ViewModels;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class PackingListBoardingInputPage : ContentPage
    {
        private PackingListBoardingInputPageViewModel _viewModel;

        public PackingListBoardingInputPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            _viewModel = (PackingListBoardingInputPageViewModel) BindingContext;

            Header.SetBinding(Header.TitleProperty, new Binding("Title", source: BindingContext));

            BarcodeEntry.SetBinding(BarcodeEntry.TextProperty, new Binding("Model.Reading", source: BindingContext, mode: BindingMode.TwoWay));
            BarcodeEntry.SetBinding(BarcodeEntry.IsReadOnlyProperty, new Binding("Model.ReadingIsReadOnly", source: BindingContext));
            BarcodeEntry.TextChanged += Reading_OnTextChanged;
            BarcodeEntry.Unfocused += Reading_OnUnfocused;
            _viewModel.Model.ReadingFocus += BarcodeEntry.Focus;

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding(nameof(_viewModel.ConfirmationCommand), source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.ClearCommandProperty, new Binding(nameof(_viewModel.ClearCommand), source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.GoBackCommandProperty, new Binding(nameof(_viewModel.GoBackCommand), source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.MenuAdtionalButtonsProperty, new Binding(nameof(_viewModel.MenuAdtionalButtons), source: BindingContext));

            _viewModel.Model.CarNumberFocus += CarNumberFocus;
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
                Team.Focus();
            });
        }

        private void CarNumberFocus()
        {
            CarNumber.Focus();
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
