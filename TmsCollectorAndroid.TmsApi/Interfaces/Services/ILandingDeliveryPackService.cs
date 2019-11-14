using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Interfaces.Services
{
    public interface ILandingDeliveryPackService
    {
        Task<HttpRequestResult<IEnumerable<PackingListViewInfoModel>>> GetListPackingListDeliveryByReturn(
            int unitLocalId, int packingListNumber, int packingListDigit);
        Task<HttpRequestResult<IEnumerable<PackingListViewInfoModel>>> GetListPackingListDeliveryByReturn(
            int unitLocalId, DateTime checkOutDate, int driverId, int vehicleId);
        Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackDeliveryReturn(
            ReadingPackDeliveryReturnModel model);
        Task<HttpRequestResult<GetDriverAndVehicleResponseModel>> GetDriverAndVehicle(int packingListDeliveryId);
    }
}