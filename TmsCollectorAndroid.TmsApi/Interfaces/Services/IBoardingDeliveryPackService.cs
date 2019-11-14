using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Interfaces.Services
{
    public interface IBoardingDeliveryPackService
    {
        Task<HttpRequestResult<LineInfoViewModel>> ValidLine(int unitId, string lineCode, DateTime checkOutDate);
        Task<HttpRequestResult<DriverInfoViewModel>> ValidDriver(int driverId);
        Task<HttpRequestResult<PackingListViewInfoModel>> GetPackingListDelivery(GetPackingListDeliveryModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> DeletePackingListDelivery(int packingListDeliveryId,
            MacAddressModel macAddress);
        Task<HttpRequestResult<PackingListViewInfoModel>> EndingDeliveryBoarding(EndingDeliveryBoardingModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackDelivery(ReadingPackDeliveryModel model);
        Task<HttpRequestResult<IEnumerable<PackingListDetailViewInfoModel>>> GetViewLackDeliveryBoardingByTrafficScheduleDetail(
            int packingListId, int unitId, int trafficScheduleDetailIdl);
    }
}