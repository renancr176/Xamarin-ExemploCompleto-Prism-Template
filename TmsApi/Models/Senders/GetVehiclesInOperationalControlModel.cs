namespace TmsCollectorAndroid.Api_Old.Models.Senders
{
    public class GetVehiclesInOperationalControlModel
    {
        public EmployeeViewInfoModel AuthenticateInfo { get; set; }

        public int UnitId { get; set; }

        public int UnitSendId { get; set; }

        public int VehicleId { get; set; }
    }
}