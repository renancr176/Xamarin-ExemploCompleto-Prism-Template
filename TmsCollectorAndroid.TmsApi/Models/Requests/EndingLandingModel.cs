namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class EndingLandingModel
    {
        public int PackingListId { get; private set; }

        public int UnitSendId { get; private set; }

        public int OperationalControlId { get; private set; }

        public int UnitLocalId { get; private set; }

        public EndingLandingModel(int packingListId, int unitSendId, int operationalControlId, int unitLocalId)
        {
            PackingListId = packingListId;
            UnitSendId = unitSendId;
            OperationalControlId = operationalControlId;
            UnitLocalId = unitLocalId;
        }
    }
}