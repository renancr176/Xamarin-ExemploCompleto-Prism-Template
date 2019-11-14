using System;
using Prism.Mvvm;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Models
{
    public class SendLabelInputModel : BindableBase
    {
        public SendLabelInputModel()
        {
            ClearModel();
        }

        #region Input Controls

        private bool _numberIsReadOnly;
        public bool NumberIsReadOnly
        {
            get { return _numberIsReadOnly; }
            set { SetProperty(ref _numberIsReadOnly, value); }
        }

        private bool _digitIsReadOnly;
        public bool DigitIsReadOnly
        {
            get { return _digitIsReadOnly; }
            set { SetProperty(ref _digitIsReadOnly, value); }
        }

        private bool _unitEmissionIsReadOnly;
        public bool UnitEmissionIsReadOnly
        {
            get { return _unitEmissionIsReadOnly; }
            set { SetProperty(ref _unitEmissionIsReadOnly, value); }
        }

        private bool _packnIsReadOnly;
        public bool PackIsReadOnly
        {
            get { return _packnIsReadOnly; }
            set { SetProperty(ref _packnIsReadOnly, value); }
        }

        public Action PackFocus;

        public Action BtnConfirmFocus;

        #endregion

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

        private string _line;
        public string Line
        {
            get { return _line; }
            set { SetProperty(ref _line, value); }
        }

        private string _trafficSchedule;
        public string TrafficSchedule
        {
            get { return _trafficSchedule; }
            set { SetProperty(ref _trafficSchedule, value); }
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

        private string _pack;
        public string Pack
        {
            get { return _pack; }
            set { SetProperty(ref _pack, value?.Trim()); }
        }

        private string _packAmount;
        public string PackAmount
        {
            get { return _packAmount; }
            set { SetProperty(ref _packAmount, value); }
        }

        public void ClearModel()
        {
            NumberIsReadOnly = DigitIsReadOnly = UnitEmissionIsReadOnly = false;

            PackIsReadOnly = true;

            Number = Digit = UnitEmission = Pack = PackAmount = String.Empty;
        }

        public bool IsValid()
        {
            return (Number.IsInt() && !string.IsNullOrEmpty(Digit) && UnitEmission.IsInt() && Pack.IsInt() &&
                    Pack.ToInt() > 0 && Pack.ToInt() <= PackAmount.ToInt());
        }

        public bool PackIsValid()
        {
            return (Pack.IsInt()
                    && Pack.ToInt() > 0
                    && Pack.ToInt() <= PackAmount.ToInt());
        }

        private BillOfLadingCollectorViewInfoModel _billOfLadingCollectorViewInfo;
        public BillOfLadingCollectorViewInfoModel BillOfLadingCollectorViewInfo
        {
            get { return _billOfLadingCollectorViewInfo; }
            set { SetProperty(ref _billOfLadingCollectorViewInfo, value); }
        }
    }
}