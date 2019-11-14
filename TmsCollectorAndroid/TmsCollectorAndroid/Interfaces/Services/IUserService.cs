using System.Threading.Tasks;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Interfaces.Services
{
    public interface IUserService
    {
        bool IsLogedIn { get; }
        UserModel User { get; }

        Task<AuthenticationResponseModel> LogIn(LoginModel model);
        void LogOut();
    }
}