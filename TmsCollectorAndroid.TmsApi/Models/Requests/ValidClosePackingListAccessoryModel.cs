using System.Collections.Generic;

namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class ValidClosePackingListAccessoryModel
    {
        public int PackingListAccessoryId { get; private set; }

        public bool IgnoreAmountSealsSuperior { get; private set; }

        public IEnumerable<int> IgnoreBillOfLadings { get; private set; }

        public string MacAddress { get; private set; }

        public ValidClosePackingListAccessoryModel(int packingListAccessoryId, bool ignoreAmountSealsSuperior,
            List<int> ignoreBillOfLadings, string macAddress)
        {
            PackingListAccessoryId = packingListAccessoryId;
            IgnoreAmountSealsSuperior = ignoreAmountSealsSuperior;
            IgnoreBillOfLadings = ignoreBillOfLadings;
            MacAddress = macAddress;
        }
    }
}