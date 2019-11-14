using System.Collections.Generic;

namespace TmsCollectorAndroid.Interfaces.Models
{
    public interface IConfigurationModel
    {
        IEnumerable<string> ApiUrls { get; }
        string CollectorVersion { get; }
    }
}