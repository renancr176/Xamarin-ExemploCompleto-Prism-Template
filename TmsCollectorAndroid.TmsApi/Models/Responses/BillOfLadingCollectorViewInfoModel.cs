using System.Collections.Generic;
using TmsCollectorAndroid.TmsApi.Enums;

namespace TmsCollectorAndroid.TmsApi.Models.Responses
{
    public class BillOfLadingCollectorViewInfoModel
    {
        public int Id { get; set; }

        public int InternalId { get; set; }

        public int Number { get; set; }

        public string Digit { get; set; }

        public int UnitEmissionId { get; set; }

        public string UnitEmissionCode { get; set; }

        public int PackAmount { get; set; }

        public IEnumerable<BillOfLadingPackModel> BillOfLadingPacks { get; set; }

        public EBillOfLadingTypeEnum EBillOfLadingType { get; set; }
    }
}