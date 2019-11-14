using TmsCollectorAndroid.Models.TmsApiExtendedModels;

namespace TmsCollectorAndroid.Extensions
{
    public static class TmsApiModelsExtensions
    {
        public static PackingListDetailViewInfoModel ToDerivedClass(
            this TmsApi.Models.Responses.PackingListDetailViewInfoModel model)
        {
            return new PackingListDetailViewInfoModel(model.Id, model.PackingListId, model.Confirmed, model.Amount,
                model.BillOfLadingId, model.BillOfLadingNumber, model.BillOfLadingDigit,
                model.BillOfLadingUnitEmissionId, model.BillOfLadingUnitEmissionCode, model.Packs, model.LockId,
                model.WarehousePasswordId);
        }

        public static PackingListPackViewInfoModel ToDerivedClass(
            this TmsApi.Models.Responses.PackingListPackViewInfoModel model, int amount)
        {
            return new PackingListPackViewInfoModel(model.Id, model.Confirmed, model.PackingListId,
                model.PackingListDetailUnitSendId, model.BillOfLadingPackId, model.BillOfLadingPackNumber, model.Status,
                model.TypeLink, model.BOLId, model.BOLNumber, model.BOLDigit, model.BOLUnit, amount);
        }
    }
}