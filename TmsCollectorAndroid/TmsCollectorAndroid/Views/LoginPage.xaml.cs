using System;
using System.Threading.Tasks;
using TmsCollectorAndroid.ViewModels;
using Xamarin.Forms;

namespace TmsCollectorAndroid.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginPageViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            _viewModel = (LoginPageViewModel) BindingContext;

            _viewModel.Model.UserNameFocus += UserNameFocus;
        }

        protected override void OnAppearing()
        {
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                Unit.Focus();
            });
        }

        private void UserNameFocus()
        {
            UserName.Focus();
        }

        private void UserName_OnUnfocused(object sender, FocusEventArgs e)
        {
            UserPassword.Focus();
        }

        private void UserPassword_OnUnfocused(object sender, FocusEventArgs e)
        {
            BtnConfirmation.Focus();
        }
    }
}
