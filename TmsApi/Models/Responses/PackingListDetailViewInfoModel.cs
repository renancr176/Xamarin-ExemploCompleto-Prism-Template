namespace TmsCollectorAndroid.Api_Old.Models.Responses
{
    public class PackingListDetailViewInfoModel : BaseCollectorInfoModel
    {
        public PackingListDetailViewInfoModel()
        {
            Packs = new List<PackingListPackViewInfoModel>();
        }

        public int Id { get; set; }
        public int PackingListId { get; set; }
        public bool Confirmed { get; set; }

        public int Amount { get; set; }

        public int BillOfLadingId { get; set; }
        public int BillOfLadingNumber { get; set; }
        public string BillOfLadingDigit { get; set; }
        public int BillOfLadingUnitEmissionId { get; set; }
        public string BillOfLadingUnitEmissionCode { get; set; }

        public List<PackingListPackViewInfoModel> Packs { get; set; }

        public int LockId { get; set; }
        public int WarehousePasswordId { get; set; }
    }
}