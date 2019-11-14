using System;
using Prism.Mvvm;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.ViewModels.Controls
{
    public class FooterViewModel : BindableBase
    {
        public FooterViewModel(IUserService userService)
        {
            _userService = userService;
            Date = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private readonly IUserService _userService;

        public string UserName => _userService.User.EmployeeAuthenticated.UserLogin;

        private string _date;
        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }
    }
}