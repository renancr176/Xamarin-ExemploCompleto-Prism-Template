namespace TmsCollectorAndroid.Api_Old.Models
{
    public class ApiTokenModel
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public EmployeeViewInfoModel EmployeeViewInfo { get; set; }

        public DateTime CreatedAt { get; set; }

        public ApiTokenModel()
        {
            CreatedAt = DateTime.Now;
        }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(access_token) && DateTime.Now < CreatedAt.AddSeconds(expires_in));
        }
    }
}