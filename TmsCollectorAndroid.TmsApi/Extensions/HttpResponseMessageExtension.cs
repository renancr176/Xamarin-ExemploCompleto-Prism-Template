using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TmsCollectorAndroid.TmsApi.Models;

namespace TmsCollectorAndroid.TmsApi.Extensions
{
    public static class HttpResponseMessageExtension
    {
        public static async Task<HttpRequestResult<TObject>> HttpRequestResult<TObject>(this Task<HttpResponseMessage> responseMessage)
        {
            HttpRequestResult<TObject> obj;

            try
            {
                using (HttpResponseMessage result = await responseMessage.ConfigureAwait(false))
                {
                    if (result == null)
                    {
                        var defaultTObject =  default(TObject);
                        return new HttpRequestResult<TObject>(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), defaultTObject, new ErrorModel());
                    }
                    
                    var jsonStr = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                    try
                    {
                        var jToken = JsonConvert.DeserializeObject<JToken>(jsonStr);
                        var response = JsonConvert.DeserializeObject<TObject>(jsonStr);
                        var error = (((jToken.Type == JTokenType.Object)) ? JsonConvert.DeserializeObject<ErrorModel>(jsonStr):null);

                        if (!result.IsSuccessStatusCode 
                        && (error == null || (string.IsNullOrEmpty(error.Error) && string.IsNullOrWhiteSpace(error.ErrorDescription))))
                        {
                            if (jToken.Type == JTokenType.Object
                            && ((JObject)jToken).ContainsKey("message"))
                            {
                                error = new ErrorModel()
                                {
                                    Error = "Undefined",
                                    ErrorDescription = ((JObject)jToken).GetValue("message")?.Value<string>()
                                };
                            }
                        }

                        return new HttpRequestResult<TObject>(result, response, error);
                    }
                    catch (Exception ex)
                    {
                        var defaultTObject = default(TObject);
                        return new HttpRequestResult<TObject>(result, defaultTObject, new ErrorModel(){Error = ex.Source, ErrorDescription = ex.Message});
                    }
                }
            }
            catch (Exception e)
            {
                obj = new HttpRequestResult<TObject>(HttpStatusCode.InternalServerError);
            }

            return obj;
        }

        internal static HttpCall GetHttpCall(this HttpRequestMessage request)
        {
            object obj;
            if (request?.Properties != null && request.Properties.TryGetValue("FlurlHttpCall", out obj))
            {
                HttpCall httpCall = obj as HttpCall;
                if (httpCall != null)
                    return httpCall;
            }
            return (HttpCall)null;
        }

        internal static async Task<HttpResponseMessage> HandleExceptionAsync(
            HttpCall call,
            Exception ex,
            CancellationToken token)
        {
            call.Exception = ex;
            await HandleEventAsync(call.FlurlRequest.Settings.OnError, call.FlurlRequest.Settings.OnErrorAsync, call).ConfigureAwait(false);
            if (call.ExceptionHandled)
                return call.Response;
            if (ex is OperationCanceledException && !token.IsCancellationRequested)
                throw new FlurlHttpTimeoutException(call, ex);
            if (ex is FlurlHttpException)
                throw ex;
            throw new FlurlHttpException(call, ex);
        }

        private static Task HandleEventAsync(
            Action<HttpCall> syncHandler,
            Func<HttpCall, Task> asyncHandler,
            HttpCall call)
        {
            if (syncHandler != null)
                syncHandler(call);
            if (asyncHandler != null)
                return asyncHandler(call);
            return (Task)Task.FromResult<int>(0);
        }
    }
}