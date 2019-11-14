using System.Collections.Generic;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Services
{
    public class LandingService : ServiceBase, ILandingService
    {
        public string ApiController { get; private set; }

        public LandingService()
        {
            ApiController = "ws";
        }

        public async Task<HttpRequestResult<VehicleViewInfoModel>> GetLandingByTrafficScheduleId(int unitId,
            string unitCode, string carNumber, int teamId)
        {
            return await GetAsyncAuthenticatedData<VehicleViewInfoModel>(
                $"{ApiController}/landing-by-traffic-schedule-id/{unitId}/{unitCode}/{carNumber}/{teamId}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackLanding(ReadingPackLandingModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, ReadingPackLandingModel>(
                $"{ApiController}/reading-pack-landing", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> ReadingTransportAccessoryLanding(
            ReadingTransportAccessoryLandingModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, ReadingTransportAccessoryLandingModel>(
                $"{ApiController}/reading-transport-accessory-landing", model);
        }

        public async Task<HttpRequestResult<PackingListDetailViewInfoModel>> GetViewLackLanding(
            int trafficScheduleDetailId, int billOfLadingNumber, string billOfLadingDigit,
            string unitEmissionCode)
        {
            return await GetAsyncAuthenticatedData<PackingListDetailViewInfoModel>(
                $"{ApiController}/view-lack-landing/{trafficScheduleDetailId}/{billOfLadingNumber}/{billOfLadingDigit}/{unitEmissionCode}");
        }

        public async Task<HttpRequestResult<IEnumerable<PackingListDetailViewInfoModel>>>
            GetViewLackLandingByTrafficScheduleDetail(int trafficScheduleDetailId)
        {
            return await GetAsyncAuthenticatedData<IEnumerable<PackingListDetailViewInfoModel>>(
                $"{ApiController}/view-lack-landing-by-traffic-schedule-detail/{trafficScheduleDetailId}");
        }

        public async Task<HttpRequestResult<VehicleViewInfoModel>> StartingLanding(StartingLandingModel model)
        {
            return await PostAsyncReceiveData<VehicleViewInfoModel, StartingLandingModel>(
                $"{ApiController}/starting-landing", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> EndingLanding(EndingLandingModel model)
        {
            return await PutAsyncReceiveData<PackingListViewInfoModel, EndingLandingModel>(
                $"{ApiController}/ending-landing", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> CancelPackLanding(CancelPackLandingModel model)
        {
            return await PutAsyncReceiveData<PackingListViewInfoModel, CancelPackLandingModel>(
                $"{ApiController}/cancel-pack-landing", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> CancelPackingListTransportAccessoryLanding(
            CancelPackingListTransportAccessoryLandingModel model)
        {
            return await PutAsyncReceiveData<PackingListViewInfoModel, CancelPackingListTransportAccessoryLandingModel>(
                $"{ApiController}/cancel-packing-list-transport-accessory-landing", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> CancelBillOfLadingLanding(
            CancelBillOfLadingLandingModel model)
        {
            return await PutAsyncReceiveData<PackingListViewInfoModel, CancelBillOfLadingLandingModel>(
                $"{ApiController}/cancel-bill-of-fading-landing", model);
        }
    }
}