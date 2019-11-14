namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class CancelPackingListTransportAccessoryLandingModel
    {
        public int PackingListId { get; private set; }

        public int TrafficScheduleDetailId { get; private set; }

        public string PackingListTransportAccessoryCode { get; private set; }

        public int UnitLanding { get; private set; }

        public string MacAddress { get; private set; }

        public CancelPackingListTransportAccessoryLandingModel(int packingListId, int trafficScheduleDetailId,
            string packingListTransportAccessoryCode, int unitLanding, string macAddress)
        {
            PackingListId = packingListId;
            TrafficScheduleDetailId = trafficScheduleDetailId;
            PackingListTransportAccessoryCode = packingListTransportAccessoryCode;
            UnitLanding = unitLanding;
            MacAddress = macAddress;
        }
    }
}