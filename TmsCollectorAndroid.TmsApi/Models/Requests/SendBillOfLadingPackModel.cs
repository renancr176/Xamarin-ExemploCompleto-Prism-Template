namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class SendBillOfLadingPackModel
    {
        public string MacAddress { get; private set; }
        public int BillOfLadingPackId { get; private set; }

        public SendBillOfLadingPackModel(string macAddress, int billOfLadingPackId)
        {
            MacAddress = macAddress;
            BillOfLadingPackId = billOfLadingPackId;
        }
    }
}