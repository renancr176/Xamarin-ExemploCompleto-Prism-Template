namespace TmsCollectorAndroid.TmsApi.Models.Responses
{
    public class PackingListViewInfoModel : BaseCollectorInfoModel
    {
        public int Id { get; set; }
        public string CobolNumber { get; set; }
        public int Number { get; set; }
        public int Digit { get; set; }
        public int TrafficScheduleDetailId { get; set; }
        public int TrafficScheduleDetailVersionId { get; set; }
        public int TrafficScheduleDetailSequence { get; set; }
        public string OperationalControl { get; set; }
        public int OperationalControlId { get; set; }
        public int UnitLocalId { get; set; }
        public int UnitSendId { get; set; }
        public int TransportAccessoryId { get; set; }
        public int TransportAccessoryDoors { get; set; }
        public int TotalPack { get; set; }
        public int TotalBillOfLading { get; set; }
        public int PackingListId { get; set; }
        public int PackAmount { get; set; }
        public int PackAmountReading { get; set; }
        public int TransportAccessoriesAmount { get; set; }
        public int BillOfLadingId { get; set; }
        public string TrafficScheduleDetailCheckoutDate { get; set; }
        public decimal WeightPackingList { get; set; }
        public decimal WeightCapacityVehicle { get; set; }
        public string CreateUser { get; set; }

        public string NumberDigit => $"{Number}-{Digit}";
    }
}