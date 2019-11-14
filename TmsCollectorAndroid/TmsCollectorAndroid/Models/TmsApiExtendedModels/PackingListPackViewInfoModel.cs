using TmsCollectorAndroid.TmsApi.Enums;

namespace TmsCollectorAndroid.Models.TmsApiExtendedModels
{
    public class PackingListPackViewInfoModel : TmsApi.Models.Responses.PackingListPackViewInfoModel
    {
        public PackingListPackViewInfoModel(int id, bool confirmed, int packingListId, int packingListDetailUnitSendId,
            int billOfLadingPackId, int billOfLadingPackNumber, PackOperationEnum status, TypeLinkEnum typeLink,
            int bolId, int bolNumber, string bolDigit, string bolUnit, int amount)
        {
            Id = id;
            Confirmed = confirmed;
            PackingListId = packingListId;
            PackingListDetailUnitSendId = packingListDetailUnitSendId;
            BillOfLadingPackId = billOfLadingPackId;
            BillOfLadingPackNumber = billOfLadingPackNumber;
            Status = status;
            TypeLink = typeLink;
            BOLId = bolId;
            BOLNumber = bolNumber;
            BOLDigit = bolDigit;
            BOLUnit = bolUnit;
            Amount = amount;
        }

        public int Amount { get; set; }

        public string TreeDescription => $"Volume: {BillOfLadingPackNumber}{((Amount > 0) ? $"/{Amount}":"")}";
    }
}