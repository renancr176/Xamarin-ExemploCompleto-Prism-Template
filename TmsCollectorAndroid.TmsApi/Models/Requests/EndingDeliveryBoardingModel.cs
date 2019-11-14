namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class EndingDeliveryBoardingModel
    {
        public int PackingListDeliveryId { get; private set; }

        public string MacAddress { get; private set; }

        public EndingDeliveryBoardingModel(int packingListDeliveryId, string macAddress)
        {
            PackingListDeliveryId = packingListDeliveryId;
            MacAddress = macAddress;
        }
    }
}