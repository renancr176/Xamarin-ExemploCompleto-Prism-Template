using System.Collections.Generic;
using System.Threading.Tasks;
using Honeywell.AIDC.CrossPlatform;
using Plugin.CurrentActivity;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.Droid.Service
{
    public class BarcodeReaderService : IBarcodeReaderService
    {
        public IBarcodeReader BarcodeReader { get; private set; }

        private readonly ICurrentActivity _currentActivity;

        public BarcodeReaderService(ICurrentActivity currentActivity)
        {
            _currentActivity = currentActivity;

            BarcodeReader = new BarcodeReader(_currentActivity.AppContext);

            IsReaderEnabled = true;
        }

        public async Task<BarcodeReaderBase.Result> OpenAsync()
        {
            return await BarcodeReader.OpenAsync();
        }

        public async Task<BarcodeReaderBase.Result> CloseAsync()
        {
            return await BarcodeReader.CloseAsync();
        }

        public async Task<BarcodeReaderBase.Result> SetAsync(Dictionary<string, object> settings)
        {
            return await BarcodeReader.SetAsync(settings);
        }

        public async Task<BarcodeReaderBase.Result> SoftwareTriggerAsync(bool on)
        {
            return await BarcodeReader.SoftwareTriggerAsync(on);
        }

        public async Task<BarcodeReaderBase.Result> EnableAsync(bool enabled)
        {
            var result = await BarcodeReader.EnableAsync(enabled);

            if (result.Code == 0)
                IsReaderEnabled = enabled;

            return result;
        }

        public bool IsReaderOpened => BarcodeReader.IsReaderOpened;

        public bool IsReaderEnabled { get; private set; }
    }
}