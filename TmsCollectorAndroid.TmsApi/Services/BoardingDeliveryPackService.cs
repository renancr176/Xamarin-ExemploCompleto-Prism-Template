using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Services
{
    public class BoardingDeliveryPackService : ServiceBase, IBoardingDeliveryPackService
    {
        public string ApiController { get; private set; }

        public BoardingDeliveryPackService()
        {
            ApiController = "ws";
        }

        public async Task<HttpRequestResult<LineInfoViewModel>> ValidLine(int unitId, string lineCode,
            DateTime checkOutDate)
        {
            return await GetAsyncAuthenticatedData<LineInfoViewModel>(
                $"{ApiController}/valid-line/{unitId}/{lineCode}/{checkOutDate.ToString("yyyy-MM-dd")}/{checkOutDate.Hour}/{checkOutDate.Minute}");
        }

        public async Task<HttpRequestResult<DriverInfoViewModel>> ValidDriver(int driverId)
        {
            return await GetAsyncAuthenticatedData<DriverInfoViewModel>(
                $"{ApiController}/valid-driver/{driverId}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> GetPackingListDelivery(
            GetPackingListDeliveryModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, GetPackingListDeliveryModel>(
                $"{ApiController}/packing-list-delivery", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> DeletePackingListDelivery(int packingListDeliveryId, MacAddressModel macAddress)
        {
            return await DeleteAsyncReceiveData<PackingListViewInfoModel>(
                $"{ApiController}/packing-list-delivery/{packingListDeliveryId}/{macAddress}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> EndingDeliveryBoarding(
            EndingDeliveryBoardingModel model)
        {
            return await PutAsyncReceiveData<PackingListViewInfoModel, EndingDeliveryBoardingModel>(
                $"{ApiController}/ending-delivery-boarding", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackDelivery(
            ReadingPackDeliveryModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, ReadingPackDeliveryModel>(
                $"{ApiController}/reading-pack-delivery", model);
        }

        public async Task<HttpRequestResult<IEnumerable<PackingListDetailViewInfoModel>>>
            GetViewLackDeliveryBoardingByTrafficScheduleDetail(int packingListId, int unitId,
                int trafficScheduleDetailIdl)
        {
            return await GetAsyncAuthenticatedData<IEnumerable<PackingListDetailViewInfoModel>>(
                $"{ApiController}/view-lack-delivery-boarding-by-traffic-schedule-detail/{packingListId}/{unitId}/{trafficScheduleDetailIdl}");
        }
    }
}