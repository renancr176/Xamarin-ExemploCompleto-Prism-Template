namespace TmsCollectorAndroid.Api_Old.Models.Responses
{
    public abstract class BaseCollectorInfoModel
    {
        public BaseCollectorInfoModel()
        {
            Valid = true;
            ExceptionCode = string.Empty;
            ExceptionMessage = string.Empty;
            InformationMessage = string.Empty;
        }

        public bool Valid { get; set; }
        public string ExceptionCode { get; set; }
        public string ExceptionMessage { get; set; }
        public string InformationMessage { get; set; }
    }
}