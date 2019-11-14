using System.Net.Http;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Services
{
    public class BillOfLadingService : ServiceBase, IBillOfLadingService
    {
        public string ApiController { get; private set; }

        public BillOfLadingService()
        {
            ApiController = "ws";
        }

        public async Task<HttpRequestResult<BillOfLadingCollectorViewInfoModel>> GetBillOfLading(int cobolNumber, string cobolDigit, string unitEmissionCode)
        {
            return await GetAsyncAuthenticatedData<BillOfLadingCollectorViewInfoModel>(
                $"{ApiController}/bill-of-lading/{cobolNumber}/{cobolDigit}/{unitEmissionCode}");
        }

        public async Task<HttpRequestResult<GetBillOfLadingPackIdResponseModel>> GetBillOfLadingPackId(int billOfLadingNumber, string billOfLadingDigit, string unitEmissionCode, int packNumber)
        {
            return await GetAsyncAuthenticatedData<GetBillOfLadingPackIdResponseModel>(
                $"{ApiController}/bill-of-lading-pack-id/{billOfLadingNumber}/{billOfLadingDigit}/{unitEmissionCode}/{packNumber}");
        }

        public async Task<HttpResponseMessage> SendBillOfLadingPack(SendBillOfLadingPackModel model)
        {
            return await PostAsync<SendBillOfLadingPackModel>(
                $"{ApiController}/send-bill-of-lading-pack", model);
        }

        public async Task<HttpResponseMessage> SendListBillOfLadingPack(SendListBillOfLadingPackModel model)
        {
            return await PostAsync<SendListBillOfLadingPackModel>(
                $"{ApiController}/send-list-bill-of-lading-pack", model);
        }

        public async Task<HttpRequestResult<GetBillOfLadingInformationsByBarCodeResponseModel>> GetBillOfLadingInformationsByBarCode(string barCode)
        {
            return await GetAsyncAuthenticatedData<GetBillOfLadingInformationsByBarCodeResponseModel>(
                $"{ApiController}/bill-of-lading-informations-by-bar-code/{barCode}");
        }
    }
}