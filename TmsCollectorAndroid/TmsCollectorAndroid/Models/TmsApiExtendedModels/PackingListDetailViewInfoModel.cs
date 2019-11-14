using System.Collections.Generic;
using System.Linq;
using TmsCollectorAndroid.Extensions;

namespace TmsCollectorAndroid.Models.TmsApiExtendedModels
{
    public class PackingListDetailViewInfoModel : TmsApi.Models.Responses.PackingListDetailViewInfoModel
    {
        public PackingListDetailViewInfoModel(int id, int packingListId, bool confirmed, int amount, int billOfLadingId,
            int billOfLadingNumber, string billOfLadingDigit, int billOfLadingUnitEmissionId,
            string billOfLadingUnitEmissionCode, IEnumerable<TmsApi.Models.Responses.PackingListPackViewInfoModel> packs, int lockId,
            int warehousePasswordId)
        {
            Id = id;
            PackingListId = packingListId;
            Confirmed = confirmed;
            Amount = amount;
            BillOfLadingId = billOfLadingId;
            BillOfLadingNumber = billOfLadingNumber;
            BillOfLadingDigit = billOfLadingDigit;
            BillOfLadingUnitEmissionId = billOfLadingUnitEmissionId;
            BillOfLadingUnitEmissionCode = billOfLadingUnitEmissionCode;
            Packs = packs;
            LockId = lockId;
            WarehousePasswordId = warehousePasswordId;
        }
        
        private const int RowHeight = 45;

        public int ListViewHeight => (PacksUnConfirmed.Count() * RowHeight);

        public string BillOfLading => $"{BillOfLadingNumber}-{BillOfLadingDigit} {BillOfLadingUnitEmissionCode}";

        public IEnumerable<PackingListPackViewInfoModel> PacksUnConfirmed => Packs.Where(p => !p.Confirmed)
            .Select(p => p.ToDerivedClass(Amount));
    }
}