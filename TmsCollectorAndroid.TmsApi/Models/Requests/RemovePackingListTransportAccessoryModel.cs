namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class RemovePackingListTransportAccessoryModel
    {
        public int PackingListId { get; private set; }
        public string CobolNumber { get; private set; }
        public int UnitSendId { get; private set; }
        public string MacAddress { get; private set; }

        public RemovePackingListTransportAccessoryModel(int packingListId, string cobolNumber, int unitSendId, string macAddress)
        {
            PackingListId = packingListId;
            CobolNumber = cobolNumber;
            UnitSendId = unitSendId;
            MacAddress = macAddress;
        }
    }
}