using System.Net.Http;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Interfaces.Services
{
    public interface IBillOfLadingService
    {
        Task<HttpRequestResult<BillOfLadingCollectorViewInfoModel>> GetBillOfLading(int cobolNumber, string cobolDigit,
            string unitEmissionCode);
        Task<HttpRequestResult<GetBillOfLadingPackIdResponseModel>> GetBillOfLadingPackId(int billOfLadingNumber,
            string billOfLadingDigit, string unitEmissionCode, int packNumber);
        Task<HttpResponseMessage> SendBillOfLadingPack(SendBillOfLadingPackModel model);
        Task<HttpResponseMessage> SendListBillOfLadingPack(SendListBillOfLadingPackModel model);
        Task<HttpRequestResult<GetBillOfLadingInformationsByBarCodeResponseModel>> GetBillOfLadingInformationsByBarCode(
            string barCode);
    }
}