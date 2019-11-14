using System;
using Prism.Mvvm;
using TmsCollectorAndroid.Interfaces.Services;
using Xamarin.Forms;

namespace TmsCollectorAndroid.ViewModels.Controls
{
    public class HeaderViewModel : BindableBase
    {
        public HeaderViewModel(IUserService userService)
        {
            _userService = userService;
        }

        private readonly IUserService _userService;

        public string Unit => _userService.User.Unit.Code +
        ((_userService.User.Unit.IsJointUnit)? String.Empty : "*");

        public Color UnitColor =>
            ((_userService.User.Unit.IsJointUnit) ? Color.Default : Color.FromHex("#FF0000"));

        public string CompanyAcronym => _userService.User.CompanyAcronym;
    }
}