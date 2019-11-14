using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Services
{
    public class CommonService : ServiceBase, ICommonService
    {
        public string ApiController { get; private set; }

        public CommonService()
        {
            ApiController = "ws";
        }

        public async Task<HttpRequestResult<PackingListDetailViewInfoModel>> GetWarehousePasswordId(string password)
        {
            return await GetAsyncAuthenticatedData<PackingListDetailViewInfoModel>(
                $"{ApiController}/warehouse-password-id/{password}");
        }

        public async Task<HttpResponseMessage> ReleaseWarehousePassword(string password)
        {
            return await GetAsync($"{ApiController}/release-warehouse-password/{password}");
        }

        public async Task<HttpRequestResult<UnitViewInfoModel>> ValidUnit(string unitCode)
        {
            return await GetAsyncAnonimouslyData<UnitViewInfoModel>(
                $"{ApiController}/valid-unit/{unitCode}");
        }

        public async Task<HttpRequestResult<TeamViewInfoModel>> ValidTeam(int unitId, string teamCode)
        {
            return await GetAsyncAuthenticatedData<TeamViewInfoModel>(
                $"{ApiController}/valid-team/{unitId}/{teamCode}");
        }

        public async Task<HttpRequestResult<VehicleViewInfoModel>> ValidVehicle(string carNumber)
        {
            return await GetAsyncAuthenticatedData<VehicleViewInfoModel>(
                $"{ApiController}/valid-vehicle/{carNumber}");
        }

        public async Task<HttpRequestResult<IEnumerable<VehicleViewInfoModel>>> GetVehiclesInOperationalControl(int unitId, int unitSendId, int vehicleId)
        {
            return await GetAsyncAuthenticatedData<IEnumerable<VehicleViewInfoModel>>(
                $"{ApiController}/vehicles-in-operational-control/{unitId}/{unitSendId}/{vehicleId}");
        }

        public async Task<HttpRequestResult<IEnumerable<VehicleViewInfoModel>>> GetTrafficScheduleByCarNumber(string unitLocalCode, string carNumber)
        {
            return await GetAsyncAuthenticatedData<IEnumerable<VehicleViewInfoModel>>(
                $"{ApiController}/traffic-schedule-by-car-number/{unitLocalCode}/{carNumber}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> PauseBoardingOrLanding(int operationalControlId)
        {
            return await PutAsyncReceiveData<PackingListViewInfoModel, object>(
                $"{ApiController}/pause-boarding-orLanding/{operationalControlId}", null);
        }
    }
}