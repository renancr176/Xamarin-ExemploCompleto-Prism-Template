using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Interfaces.Services
{
    public interface ISorterService
    {
        Task<HttpResponseMessage> EnableUniversalLanding(int unitId);
        Task<HttpRequestResult<PackingListDetailViewInfoModel>> ReadingByUniversalLanding(
            ReadingByUniversalLandingModel model);
        Task<HttpRequestResult<PackingListDetailViewInfoModel>> ReadingTransportAccessoryByUniversalLanding(
            ReadingTransportAccessoryByUniversalLandingModel model);
        Task<HttpRequestResult<PackingListDetailViewInfoModel>> GetViewLackByUniversalLanding(int billOfLadingNumber,
            string billOfLadingDigit, string unitEmissionCode, int unitLocalId);
        Task<HttpRequestResult<IEnumerable<PackingListDetailViewInfoModel>>>
            GetViewLandedByUniversalLanding(int unitId);
    }
}