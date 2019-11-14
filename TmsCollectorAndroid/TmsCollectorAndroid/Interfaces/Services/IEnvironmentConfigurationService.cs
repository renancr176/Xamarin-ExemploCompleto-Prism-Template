using TmsCollectorAndroid.Interfaces.Models;

namespace TmsCollectorAndroid.Interfaces.Services
{
    public interface IEnvironmentConfigurationService
    {
        IConfigurationModel Configuration { get; }
    }
}