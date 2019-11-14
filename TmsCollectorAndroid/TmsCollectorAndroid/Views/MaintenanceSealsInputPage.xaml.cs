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

            ProcessDefaultButton.SetBinding(ProcessDefaultButton.FirstButtonCommandProperty, new Binding("ConfirmationCommand", source: BindingContext));
            ProcessDefaultButton.SetBinding(ProcessDefaultButton.SecondButtonCommandProperty, new Binding("CancelCommand", source: BindingContext));

            _viewModel.Model.SealFocus += ExecuteSealFocus;
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
