namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class ReadingPackBoardingModel
    {
        public bool IgnoreWarehouse { get; private set; }
        public int TrafficScheduleDetailId { get; private set; }
        public int TeamId { get; private set; }
        public int TrafficScheduleDetailVersionId { get; private set; }
        public int TrafficScheduleDetailSequence { get; private set; }
        public int PackingListId { get; private set; }
        public int UnitLocal { get; private set; }
        public int UnitDestination { get; private set; }
        public string BarCode { get; private set; }
        public string MacAddress { get; private set; }
        public bool IsJointUnit { get; private set; }
        public bool CancelIncompleteBoarding { get; private set; }
        public int WarehousePasswordId { get; private set; }
        public bool ValidCTeUnitizado { get; private set; }

        public ReadingPackBoardingModel(bool ignoreWarehouse, int trafficScheduleDetailId, int teamId,
            int trafficScheduleDetailVersionId, int trafficScheduleDetailSequence, int packingListId, int unitLocal,
            int unitDestination, string barCode, string macAddress, bool isJointUnit, bool cancelIncompleteBoarding,
            int warehousePasswordId, bool validCTeUnitizado)
        {
            IgnoreWarehouse = ignoreWarehouse;
            TrafficScheduleDetailId = trafficScheduleDetailId;
            TeamId = teamId;
            TrafficScheduleDetailVersionId = trafficScheduleDetailVersionId;
            TrafficScheduleDetailSequence = trafficScheduleDetailSequence;
            PackingListId = packingListId;
            UnitLocal = unitLocal;
            UnitDestination = unitDestination;
            BarCode = barCode;
            MacAddress = macAddress;
            IsJointUnit = isJointUnit;
            CancelIncompleteBoarding = cancelIncompleteBoarding;
            WarehousePasswordId = warehousePasswordId;
            ValidCTeUnitizado = validCTeUnitizado;
        }
    }
}