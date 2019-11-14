using System;
using TmsCollectorAndroid.TmsApi.Enums;

namespace TmsCollectorAndroid.TmsApi.Models.Responses
{
    public class VehicleViewInfoModel : BaseCollectorInfoModel
    {
        public int Id { get; set; }
        public string CarNumber { get; set; }
        public string VehicleType { get; set; }
        public string Plate { get; set; }
        public string VehicleView { get; set; }

        public int PackingListId { get; set; }
        public int PackAmount { get; set; }
        public int PackAmountReading { get; set; }

        public int TrafficScheduleId { get; set; }
        public string TrafficScheduleDate { get; set; }
        public string TrafficScheduleTime { get; set; }
        public DateTime TrafficScheduleDateTime { get; set; }
        public string TrafficScheduleUnitLocalCode { get; set; }
        public TrafficScheduleTypeEnum TrafficScheduleType { get; set; }

        public int TrafficScheduleDetailId { get; set; }
        public int TrafficScheduleDetailVersionId { get; set; }
        public int TrafficScheduleDetailSequence { get; set; }
        public string TrafficScheduleDetailStatus { get; set; }
        public string LineCode { get; set; }
        public DateTime CheckInDateView { get; set; }
        public string OperationalControl { get; set; }
        public string OperationalControlDescription { get; set; }
        public int OperationalControlId { get; set; }

        public int UnitCurrentId { get; set; }
        public string UnitCurrentCode { get; set; }
        public string UnitCurrentDescription { get; set; }

        public int UnitSendId { get; set; }
        public string UnitSendCode { get; set; }
        public string UnitSendDescription { get; set; }

        public string UserLogin { get; set; }
        public DateTime PackingListDate { get; set; }
    }
}