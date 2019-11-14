using System;
using Prism.Mvvm;

namespace TmsCollectorAndroid.Models
{
    public class LoginModel : BindableBase
    {
        public LoginModel()
        {
        }

        #region Controle Inputs

        private bool _userNameIsEnabled;
        public bool UserNameIsEnabled
        {
            get { return _userNameIsEnabled; }
            set { SetProperty(ref _userNameIsEnabled, value); }
        }

        private bool _userPasswordIsEnabled;
        public bool UserPasswordIsEnabled
        {
            get { return _userPasswordIsEnabled; }
            set { SetProperty(ref _userPasswordIsEnabled, value); }
        }

        private bool _btnConfirmationIsEnabled;
        public bool BtnConfirmationIsEnabled
        {
            get { return _btnConfirmationIsEnabled; }
            set { SetProperty(ref _btnConfirmationIsEnabled, value); }
        }

        #endregion

        private string _unit;
        public string Unit
        {
            get { return _unit; }
            set { SetProperty(ref _unit, value); }
        }

        public int CompanyId { get; set; }

        private string _unitDescription;
        public string UnitDescription
        {
            get { return _unitDescription; }
            set { SetProperty(ref _unitDescription, value); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value?.ToUpper());
                BtnConfirmationIsEnabled = (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(UserPassword));
            }
        }

        public Action UserNameFocus;

        private string _userPassword;
        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                SetProperty(ref _userPassword, value?.ToUpper());
                BtnConfirmationIsEnabled = (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(UserName));
            }
        }

        public void ClearModel()
        {
            UserNameIsEnabled = UserPasswordIsEnabled = BtnConfirmationIsEnabled = false;
        }
    }
}