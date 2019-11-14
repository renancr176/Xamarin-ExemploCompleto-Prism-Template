namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class CancelPackBoardingModel
    {
        public int PackingListId { get; private set; }
        public string BarCode { get; private set; }
        public string MacAddress { get; private set; }
        public bool IsJointUnit { get; private set; }
        public bool IsDelivery { get; private set; }

        public CancelPackBoardingModel(int packingListId, string barCode, string macAddress, bool isJointUnit,
            bool isDelivery)
        {
            PackingListId = packingListId;
            BarCode = barCode;
            MacAddress = macAddress;
            IsJointUnit = isJointUnit;
            IsDelivery = isDelivery;
        }
    }
}