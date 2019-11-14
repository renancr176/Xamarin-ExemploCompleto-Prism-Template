using System.Collections.Generic;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Services
{
    public class BoardingService : ServiceBase, IBoardingService
    {
        public string ApiController { get; private set; }

        public BoardingService()
        {
            ApiController = "ws";
        }

        public async Task<HttpRequestResult<VehicleViewInfoModel>> GetBoardingByTrafficScheduleId(int unitId,
            string unitCode, string carNumber, int teamId, bool isJointUnit)
        {
            if (!string.IsNullOrEmpty(carNumber.Trim()))
                return await GetAsyncAuthenticatedData<VehicleViewInfoModel>(
                    $"{ApiController}/boarding-by-traffic-schedule-id/{unitId}/{unitCode}/{carNumber}/{teamId}/{isJointUnit}");

            return await GetAsyncAuthenticatedData<VehicleViewInfoModel>(
                $"{ApiController}/boarding-by-traffic-schedule-id/{unitId}/{unitCode}/{teamId}/{isJointUnit}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> GetPackingListIdBoarding(int trafficScheduleId,
            int unitLocal, int unitSend, MacAddressModel macAddress, bool isJointUnit)
        {
            return await GetAsyncAuthenticatedData<PackingListViewInfoModel>(
                $"{ApiController}/packing-list-id-boarding/{trafficScheduleId}/{unitLocal}/{unitSend}/{macAddress}/{isJointUnit}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackBoarding(ReadingPackBoardingModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, ReadingPackBoardingModel>(
                $"{ApiController}/reading-pack-boarding", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> ReadingTransportAccessory(
            ReadingTransportAccessoryModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, ReadingTransportAccessoryModel>(
                $"{ApiController}/reading-transport-accessory", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackingListTransportAccessory(
            ReadingPackingListTransportAccessoryModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, ReadingPackingListTransportAccessoryModel>(
                $"{ApiController}/reading-packing-list-transport-accessory", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> RemovePackingListTransportAccessory(
            int packingListId, string cobolNumber, int unitSendId, MacAddressModel macAddress)
        {
            return await DeleteAsyncReceiveData<PackingListViewInfoModel>(
                $"{ApiController}/remove-packing-list-transport-accessory/{packingListId}/{cobolNumber}/{unitSendId}/{macAddress}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> CancelPackBoarding(CancelPackBoardingModel model)
        {
            return await PutAsyncReceiveData<PackingListViewInfoModel, CancelPackBoardingModel>(
                $"{ApiController}/cancel-pack-boarding", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> CancelBillOfLadingBoarding(
            CancelBillOfLadingBoardingModel model)
        {
            return await PutAsyncReceiveData<PackingListViewInfoModel, CancelBillOfLadingBoardingModel>(
                $"{ApiController}/cancel-bill-of-lading-boarding", model);
        }

        public async Task<HttpRequestResult<VehicleViewInfoModel>> StartingBoarding(StartingBoardingModel model)
        {
            return await PostAsyncReceiveData<VehicleViewInfoModel, StartingBoardingModel>(
                $"{ApiController}/starting-boarding", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> EndingBoarding(EndingBoardingModel model)
        {
            return await PutAsyncReceiveData<PackingListViewInfoModel, EndingBoardingModel>(
                $"{ApiController}/ending-boarding", model);
        }

        public async Task<HttpRequestResult<PackingListDetailViewInfoModel>> GetViewLackBoarding(int packingListId,
            int billOfLadingNumber, string billOfLadingDigit, string unitEmissionCode)
        {
            return await GetAsyncAuthenticatedData<PackingListDetailViewInfoModel>(
                $"{ApiController}/view-lack-boarding/{packingListId}/{billOfLadingNumber}/{billOfLadingDigit}/{unitEmissionCode}");
        }

        public async Task<HttpRequestResult<IEnumerable<PackingListDetailViewInfoModel>>> GetViewLackBoardingByTrafficScheduleDetail(
            int packingListId, int unitId, int trafficScheduleDetailId)
        {
            return await GetAsyncAuthenticatedData<IEnumerable<PackingListDetailViewInfoModel>>(
                $"{ApiController}/view-lack-boarding-by-traffic-schedule-detail/{packingListId}/{unitId}/{trafficScheduleDetailId}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> GetPackingListWeight(int packingListId)
        {
            return await GetAsyncAuthenticatedData<PackingListViewInfoModel>(
                $"{ApiController}/packing-list-weight/{packingListId}");
        }

        public async Task<HttpRequestResult<VehicleViewInfoModel>> GetJointTrafficSchedule(int trafficScheduleDetailUnitId)
        {
            return await GetAsyncAuthenticatedData<VehicleViewInfoModel>(
                $"{ApiController}/joint-traffic-schedule/{trafficScheduleDetailUnitId}");
        }
    }
}