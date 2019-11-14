namespace TmsCollectorAndroid.Api_Old.Models.Responses
{
    public class TeamViewInfoModel : BaseCollectorInfoModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public int UnitId { get; set; }
    }
}