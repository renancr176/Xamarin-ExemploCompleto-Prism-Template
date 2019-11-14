using Flurl;
using TmsCollectorAndroid.Api_Old.Models;

namespace TmsCollectorAndroid.Api_Old.Interfaces.Services
{
    public interface IServiceBase
    {
        Url ApiUrl { get; }
        EmployeeViewInfoModel EmployeeViewInfo { get; }

        void SetApiUrl(IEnumerable<string> baseUrls);
        Task<bool> TestConection();
        Task<bool> Authenticate(int companyId, string userName, string userPassword, bool setToClose);

        Task<string> CheckVersion();
        Task<string> GetWebServiceVersion();
    }
}