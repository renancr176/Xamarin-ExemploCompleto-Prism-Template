namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class ReadingPackingListTransportAccessoryModel
    {
        public int PackingListId { get; private set; }
        public int TrafficScheduleDetailId { get; private set; }
        public string CobolNumber { get; private set; }
        public int TeamId { get; private set; }
        public int UnitLocalId { get; private set; }
        public int UnitSendId { get; private set; }
        public string MacAddress { get; private set; }
        public bool RequiresSeals { get; private set; }

        public ReadingPackingListTransportAccessoryModel(int packingListId, int trafficScheduleDetailId,
            string cobolNumber, int teamId, int unitLocalId, int unitSendId, string macAddress, bool requiresSeals)
        {
            PackingListId = packingListId;
            TrafficScheduleDetailId = trafficScheduleDetailId;
            CobolNumber = cobolNumber;
            TeamId = teamId;
            UnitLocalId = unitLocalId;
            UnitSendId = unitSendId;
            MacAddress = macAddress;
            RequiresSeals = requiresSeals;
        }
    }
}