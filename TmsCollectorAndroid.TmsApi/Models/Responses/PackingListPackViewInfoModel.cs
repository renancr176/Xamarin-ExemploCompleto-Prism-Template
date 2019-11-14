using TmsCollectorAndroid.TmsApi.Enums;

namespace TmsCollectorAndroid.TmsApi.Models.Responses
{
    public class PackingListPackViewInfoModel
    {
        public int Id { get; set; }
        public bool Confirmed { get; set; }

        public int PackingListId { get; set; }
        public int PackingListDetailUnitSendId { get; set; }

        public int BillOfLadingPackId { get; set; }
        public int BillOfLadingPackNumber { get; set; }

        public PackOperationEnum Status { get; set; }
        public TypeLinkEnum TypeLink { get; set; }

        public int BOLId { get; set; }
        public int BOLNumber { get; set; }
        public string BOLDigit { get; set; }
        public string BOLUnit { get; set; }
    }
}