namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class CancelBillOfLadingLandingModel
    {
        public int PackingListId { get; private set; }

        public int TrafficScheduleDetailId { get; private set; }

        public int UnitLanding { get; private set; }

        public string BarCode { get; private set; }

        public string MacAddress { get; private set; }

        public CancelBillOfLadingLandingModel(int packingListId, int trafficScheduleDetailId, int unitLanding,
            string barCode, string macAddress)
        {
            PackingListId = packingListId;
            TrafficScheduleDetailId = trafficScheduleDetailId;
            UnitLanding = unitLanding;
            BarCode = barCode;
            MacAddress = macAddress;
        }
    }
}