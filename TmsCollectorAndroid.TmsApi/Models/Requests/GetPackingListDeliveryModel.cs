namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class GetPackingListDeliveryModel
    {
        public int UnitId { get; private set; }

        public string CheckOutDate { get; private set; }

        public int LineId { get; private set; }

        public int VehicleId { get; private set; }

        public int DriverId { get; private set; }

        public string MacAddress { get; private set; }

        public bool LoadPackingList { get; private set; }

        public GetPackingListDeliveryModel(int unitId, string checkOutDate, int lineId, int vehicleId, int driverId,
            string macAddress, bool loadPackingList)
        {
            UnitId = unitId;
            CheckOutDate = checkOutDate;
            LineId = lineId;
            VehicleId = vehicleId;
            DriverId = driverId;
            MacAddress = macAddress;
            LoadPackingList = loadPackingList;
        }
    }
}