namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class GetListPackingListAccessoryByPalletModel
    {
        public string UnitLocalCode { get; private set; }

        public int UnitLocalId { get; private set; }

        public int UnitSendId { get; private set; }

        public int TransportAccessoryId { get; private set; }

        public string MacAddress { get; private set; }

        public GetListPackingListAccessoryByPalletModel(string unitLocalCode, int unitLocalId, int unitSendId,
            int transportAccessoryId, string macAddress)
        {
            UnitLocalCode = unitLocalCode;
            UnitLocalId = unitLocalId;
            UnitSendId = unitSendId;
            TransportAccessoryId = transportAccessoryId;
            MacAddress = macAddress;
        }
    }
}