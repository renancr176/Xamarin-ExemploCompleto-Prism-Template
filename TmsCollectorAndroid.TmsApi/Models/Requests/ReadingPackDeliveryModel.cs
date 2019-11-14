namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class ReadingPackDeliveryModel
    {
        public int PackingListDeliveryId { get; private set; }

        public int TrafficScheduleDetailId { get; private set; }

        public int UnitLocal { get; private set; }

        public string BarCode { get; private set; }

        public string MacAddress { get; private set; }

        public bool IgnoreWarehouse { get; private set; }

        public ReadingPackDeliveryModel(int packingListDeliveryId, int trafficScheduleDetailId, int unitLocal,
            string barCode, string macAddress, bool ignoreWarehouse)
        {
            PackingListDeliveryId = packingListDeliveryId;
            TrafficScheduleDetailId = trafficScheduleDetailId;
            UnitLocal = unitLocal;
            BarCode = barCode;
            MacAddress = macAddress;
            IgnoreWarehouse = ignoreWarehouse;
        }
    }
}