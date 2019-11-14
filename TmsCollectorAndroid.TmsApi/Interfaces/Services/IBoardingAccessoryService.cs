using System.Collections.Generic;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Interfaces.Services
{
    public interface IBoardingAccessoryService
    {
        Task<HttpRequestResult<TransportAccessoryViewInfoModel>> ValidTransportAccessory(string cobolNumber,
            int unitLocalId);
        Task<HttpRequestResult<PackingListViewInfoModel>> GetPackingListAccessory(GetPackingListAccessoryModel model);
        Task<HttpRequestResult<IEnumerable<PackingListViewInfoModel>>> GetListPackingListAccessoryByPallet(
            GetListPackingListAccessoryByPalletModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> ReadingPackAccessory(ReadingPackAccessoryModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> ValidClosePackingListAccessory(
            ValidClosePackingListAccessoryModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> ClosePackingListAccessory(int packingListAccessoryId, MacAddressModel macAddress);
        Task<HttpRequestResult<GetSealsResponseModel>> GetSeals(int packingListAccessoryId);
        Task<HttpRequestResult<PackingListViewInfoModel>> AddSeal(AddSealModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> RemoveSeal(RemoveSealModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> LinkPack(LinkPackModel model);
        Task<HttpRequestResult<PackingListViewInfoModel>> DeletePackingListAccessory(int packingListAccessoryId);
        Task<HttpRequestResult<IEnumerable<TransportAccessoryViewInfoModel>>> GetPallets();
    }
}