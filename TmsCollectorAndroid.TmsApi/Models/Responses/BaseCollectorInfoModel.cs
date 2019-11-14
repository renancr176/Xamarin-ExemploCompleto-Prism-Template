using Newtonsoft.Json;
using TmsCollectorAndroid.TmsApi.Converters;
using TmsCollectorAndroid.TmsApi.Enums;

namespace TmsCollectorAndroid.TmsApi.Models.Responses
{
    public abstract class BaseCollectorInfoModel
    {
        public BaseCollectorInfoModel()
        {
            Valid = true;
            ExceptionCode = ExceptionCodeEnum.NoError;
            ExceptionMessage = string.Empty;
            InformationMessage = string.Empty;
        }

        public bool Valid { get; set; }
        [JsonConverter(typeof(StringToEnumConverter<ExceptionCodeEnum>))]
        public ExceptionCodeEnum ExceptionCode { get; set; }
        public string ExceptionMessage { get; set; }
        public string InformationMessage { get; set; }
    }
}