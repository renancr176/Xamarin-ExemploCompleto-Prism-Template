using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Services
{
    public class LandingDeliveryPackService : ServiceBase, ILandingDeliveryPackService
    {
        public string ApiController { get; private set; }

        public LandingDeliveryPackService()
        {
            ApiController = "ws";
        }

        public async Task<HttpRequestResult<IEnumerable<PackingListViewInfoModel>>> GetListPackingListDeliveryByReturn(int unitLocalId, int packingListNumber, int packingListDigit)
        {
            return await GetAsyncAuthenticatedData<IEnumerable<PackingListViewInfoModel>>(
                $"{ApiController}/list-packing-list-delivery-by-return/{unitLocalId}/{packingListNumber}/{packingListDigit}");
        }

        public async Task<HttpRequestResult<IEnumerable<PackingListViewInfoModel>>> GetListPackingListDeliveryByReturn(int unitLocalId, DateTime checkOutDate, int driverId, int vehicleId)
        {
            return await GetAsyncAuthenticatedData<IEnumerable<PackingListViewInfoModel>>(
                $"{ApiController}/list-packing-list-delivery-by-return/{unitLocalId}/{checkOutDate.ToString("yyyy-MM-dd")}/{checkOutDate.ToString("HH")}/{checkOutDate.ToString("mm")}/{driverId}/{vehicleId}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackDeliveryReturn(
            ReadingPackDeliveryReturnModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, ReadingPackDeliveryReturnModel>(
                $"{ApiController}/reading-pack-delivery-return", model);
        }

        public async Task<HttpRequestResult<GetDriverAndVehicleResponseModel>> GetDriverAndVehicle(int packingListDeliveryId)
        {
            return await GetAsyncAuthenticatedData<GetDriverAndVehicleResponseModel>(
                $"{ApiController}/driver-and-vehicle/{packingListDeliveryId}");
        }
    }
}