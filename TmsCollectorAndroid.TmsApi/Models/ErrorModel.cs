using Newtonsoft.Json;

namespace TmsCollectorAndroid.TmsApi.Models
{
    public class ErrorModel
    {
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}