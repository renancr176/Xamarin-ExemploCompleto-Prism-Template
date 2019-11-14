namespace TmsCollectorAndroid.TmsApi.Models.Responses
{
    public class TransportAccessoryViewInfoModel : BaseCollectorInfoModel
    {
        public int Id { get; set; }
        public string PropertyNumber { get; set; }
        public string CobolNumber { get; set; }
        public string Description { get; set; }
        public string AccessoryGroupKind { get; set; }
        public bool Active { get; set; }
        public int UnitInfoWarehouseId { get; set; }
    }
}