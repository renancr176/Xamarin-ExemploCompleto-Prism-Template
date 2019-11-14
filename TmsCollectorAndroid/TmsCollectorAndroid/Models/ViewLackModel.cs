using System;
using System.Linq;
using Prism.Mvvm;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;

namespace TmsCollectorAndroid.Models
{
    public class ViewLackModel : BindableBase
    {

        #region Input Controls

        private bool _vehicleLineIsVisible;
        public bool VehicleLineIsVisible
        {
            get { return _vehicleLineIsVisible; }
            set
            {
                SetProperty(ref _vehicleLineIsVisible, value);
                UnitSentLineIsVisible = !value;
            }
        }

        private bool _unitSentLineIsVisible;
        public bool UnitSentLineIsVisible
        {
            get { return _unitSentLineIsVisible; }
            set { SetProperty(ref _unitSentLineIsVisible, value); }
        }

        private bool _lineIsVisible;
        public bool LineIsVisible
        {
            get { return _lineIsVisible; }
            set
            {
                SetProperty(ref _lineIsVisible, value);
                TrafficScheduleUsesFullWidth = !value;
            }
        }

        private bool _trafficScheduleUsesFullWidth;
        public bool TrafficScheduleUsesFullWidth
        {
            get { return _trafficScheduleUsesFullWidth; }
            set { SetProperty(ref _trafficScheduleUsesFullWidth, value); }
        }

        #endregion

        private LackTypeEnum _lackType;
        public LackTypeEnum LackType
        {
            get { return _lackType; }
            set { SetProperty(ref _lackType, value); }
        }

        private int? _packingListId;
        public int? PackingListId
        {
            get { return _packingListId; }
            set { SetProperty(ref _packingListId, value); }
        }

        private int? _trafficScheduleDetailId;
        public int? TrafficScheduleDetailId
        {
            get { return _trafficScheduleDetailId; }
            set { SetProperty(ref _trafficScheduleDetailId, value); }
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

        private string _unitSent;
        public string UnitSent
        {
            get { return _unitSent; }
            set { SetProperty(ref _unitSent, value); }
        }

        private string _unitSentDescription;
        public string UnitSentDescription
        {
            get { return _unitSentDescription; }
            set { SetProperty(ref _unitSentDescription, value); }
        }

        private string _unitDestination;
        public string UnitDestination
        {
            get { return _unitDestination; }
            set { SetProperty(ref _unitDestination, value); }
        }

        private string _unitDestinationDescription;
        public string UnitDestinationDescription
        {
            get { return _unitDestinationDescription; }
            set { SetProperty(ref _unitDestinationDescription, value); }
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

        private string _number;
        public string Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value?.Trim()); }
        }

        private string _digit;
        public string Digit
        {
            get { return _digit; }
            set { SetProperty(ref _digit, value?.Trim().ToUpper()); }
        }

        private string _unitEmission;
        public string UnitEmission
        {
            get { return _unitEmission; }
            set { SetProperty(ref _unitEmission, value?.Trim()); }
        }

        public string BillOfLading
        {
            get { return $"{Number}-{Digit} {UnitEmission}"; }
        }

        public void ClearModel()
        {
            Number = Digit = UnitEmission = String.Empty;
        }

        public bool IsValidDefaultValues()
        {
            return (Enum.GetValues(typeof(LackTypeEnum)).Cast<LackTypeEnum>().Any(e => e == LackType) &&
                    ((LackType == LackTypeEnum.Boarding && PackingListId.HasValue) ||
                     (LackType == LackTypeEnum.Landing && TrafficScheduleDetailId.HasValue)));
        }

        public bool IsValid()
        {
            return (IsValidDefaultValues() && Number.IsInt() && !string.IsNullOrEmpty(Digit) && UnitEmission.IsInt());
        }
    }
}