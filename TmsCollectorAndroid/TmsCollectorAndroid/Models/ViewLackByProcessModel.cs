using System;
using System.Linq;
using Prism.Mvvm;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Models
{
    public class ViewLackByProcessModel : BindableBase
    {
        private LackTypeEnum _lackType;
        public LackTypeEnum LackType
        {
            get { return _lackType; }
            set { SetProperty(ref _lackType, value); }
        }

        private VehicleViewInfoModel _vehicleViewInfo;
        public VehicleViewInfoModel VehicleViewInfo
        {
            get { return _vehicleViewInfo; }
            set
            {
                SetProperty(ref _vehicleViewInfo, value);
                SetPropertiesFromVehicleViewInfo();
            }
        }

        private int _packingListId;
        public int PackingListId
        {
            get { return _packingListId; }
            set { SetProperty(ref _packingListId, value); }
        }

        private string _vehicle;
        public string Vehicle
        {
            get { return _vehicle; }
            set { SetProperty(ref _vehicle, value); }
        }

        private string _vehicleDescription;
        public string VehicleDescription
        {
            get { return _vehicleDescription; }
            set { SetProperty(ref _vehicleDescription, value); }
        }

        private string _unitDestination;
        public string UnitDestination
        {
            get { return _unitDestination; }
            set { SetProperty(ref _unitDestination, value); }
        }

        private string _unitDescription;
        public string UnitDescription
        {
            get { return _unitDescription; }
            set { SetProperty(ref _unitDescription, value); }
        }

        private string _date;
        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private string _trafficSchedule;
        public string TrafficSchedule
        {
            get { return _trafficSchedule; }
            set { SetProperty(ref _trafficSchedule, value); }
        }

        private string _line;
        public string Line
        {
            get { return _line; }
            set { SetProperty(ref _line, value); }
        }

        public bool IsValidDefaultValues()
        {
            return (Enum.GetValues(typeof(LackTypeEnum)).Cast<LackTypeEnum>().Any(e => e == LackType) &&
                    (LackType == LackTypeEnum.Landing || (LackType == LackTypeEnum.Boarding && PackingListId > 0))
                    && VehicleViewInfo != null);
        }

        private void SetPropertiesFromVehicleViewInfo()
        {
            Vehicle = VehicleViewInfo?.CarNumber;
            VehicleDescription = VehicleViewInfo?.Plate;
            UnitDestination = VehicleViewInfo?.UnitSendCode;
            UnitDescription = VehicleViewInfo?.UnitSendDescription;
            Date = VehicleViewInfo?.TrafficScheduleDateTime.ToString("dd/MM/yyyy HH:mm");
            TrafficSchedule = VehicleViewInfo?.TrafficScheduleId.ToString();
            Line = VehicleViewInfo?.LineCode;
        }
    }
}