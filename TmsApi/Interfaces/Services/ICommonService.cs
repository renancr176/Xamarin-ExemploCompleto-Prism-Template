using TmsCollectorAndroid.Api_Old.Models;
using TmsCollectorAndroid.Api_Old.Models.Responses;
using TmsCollectorAndroid.Api_Old.Models.Senders;

namespace TmsCollectorAndroid.Api_Old.Interfaces.Services
{
    public interface ICommonService
    {
        Task<HttpRequestResult<PackingListDetailViewInfoModel>> GetWarehousePasswordId(string password);
        Task<HttpResponseMessage> ReleaseWarehousePassword(string password);
        Task<HttpRequestResult<UnitViewInfoModel>> ValidUnit(string unitCode);
        Task<HttpRequestResult<TeamViewInfoModel>> ValidTeam(int unitId, string teamCode);
        Task<HttpRequestResult<VehicleViewInfoModel>> ValidVehicle(string carNumber);
        Task<HttpRequestResult<IEnumerable<VehicleViewInfoModel>>> GetVehiclesInOperationalControl(GetVehiclesInOperationalControlModel model);
        Task<HttpRequestResult<IEnumerable<VehicleViewInfoModel>>> GetTrafficScheduleByCarNumber(string unitLocalCode, string carNumber);
    }
}