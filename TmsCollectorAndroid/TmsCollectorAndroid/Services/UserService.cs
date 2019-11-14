using System.Threading.Tasks;
using Prism.Navigation;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Services
{
    public class UserService : IUserService
    {
        public bool SetToClose { get; private set; }
        public bool IsLogedIn { get; private set; }
        public UserModel User { get; private set; }
        
        public UserService(INavigationService navigationService,
            IServiceBase tmsApiServiceBase,
            ICommonService commonService)
        {
            _navigationService = navigationService;
            _tmsApiServiceBase = tmsApiServiceBase;
            _commonService = commonService;

            SetToClose = false;
            IsLogedIn = false;
        }

        private readonly INavigationService _navigationService;
        private readonly IServiceBase _tmsApiServiceBase;
        private readonly ICommonService _commonService;

        public async Task<AuthenticationResponseModel> LogIn(LoginModel model)
        {
            var unit = await _commonService.ValidUnit(model.Unit);

            if (!unit.IsSuccessStatusCode)
                return new AuthenticationResponseModel(false, "Unidade inválida.");

            var result = await _tmsApiServiceBase.Authenticate(model.CompanyId, model.UserName, model.UserPassword, SetToClose);

            IsLogedIn = result.IsAuthenticated;

            if (result.IsAuthenticated)
            {
                User = new UserModel(unit.Response, _tmsApiServiceBase.EmployeeViewInfo);
            }

            return result;
        }

        public async void LogOut()
        {
            IsLogedIn = false;
            User = null;

            await _tmsApiServiceBase.FinalizeSession();

            await _navigationService.NavigateAsync("/LoginPage");
        }

        public EmployeeViewInfoModel EmployeeAuthenticated()
        {
            return _tmsApiServiceBase.EmployeeViewInfo;
        }
    }
}