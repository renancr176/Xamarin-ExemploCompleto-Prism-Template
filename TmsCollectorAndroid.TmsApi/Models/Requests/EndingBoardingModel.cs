namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class EndingBoardingModel
    {
        public int PackingListId { get; private set; }
        public int OperationalControlId { get; private set; }
        public string MacAddress { get; private set; }
        public bool ConfirmLanding { get; private set; }

        public EndingBoardingModel(int packingListId, int operationalControlId, string macAddress, bool confirmLanding)
        {
            PackingListId = packingListId;
            OperationalControlId = operationalControlId;
            MacAddress = macAddress;
            ConfirmLanding = confirmLanding;
        }
    }
}