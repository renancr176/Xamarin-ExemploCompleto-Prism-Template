namespace TmsCollectorAndroid.Interfaces.Services
{
    public interface IWifiService
    {
        string MacAddress { get; }

        bool IsEnabled { get; }

        void Enable();

        void Disable();
    }
}