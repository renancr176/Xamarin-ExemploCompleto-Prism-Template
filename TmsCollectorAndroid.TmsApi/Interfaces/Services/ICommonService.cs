using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Interfaces.Services
{
    public interface ICommonService
    {
        Task<HttpRequestResult<PackingListDetailViewInfoModel>> GetWarehousePasswordId(string password);
        Task<HttpResponseMessage> ReleaseWarehousePassword(string password);
        Task<HttpRequestResult<UnitViewInfoModel>> ValidUnit(string unitCode);
        Task<HttpRequestResult<TeamViewInfoModel>> ValidTeam(int unitId, string teamCode);
        Task<HttpRequestResult<VehicleViewInfoModel>> ValidVehicle(string carNumber);
        Task<HttpRequestResult<IEnumerable<VehicleViewInfoModel>>> GetVehiclesInOperationalControl(int unitId, int unitSendId, int vehicleId);
        Task<HttpRequestResult<IEnumerable<VehicleViewInfoModel>>> GetTrafficScheduleByCarNumber(string unitLocalCode, string carNumber);
        Task<HttpRequestResult<PackingListViewInfoModel>> PauseBoardingOrLanding(int operationalControlId);
    }
}