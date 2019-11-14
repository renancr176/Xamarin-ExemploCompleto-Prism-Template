using Flurl.Http;
using Newtonsoft.Json;
using TmsCollectorAndroid.Api_Old.Models;

namespace TmsCollectorAndroid.Api_Old.Extensions
{
    public static class HttpResponseMessageExtension
    {
        public static async Task<HttpRequestResult<TObject>> HttpRequestResult<TObject>(this Task<HttpResponseMessage> responseMessage)
        {
            using (HttpResponseMessage result = await responseMessage.ConfigureAwait(false))
            {
                if (result == null)
                    return default(HttpRequestResult<TObject>);
                HttpCall call = result.RequestMessage.GetHttpCall();

                var jsonStr = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                try
                {
                    var response = JsonConvert.DeserializeObject<TObject>(jsonStr);
                    var error = JsonConvert.DeserializeObject<ErrorModel>(jsonStr);

                    return new HttpRequestResult<TObject>(result, response, error);
                }
                catch (Exception ex)
                {
                    call.Exception = (Exception)new FlurlParsingException(call, "JSON", await result.Content.ReadAsStringAsync(), ex);
                    HttpResponseMessage httpResponseMessage = await HandleExceptionAsync(call, call.Exception, CancellationToken.None).ConfigureAwait(false);
                    return default(HttpRequestResult<TObject>);
                }
                call = (HttpCall)null;
            }
            HttpRequestResult<TObject> obj = new HttpRequestResult<TObject>(HttpStatusCode.BadRequest);
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