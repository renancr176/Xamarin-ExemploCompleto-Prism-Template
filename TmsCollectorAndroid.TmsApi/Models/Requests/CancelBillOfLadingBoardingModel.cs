namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class CancelBillOfLadingBoardingModel
    {
        public int PackingListId { get; private set; }
        public string BarCode { get; private set; }
        public string MacAddress { get; private set; }
        public bool IsJointUnit { get; private set; }

        public CancelBillOfLadingBoardingModel(int packingListId, string barCode, string macAddress, bool isJointUnit)
        {
            PackingListId = packingListId;
            BarCode = barCode;
            MacAddress = macAddress;
            IsJointUnit = isJointUnit;
        }
    }
}