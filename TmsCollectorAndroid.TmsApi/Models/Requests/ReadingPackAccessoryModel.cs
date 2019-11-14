namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class ReadingPackAccessoryModel
    {
        public bool IgnoreWarehouse { get; private set; }

        public int PackingListAccessoryId { get; private set; }

        public int UnitLocal { get; private set; }

        public int UnitDestination { get; private set; }

        public string BarCode { get; private set; }

        public string MacAddress { get; private set; }

        public ReadingPackAccessoryModel(bool ignoreWarehouse, int packingListAccessoryId, int unitLocal,
            int unitDestination, string barCode, string macAddress)
        {
            IgnoreWarehouse = ignoreWarehouse;
            PackingListAccessoryId = packingListAccessoryId;
            UnitLocal = unitLocal;
            UnitDestination = unitDestination;
            BarCode = barCode;
            MacAddress = macAddress;
        }
    }
}