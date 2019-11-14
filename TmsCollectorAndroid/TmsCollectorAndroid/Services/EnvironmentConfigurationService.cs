using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TmsCollectorAndroid.Interfaces.Models;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;

namespace TmsCollectorAndroid.Services
{
    public class EnvironmentConfigurationService : IEnvironmentConfigurationService
    {
        public IConfigurationModel Configuration { get; private set; }

        public EnvironmentConfigurationService()
        {
            var embeddedResourceStream = Assembly.GetAssembly(typeof(IConfigurationModel))
                .GetManifestResourceStream("TmsCollectorAndroid.Configuration.config.json");

            if (embeddedResourceStream == null) return;

            using (var streamReader = new StreamReader(embeddedResourceStream))
            {
                var jsonString = streamReader.ReadToEnd();
                var jObj = JsonConvert.DeserializeObject<JObject>(jsonString);
                var apiBaseUrl = jObj.GetValue("ApiBaseUrl")?.Value<string>();
                var apiUrls = new List<string>();

                if (!string.IsNullOrEmpty(apiBaseUrl))
                {
                    foreach (var url in apiBaseUrl.Split(';'))
                    {
                        apiUrls.Add(url);
                    }
                }

                Configuration = new ConfigurationModel(
                    apiUrls,
                    jObj.GetValue("CollectorVersion")?.Value<string>() ?? String.Empty
                );
            }
        }
    }
}