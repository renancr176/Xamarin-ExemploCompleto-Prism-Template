using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Interfaces.Services
{
    public interface IServiceBase
    {
        Url ApiUrl { get; }
        EmployeeViewInfoModel EmployeeViewInfo { get; }

        void SetApiUrl(IEnumerable<string> baseUrls);
        Task<bool> TestConection();
        Task<AuthenticationResponseModel> Authenticate(int companyId, string userName, string userPassword);
        Task FinalizeSession();
        Task<string> CheckVersion();
        Task<string> GetWebServiceVersion();
    }
}