using System.Collections.Generic;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Interfaces.Services
{
    public interface IBoardingService
    {
        Task<HttpRequestResult<VehicleViewInfoModel>> GetBoardingByTrafficScheduleId(int unitId, string unitCode,
            string carNumber, int teamId, bool isJointUnit);
        Task<HttpRequestResult<PackingListViewInfoModel>> GetPackingListIdBoarding(int trafficScheduleId, int unitLocal,
            int unitSend, MacAddressModel macAddress, bool isJointUnit);
        Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackBoarding(ReadingPackBoardingModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> ReadingTransportAccessory(
            ReadingTransportAccessoryModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackingListTransportAccessory(
            ReadingPackingListTransportAccessoryModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> RemovePackingListTransportAccessory(int packingListId,
            string cobolNumber, int unitSendId, MacAddressModel macAddress);
        Task<HttpRequestResult<PackingListViewInfoModel>> CancelPackBoarding(CancelPackBoardingModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> CancelBillOfLadingBoarding(
            CancelBillOfLadingBoardingModel model);
        Task<HttpRequestResult<VehicleViewInfoModel>> StartingBoarding(StartingBoardingModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> EndingBoarding(EndingBoardingModel model);
        Task<HttpRequestResult<PackingListDetailViewInfoModel>> GetViewLackBoarding(int packingListId,
            int billOfLadingNumber, string billOfLadingDigit, string unitEmissionCode);
        Task<HttpRequestResult<IEnumerable<PackingListDetailViewInfoModel>>> GetViewLackBoardingByTrafficScheduleDetail(
            int packingListId, int unitId, int trafficScheduleDetailId);
        Task<HttpRequestResult<PackingListViewInfoModel>> GetPackingListWeight(int packingListId);
        Task<HttpRequestResult<VehicleViewInfoModel>> GetJointTrafficSchedule(int trafficScheduleDetailUnitId);
    }
}