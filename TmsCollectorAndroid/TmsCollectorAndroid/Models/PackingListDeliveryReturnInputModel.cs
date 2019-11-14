using System;
using Prism.Mvvm;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Models
{
    public class PackingListDeliveryReturnInputModel : BindableBase
    {
        public PackingListDeliveryReturnInputModel()
        {
            ClearModel();
        }

        #region Controles Inputs

        private bool _packingListNumberIsReadOnly;
        public bool PackingListNumberIsReadOnly
        {
            get { return _packingListNumberIsReadOnly; }
            set { SetProperty(ref _packingListNumberIsReadOnly, value); }
        }

        private bool _packingListDigitIsReadOnly;
        public bool PackingListDigitIsReadOnly
        {
            get { return _packingListDigitIsReadOnly; }
            set { SetProperty(ref _packingListDigitIsReadOnly, value); }
        }

        private bool _dateIsReadOnly;
        public bool DateIsReadOnly
        {
            get { return _dateIsReadOnly; }
            set { SetProperty(ref _dateIsReadOnly, value); }
        }

        private bool _timeIsReadOnly;
        public bool TimeIsReadOnly
        {
            get { return _timeIsReadOnly; }
            set { SetProperty(ref _timeIsReadOnly, value); }
        }

        private bool _driverIsReadOnly;
        public bool DriverIsReadOnly
        {
            get { return _driverIsReadOnly; }
            set { SetProperty(ref _driverIsReadOnly, value); }
        }

        private bool _vehicleIsReadOnly;
        public bool VehicleIsReadOnly
        {
            get { return _vehicleIsReadOnly; }
            set { SetProperty(ref _vehicleIsReadOnly, value); }
        }

        private bool _readingIsReadOnly;
        public bool ReadingIsReadOnly
        {
            get { return _readingIsReadOnly; }
            set { SetProperty(ref _readingIsReadOnly, value); }
        }

        #endregion

        private string _packingListNumber;
        public string PackingListNumber
        {
            get { return _packingListNumber; }
            set { SetProperty(ref _packingListNumber, value); }
        }

        private string _packingListDigit;
        public string PackingListDigit
        {
            get { return _packingListDigit; }
            set { SetProperty(ref _packingListDigit, value); }
        }

        public Action PackingListDigitFocus;

        private string _date;
        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public Action DateFocus;

        private string _time;
        public string Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        public DateTime? CheckOutDate
        {
            get
            {
                if (!string.IsNullOrEmpty(Date) && DateTime.TryParse(Date, out DateTime date)
                && !string.IsNullOrEmpty(Time) && TimeSpan.TryParse(Time, out TimeSpan time))
                {
                    return date.Add(time);
                }

                return null;
            }
        }

        private string _driver;
        public string Driver
        {
            get { return _driver; }
            set { SetProperty(ref _driver, value); }
        }

        public Action DriverFocus;

        private string _driverDescription;
        public string DriverDescription
        {
            get { return _driverDescription; }
            set { SetProperty(ref _driverDescription, value); }
        }

        private string _vehicle;
        public string Vehicle
        {
            get { return _vehicle; }
            set { SetProperty(ref _vehicle, value); }
        }

        public Action VehicleFocus;

        private string _vehicleView;
        public string VehicleView
        {
            get { return _vehicleView; }
            set { SetProperty(ref _vehicleView, value); }
        }

        private string _bolAmountView;
        public string BolAmountView
        {
            get { return _bolAmountView; }
            set { SetProperty(ref _bolAmountView, value); }
        }

        private string _packsAmountView;
        public string PacksAmountView
        {
            get { return _packsAmountView; }
            set { SetProperty(ref _packsAmountView, value); }
        }

        private string _reading;
        public string Reading
        {
            get { return _reading; }
            set { SetProperty(ref _reading, value); }
        }

        public Action ReadingFocus;

        public void ClearModel()
        {
            PackingListNumberIsReadOnly = PackingListDigitIsReadOnly = DateIsReadOnly = TimeIsReadOnly = false;

            DriverIsReadOnly = VehicleIsReadOnly = ReadingIsReadOnly = true;

            PackingListNumber = PackingListDigit = Date = Time = Driver = DriverDescription =
                Vehicle = VehicleView = BolAmountView = PacksAmountView = Reading = String.Empty;
        }

        public bool IsValid()
        {
            return (PackingListViewInfo != null && PackingListViewInfo.Valid);
        }

        private PackingListViewInfoModel _packingListViewInfo;
        public PackingListViewInfoModel PackingListViewInfo
        {
            get { return _packingListViewInfo; }
            set { SetProperty(ref _packingListViewInfo, value); }
        }

        private DriverInfoViewModel _driverInfoView;
        public DriverInfoViewModel DriverInfoView
        {
            get { return _driverInfoView; }
            set { SetProperty(ref _driverInfoView, value); }
        }

        private VehicleViewInfoModel _vehicleViewInfo;
        public VehicleViewInfoModel VehicleViewInfo
        {
            get { return _vehicleViewInfo; }
            set { SetProperty(ref _vehicleViewInfo, value); }
        }
    }
}