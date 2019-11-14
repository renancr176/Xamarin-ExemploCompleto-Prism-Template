namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class LinkPackModel
    {
        public int PackingListAccessoryId { get; private set; }

        public int BillOfLadingId { get; private set; }

        public string MacAddress { get; private set; }

        public LinkPackModel(int packingListAccessoryId, int billOfLadingId, string macAddress)
        {
            PackingListAccessoryId = packingListAccessoryId;
            BillOfLadingId = billOfLadingId;
            MacAddress = macAddress;
        }
    }
}