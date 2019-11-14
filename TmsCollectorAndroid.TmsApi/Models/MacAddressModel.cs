namespace TmsCollectorAndroid.TmsApi.Models
{
    public class MacAddressModel
    {
        public string MacAddress { get; private set; }

        public MacAddressModel(string macAddress)
        {
            MacAddress = macAddress;
        }

        public override string ToString()
        {
            return MacAddress.Replace(":", "");
        }
    }
}