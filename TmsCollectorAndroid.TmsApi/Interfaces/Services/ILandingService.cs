using System.Collections.Generic;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Interfaces.Services
{
    public interface ILandingService
    {
        Task<HttpRequestResult<VehicleViewInfoModel>> GetLandingByTrafficScheduleId(int unitId, string unitCode,
            string carNumber, int teamId);
        Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackLanding(ReadingPackLandingModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> ReadingTransportAccessoryLanding(
            ReadingTransportAccessoryLandingModel model);
        Task<HttpRequestResult<PackingListDetailViewInfoModel>> GetViewLackLanding(int trafficScheduleDetailId,
            int billOfLadingNumber, string billOfLadingDigit, string unitEmissionCode);
        Task<HttpRequestResult<IEnumerable<PackingListDetailViewInfoModel>>> GetViewLackLandingByTrafficScheduleDetail(
            int trafficScheduleDetailId);
        Task<HttpRequestResult<VehicleViewInfoModel>> StartingLanding(StartingLandingModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> EndingLanding(EndingLandingModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> CancelPackLanding(CancelPackLandingModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> CancelPackingListTransportAccessoryLanding(
            CancelPackingListTransportAccessoryLandingModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> CancelBillOfLadingLanding(CancelBillOfLadingLandingModel model);
    }
}