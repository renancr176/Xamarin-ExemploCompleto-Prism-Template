using System.Collections.Generic;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Services
{
    public class BoardingAccessoryService : ServiceBase, IBoardingAccessoryService
    {
        public string ApiController { get; private set; }

        public BoardingAccessoryService()
        {
            ApiController = "ws";
        }

        public async Task<HttpRequestResult<TransportAccessoryViewInfoModel>> ValidTransportAccessory(string cobolNumber, int unitLocalId)
        {
            return await GetAsyncAuthenticatedData<TransportAccessoryViewInfoModel>(
                $"{ApiController}/valid-transport-accessory/{cobolNumber}/{unitLocalId}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> GetPackingListAccessory(GetPackingListAccessoryModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, GetPackingListAccessoryModel>(
                $"{ApiController}/packing-list-accessory", model);
        }

        public async Task<HttpRequestResult<IEnumerable<PackingListViewInfoModel>>> GetListPackingListAccessoryByPallet(GetListPackingListAccessoryByPalletModel model)
        {
            return await PostAsyncReceiveData<IEnumerable<PackingListViewInfoModel>, GetListPackingListAccessoryByPalletModel>(
                $"{ApiController}/list-packing-list-accessory-by-pallet", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackAccessory(ReadingPackAccessoryModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, ReadingPackAccessoryModel>(
                $"{ApiController}/reading-pack-accessory", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> ValidClosePackingListAccessory(ValidClosePackingListAccessoryModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, ValidClosePackingListAccessoryModel>(
                $"{ApiController}/valid-close-packing-list-accessory", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> ClosePackingListAccessory(int packingListAccessoryId, MacAddressModel macAddress)
        {
            return await GetAsyncAuthenticatedData<PackingListViewInfoModel>(
                $"{ApiController}/close-packing-list-accessory/{packingListAccessoryId}/{macAddress}");
        }

        public async Task<HttpRequestResult<GetSealsResponseModel>> GetSeals(int packingListAccessoryId)
        {
            return await GetAsyncAuthenticatedData<GetSealsResponseModel>(
                $"{ApiController}/get-seals/{packingListAccessoryId}");
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> AddSeal(AddSealModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, AddSealModel>(
                $"{ApiController}/add-seal", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> RemoveSeal(RemoveSealModel model)
        {
            return await PutAsyncReceiveData<PackingListViewInfoModel, RemoveSealModel>(
                $"{ApiController}/remove-seal", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> LinkPack(LinkPackModel model)
        {
            return await PostAsyncReceiveData<PackingListViewInfoModel, LinkPackModel>(
                $"{ApiController}/link-pack", model);
        }

        public async Task<HttpRequestResult<PackingListViewInfoModel>> DeletePackingListAccessory(int packingListAccessoryId)
        {
            return await DeleteAsyncReceiveData<PackingListViewInfoModel>(
                $"{ApiController}/delete-packing-list-accessory/{packingListAccessoryId}");
        }

        public async Task<HttpRequestResult<IEnumerable<TransportAccessoryViewInfoModel>>> GetPallets()
        {
            return await GetAsyncAuthenticatedData<IEnumerable<TransportAccessoryViewInfoModel>>(
                $"{ApiController}/pallets");
        }
    }
}