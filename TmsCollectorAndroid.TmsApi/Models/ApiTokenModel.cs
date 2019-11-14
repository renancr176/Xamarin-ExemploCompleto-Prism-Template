using System;
using Newtonsoft.Json;

namespace TmsCollectorAndroid.TmsApi.Models
{
    public class ApiTokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public EmployeeViewInfoModel EmployeeViewInfo { get; set; }

        public DateTime CreatedAt { get; set; }

        public ApiTokenModel()
        {
            CreatedAt = DateTime.Now;
        }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(AccessToken) && DateTime.Now < CreatedAt.AddSeconds(ExpiresIn));
        }
    }
}