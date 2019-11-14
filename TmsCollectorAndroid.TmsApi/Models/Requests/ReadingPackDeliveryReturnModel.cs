namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class ReadingPackDeliveryReturnModel
    {
        public int PackingListDeliveryId { get; private set; }

        public string BarCode { get; private set; }

        public string MacAddress { get; private set; }

        public ReadingPackDeliveryReturnModel(int packingListDeliveryId, string barCode, string macAddress)
        {
            PackingListDeliveryId = packingListDeliveryId;
            BarCode = barCode;
            MacAddress = macAddress;
        }
    }
}