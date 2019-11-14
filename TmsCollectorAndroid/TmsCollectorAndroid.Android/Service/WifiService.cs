using Android.Content;
using Android.Net.Wifi;
using Plugin.CurrentActivity;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.Droid.Service
{
    public class WifiService : IWifiService
    {
        private readonly ICurrentActivity _currentActivity;

        public WifiManager Manager { get; private set; }

        public WifiService(ICurrentActivity currentActivity)
        {
            _currentActivity = currentActivity;
            Manager = (WifiManager) _currentActivity.Activity.GetSystemService(Context.WifiService);
        }

        public string MacAddress => Manager.ConnectionInfo.MacAddress;

        public bool IsEnabled => Manager.IsWifiEnabled;

        public void Enable()
        {
            Manager.SetWifiEnabled(true);
        }

        public void Disable()
        {
            Manager.SetWifiEnabled(false);
        }
    }
}