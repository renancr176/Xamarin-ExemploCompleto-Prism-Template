namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class ReadingPackLandingModel
    {
        public int TrafficScheduleDetailId { get; private set; }

        public int TeamId { get; private set; }

        public int PackingListId { get; private set; }

        public int UnitLanding { get; private set; }

        public string BarCode { get; private set; }

        public string MacAddress { get; private set; }

        public bool IgnoreLoaded { get; private set; }

        public ReadingPackLandingModel(int trafficScheduleDetailId, int teamId, int packingListId, int unitLanding,
            string barCode, string macAddress, bool ignoreLoaded)
        {
            TrafficScheduleDetailId = trafficScheduleDetailId;
            TeamId = teamId;
            PackingListId = packingListId;
            UnitLanding = unitLanding;
            BarCode = barCode;
            MacAddress = macAddress;
            IgnoreLoaded = ignoreLoaded;
        }
    }
}