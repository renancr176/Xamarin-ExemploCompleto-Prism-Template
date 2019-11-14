using Honeywell.AIDC.CrossPlatform;

namespace TmsCollectorAndroid.Interfaces.Services
{
    public interface IBarcodeReaderService : IBarcodeReader
    {
        IBarcodeReader BarcodeReader { get; }
        bool IsReaderEnabled { get; }
    }
}