using System.Net;
using System.Net.Http;

namespace TmsCollectorAndroid.TmsApi.Models
{
    public class HttpRequestResult<TObject> : HttpResponseMessage
    {
        public TObject Response { get; private set; }
        public ErrorModel Error { get; private set; }

        public HttpRequestResult(HttpResponseMessage httpResponseMessage, TObject response, ErrorModel error)
        {
            Version = httpResponseMessage.Version;
            Content = httpResponseMessage.Content;
            StatusCode = httpResponseMessage.StatusCode;
            ReasonPhrase = httpResponseMessage.ReasonPhrase;
            RequestMessage = httpResponseMessage.RequestMessage;

            Response = response;
            Error = error;
        }

        public HttpRequestResult(HttpStatusCode status)
            : base(status)
        {
        }
    }
}