namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class AddSealModel
    {
        public int PackingListAccessoryId { get; private set; }

        public string Seal { get; private set; }

        public string MacAddress { get; private set; }

        public AddSealModel(int packingListAccessoryId, string seal, string macAddress)
        {
            PackingListAccessoryId = packingListAccessoryId;
            Seal = seal;
            MacAddress = macAddress;
        }
    }
}