namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class StartingLandingModel
    {
        public int OperationalControlId { get; private set; }

        public int UnitId { get; private set; }

        public int TrafficScheduleDetailId { get; private set; }

        public string TeamCode { get; private set; }

        public StartingLandingModel(int operationalControlId, int unitId, int trafficScheduleDetailId, string teamCode)
        {
            OperationalControlId = operationalControlId;
            UnitId = unitId;
            TrafficScheduleDetailId = trafficScheduleDetailId;
            TeamCode = teamCode;
        }
    }
}