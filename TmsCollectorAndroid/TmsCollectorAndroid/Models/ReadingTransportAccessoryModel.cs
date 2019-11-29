namespace TmsCollectorAndroid.Models
{
    public class ReadingTransportAccessoryModel
    {
        public string TeamCode { get; private set; }

        public int PackingListId { get; private set; }

        public int TrafficScheduleDetailId { get; private set; }

        public int UnitLocal { get; private set; }

        public int UnitDestinationId { get; private set; }

        public string TextBoxBarCode { get; private set; }

        public int TeamId { get; private set; }

        public string PackAmount { get; private set; }

        public string PackAmountReading { get; private set; }

        public bool IsJointUnit { get; private set; }

        public bool RequiresSeal { get; private set; }

        public ReadingTransportAccessoryModel(string teamCode, int packingListId, int trafficScheduleDetailId,
            int unitLocal, int unitDestinationId, string textBoxBarCode, int teamId, string packAmount,
            string packAmountReading, bool isJointUnit, bool requiresSeal)
        {
            TeamCode = teamCode;
            PackingListId = packingListId;
            TrafficScheduleDetailId = trafficScheduleDetailId;
            UnitLocal = unitLocal;
            UnitDestinationId = unitDestinationId;
            TextBoxBarCode = textBoxBarCode;
            TeamId = teamId;
            PackAmount = packAmount;
            PackAmountReading = packAmountReading;
            IsJointUnit = isJointUnit;
            RequiresSeal = requiresSeal;
        }
    }
}