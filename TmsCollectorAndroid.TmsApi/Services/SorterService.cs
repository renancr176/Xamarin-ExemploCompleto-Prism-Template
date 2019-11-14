using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.TmsApi.Services
{
    public class SorterService : ServiceBase, ISorterService
    {
        public string ApiController { get; private set; }

        public SorterService()
        {
            ApiController = "ws";
        }

        public async Task<HttpResponseMessage> EnableUniversalLanding(int unitId)
        {
            return await GetAsync($"{ApiController}/enable-universal-landing/{unitId}");
        }

        public async Task<HttpRequestResult<PackingListDetailViewInfoModel>> ReadingByUniversalLanding(
            ReadingByUniversalLandingModel model)
        {
            return await PostAsyncReceiveData<PackingListDetailViewInfoModel, ReadingByUniversalLandingModel>(
                $"{ApiController}/reading-by-universal-landing", model);
        }

        public async Task<HttpRequestResult<PackingListDetailViewInfoModel>>
            ReadingTransportAccessoryByUniversalLanding(ReadingTransportAccessoryByUniversalLandingModel model)
        {
            return await PostAsyncReceiveData<PackingListDetailViewInfoModel,
                ReadingTransportAccessoryByUniversalLandingModel>(
                $"{ApiController}/reading-transport-accessory-by-universal-landing", model);
        }

        public async Task<HttpRequestResult<PackingListDetailViewInfoModel>> GetViewLackByUniversalLanding(
            int billOfLadingNumber, string billOfLadingDigit, string unitEmissionCode,
            int unitLocalId)
        {
            return await GetAsyncAuthenticatedData<PackingListDetailViewInfoModel>(
                $"{ApiController}/view-lack-by-universal-landing/{billOfLadingNumber}/{billOfLadingDigit}/{unitEmissionCode}/{unitLocalId}");
        }

        public async Task<HttpRequestResult<IEnumerable<PackingListDetailViewInfoModel>>>
            GetViewLandedByUniversalLanding(int unitId)
        {
            return await GetAsyncAuthenticatedData<IEnumerable<PackingListDetailViewInfoModel>>(
                $"{ApiController}/view-landed-by-universal-landing/{unitId}");
        }
    }
}