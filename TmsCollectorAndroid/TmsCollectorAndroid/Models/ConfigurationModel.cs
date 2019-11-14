using System.Collections.Generic;
using TmsCollectorAndroid.Interfaces.Models;

namespace TmsCollectorAndroid.Models
{
    public class ConfigurationModel : IConfigurationModel
    {
        public IEnumerable<string> ApiUrls { get; private set; }

        public string CollectorVersion { get; private set; }

        public ConfigurationModel(List<string> apiUrls, string collectorVersion)
        {
            ApiUrls = apiUrls;
            CollectorVersion = collectorVersion;
        }
    }
}