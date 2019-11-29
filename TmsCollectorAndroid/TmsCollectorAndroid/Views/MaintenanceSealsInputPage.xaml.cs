using System;
using System.Threading.Tasks;
using TmsCollectorAndroid.ViewModels;
using TmsCollectorAndroid.Views.Controls;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class MaintenanceSealsInputPage : ContentPage
    {
        private readonly MaintenanceSealsInputPageViewModel _viewModel;
        public MaintenanceSealsInputPage()
        {
            InitializeComponent();

            _viewModel = (MaintenanceSealsInputPageViewModel) BindingContext;

            NavigationPage.SetHasNavigationBar(this, false);

            Header.SetBinding(Header.TitleProperty, new Binding("Title", source: BindingContext));

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.SecondButtonCommandProperty, new Binding("ConfirmationCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("CancelCommand", source: BindingContext));

            _viewModel.Model.SealFocus += ExecuteSealFocus;
            _viewModel.Model.BtnConfirmationFocus += ExecuteBtnConfirmationFocus;
        }

        private void ExecuteBtnConfirmationFocus()
        {
            ProcessDefaultButton.SecondButtonFocus();
        }

        protected override void OnAppearing()
        {
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                Seal.Focus();
            });
        }

        private void ExecuteSealFocus()
        {
            Seal.Focus();
        }
    }
}
