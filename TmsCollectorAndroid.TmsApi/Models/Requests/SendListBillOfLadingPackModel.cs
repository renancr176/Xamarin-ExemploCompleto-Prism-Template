using System.Collections.Generic;

namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class SendListBillOfLadingPackModel
    {
        public string MacAddress { get; private set; }
        public IEnumerable<string> Packs { get; private set; }

        public SendListBillOfLadingPackModel(string macAddress, IEnumerable<string> packs)
        {
            MacAddress = macAddress;
            Packs = packs;
        }
    }
}