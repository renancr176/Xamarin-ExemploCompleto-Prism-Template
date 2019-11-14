namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class RemoveSealModel
    {
        public int PackingListAccessoryId { get; private set; }

        public string Seal { get; private set; }

        public string MacAddress { get; private set; }

        public RemoveSealModel(int packingListAccessoryId, string seal, string macAddress)
        {
            PackingListAccessoryId = packingListAccessoryId;
            Seal = seal;
            MacAddress = macAddress;
        }
    }
}