using System;

namespace TmsCollectorAndroid.TmsApi.Models.Responses
{
    public class LineInfoViewModel : BaseCollectorInfoModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Descritpion { get; set; }
        public bool Active { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}