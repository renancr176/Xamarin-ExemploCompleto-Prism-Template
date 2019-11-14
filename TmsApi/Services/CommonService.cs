using Flurl.Http;
using TmsCollectorAndroid.Api_Old.Interfaces.Services;
using TmsCollectorAndroid.Api_Old.Models;
using TmsCollectorAndroid.Api_Old.Models.Responses;
using TmsCollectorAndroid.Api_Old.Models.Senders;

namespace TmsCollectorAndroid.Api_Old.Services
{
    public class CommonService : ServiceBase, ICommonService
    {
        public string Controller { get; private set; }

        public CommonService()
        {
            Controller = "ws";
        }

        public async Task<HttpRequestResult<PackingListDetailViewInfoModel>> GetWarehousePasswordId(string password)
        {
            try
            {
                return await ApiUrl.AppendPathSegment($"{Controller}/warehouse-password-id/{password}")
                    .PostJsonAsync(EmployeeViewInfo)
                    .HttpRequestResult<PackingListDetailViewInfoModel>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<PackingListDetailViewInfoModel>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<PackingListDetailViewInfoModel>(HttpStatusCode.BadRequest);
        }

        public async Task<HttpResponseMessage> ReleaseWarehousePassword(string password)
        {
            try
            {
                return await ApiUrl.AppendPathSegment($"{Controller}/release-warehouse-password/{password}")
                    .PostJsonAsync(EmployeeViewInfo);
            }
            catch (FlurlHttpException e)
            {
                return new HttpResponseMessage(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        public async Task<HttpRequestResult<UnitViewInfoModel>> ValidUnit(string unitCode)
        {
            try
            {
                return await ApiUrl.AppendPathSegment($"{Controller}/valid-unit/{unitCode}")
                    .GetAsync()
                    .HttpRequestResult<UnitViewInfoModel>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<UnitViewInfoModel>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<UnitViewInfoModel>(HttpStatusCode.BadRequest);
        }

        public async Task<HttpRequestResult<TeamViewInfoModel>> ValidTeam(int unitId, string teamCode)
        {
            try
            {
                return await ApiUrl.AppendPathSegment($"{Controller}/valid-team/{unitId}/{teamCode}")
                    .PostJsonAsync(EmployeeViewInfo)
                    .HttpRequestResult<TeamViewInfoModel>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<TeamViewInfoModel>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<TeamViewInfoModel>(HttpStatusCode.BadRequest);
        }

        public async Task<HttpRequestResult<VehicleViewInfoModel>> ValidVehicle(string carNumber)
        {
            try
            {
                return await ApiUrl.AppendPathSegment($"{Controller}/valid-vehicle/{carNumber}")
                    .PostJsonAsync(EmployeeViewInfo)
                    .HttpRequestResult<VehicleViewInfoModel>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<VehicleViewInfoModel>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<VehicleViewInfoModel>(HttpStatusCode.BadRequest);
        }

        public async Task<HttpRequestResult<IEnumerable<VehicleViewInfoModel>>> GetVehiclesInOperationalControl(GetVehiclesInOperationalControlModel model)
        {
            model.AuthenticateInfo = EmployeeViewInfo;
            try
            {
                return await ApiUrl.AppendPathSegment($"{Controller}/vehicles-in-operational-control")
                    .PostJsonAsync(model)
                    .HttpRequestResult<IEnumerable<VehicleViewInfoModel>>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<IEnumerable<VehicleViewInfoModel>>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<IEnumerable<VehicleViewInfoModel>>(HttpStatusCode.BadRequest);
        }

        public async Task<HttpRequestResult<IEnumerable<VehicleViewInfoModel>>> GetTrafficScheduleByCarNumber(string unitLocalCode, string carNumber)
        {
            try
            {
                return await ApiUrl.AppendPathSegment($"{Controller}/traffic-schedule-by-car-number/{unitLocalCode}/{carNumber}")
                    .PostJsonAsync(EmployeeViewInfo)
                    .HttpRequestResult<IEnumerable<VehicleViewInfoModel>>();
            }
            catch (FlurlHttpException e)
            {
                return new HttpRequestResult<IEnumerable<VehicleViewInfoModel>>(e.Call.HttpStatus ?? HttpStatusCode.BadRequest);
            }
            catch (Exception) { }

            return new HttpRequestResult<IEnumerable<VehicleViewInfoModel>>(HttpStatusCode.BadRequest);
        }
    }
}