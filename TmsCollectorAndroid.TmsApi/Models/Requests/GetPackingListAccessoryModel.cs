namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class GetPackingListAccessoryModel
    {
        public string UnitLocalCode { get; private set; }

        public int UnitLocalId { get; private set; }

        public string UnitSendCode { get; private set; }

        public int UnitSendId { get; private set; }

        public int TransportAccessoryCobolCode { get; private set; }

        public int TransportAccessoryId { get; private set; }

        public string MacAddress { get; private set; }

        public bool ForceNew { get; private set; }

        public GetPackingListAccessoryModel(string unitLocalCode, int unitLocalId, string unitSendCode, int unitSendId,
            int transportAccessoryCobolCode, int transportAccessoryId, string macAddress, bool forceNew)
        {
            UnitLocalCode = unitLocalCode;
            UnitLocalId = unitLocalId;
            UnitSendCode = unitSendCode;
            UnitSendId = unitSendId;
            TransportAccessoryCobolCode = transportAccessoryCobolCode;
            TransportAccessoryId = transportAccessoryId;
            MacAddress = macAddress;
            ForceNew = forceNew;
        }
    }
}