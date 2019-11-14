namespace TmsCollectorAndroid.Api_Old.Models.Responses
{
    public class UnitViewInfoModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int CompanyId { get; set; }

        public int JointUnitId { get; set; }
        public string JointUnitCode { get; set; }
        public string JointUnitDescription { get; set; }

        public bool IsUniversalLanding { get; set; }
    }
}