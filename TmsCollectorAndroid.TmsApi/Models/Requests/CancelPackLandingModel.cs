namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class CancelPackLandingModel
    {
        public int PackingListId { get; private set; }

        public int TrafficScheduleDetailId { get; private set; }

        public string BarCode { get; private set; }

        public int UnitLanding { get; private set; }

        public string MacAddress { get; private set; }

        public CancelPackLandingModel(int packingListId, int trafficScheduleDetailId, string barCode, int unitLanding,
            string macAddress)
        {
            PackingListId = packingListId;
            TrafficScheduleDetailId = trafficScheduleDetailId;
            BarCode = barCode;
            UnitLanding = unitLanding;
            MacAddress = macAddress;
        }
    }
}