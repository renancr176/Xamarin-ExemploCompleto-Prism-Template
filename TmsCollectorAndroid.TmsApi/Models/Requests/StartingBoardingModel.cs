namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class StartingBoardingModel
    {
        public int OperationalControlId { get; private set; }
        public int UnitId { get; private set; }
        public int TrafficScheduleDetailId { get; private set; }
        public string TeamCode { get; private set; }
        public bool IsJointUnit { get; private set; }

        public StartingBoardingModel(int operationalControlId, int unitId, int trafficScheduleDetailId, string teamCode, bool isJointUnit)
        {
            OperationalControlId = operationalControlId;
            UnitId = unitId;
            TrafficScheduleDetailId = trafficScheduleDetailId;
            TeamCode = teamCode;
            IsJointUnit = isJointUnit;
        }
    }
}