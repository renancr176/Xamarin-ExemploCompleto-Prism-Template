namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class ReadingTransportAccessoryLandingModel
    {
        public int TrafficScheduleDetailId { get; private set; }

        public int TeamId { get; private set; }

        public int PackingListId { get; private set; }

        public int UnitLanding { get; private set; }

        public string PackingListTransportAccessoryCode { get; private set; }

        public string MacAddress { get; private set; }

        public bool IgnoreLoaded { get; private set; }

        public bool RequiresSeals { get; private set; }

        public ReadingTransportAccessoryLandingModel(int trafficScheduleDetailId, int teamId, int packingListId,
            int unitLanding, string packingListTransportAccessoryCode, string macAddress, bool ignoreLoaded,
            bool requiresSeals)
        {
            TrafficScheduleDetailId = trafficScheduleDetailId;
            TeamId = teamId;
            PackingListId = packingListId;
            UnitLanding = unitLanding;
            PackingListTransportAccessoryCode = packingListTransportAccessoryCode;
            MacAddress = macAddress;
            IgnoreLoaded = ignoreLoaded;
            RequiresSeals = requiresSeals;
        }
    }
}