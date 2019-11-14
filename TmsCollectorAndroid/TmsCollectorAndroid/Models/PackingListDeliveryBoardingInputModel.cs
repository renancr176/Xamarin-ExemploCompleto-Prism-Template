using System;
using Prism.Mvvm;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Models
{
    public class PackingListDeliveryBoardingInputModel : BindableBase
    {
        public PackingListDeliveryBoardingInputModel()
        {
            ClearModel();
        }

        #region Controles Inputs

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

        private bool _lineIsReadOnly;
        public bool LineIsReadOnly
        {
            get { return _lineIsReadOnly; }
            set { SetProperty(ref _lineIsReadOnly, value); }
        }

        private bool _driverIsReadOnly;
        public bool DriverIsReadOnly
        {
            get { return _driverIsReadOnly; }
            set { SetProperty(ref _driverIsReadOnly, value); }
        }

        public Action DriverFocus;

        private bool _vehicleIsReadOnly;
        public bool VehicleIsReadOnly
        {
            get { return _vehicleIsReadOnly; }
            set { SetProperty(ref _vehicleIsReadOnly, value); }
        }

        public Action VehicleFocus;

        private bool _ReadingIsReadOnly;
        public bool ReadingIsReadOnly
        {
            get { return _ReadingIsReadOnly; }
            set { SetProperty(ref _ReadingIsReadOnly, value); }
        }

        public Action ReadingFocus;

        #endregion

        private string _date;
        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private string _time;
        public string Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        private string _line;
        public string Line
        {
            get { return _line; }
            set { SetProperty(ref _line, value?.Trim()); }
        }

        private string _lineDescription;
        public string LineDescription
        {
            get { return _lineDescription; }
            set { SetProperty(ref _lineDescription, value); }
        }

        private string _driver;
        public string Driver
        {
            get { return _driver; }
            set { SetProperty(ref _driver, value?.Trim()); }
        }

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
            set { SetProperty(ref _vehicle, value?.Trim()); }
        }

        private string _vehicleDescription;
        public string VehicleDescription
        {
            get { return _vehicleDescription; }
            set { SetProperty(ref _vehicleDescription, value); }
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
            set { SetProperty(ref _reading, value?.Trim()); }
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

        public void ClearModel()
        {
            DateIsReadOnly = TimeIsReadOnly = LineIsReadOnly = false;

            Date = Time = Line = LineDescription = String.Empty;

            LineInfoView = null;

            ClearModelAfterLine();
        }

        public void ClearModelAfterLine()
        {
            DriverIsReadOnly = false;

            Driver = DriverDescription = String.Empty;

            DriverInfoView = null;

            ClearModelAfterDriver();
        }

        public void ClearModelAfterDriver()
        {
            VehicleIsReadOnly = false;

            Vehicle = VehicleDescription = String.Empty;

            VehicleViewInfo = null;

            ClearModelAfterVehicle();
        }

        public void ClearModelAfterVehicle()
        {
            ReadingIsReadOnly = true;

            BolAmountView = PacksAmountView = Reading = String.Empty;

            PackingListViewInfo = null;
        }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(Date) && !string.IsNullOrEmpty(Time) && !string.IsNullOrEmpty(Line) &&
                    !string.IsNullOrEmpty(Driver) && !string.IsNullOrEmpty(Vehicle) && !string.IsNullOrEmpty(Reading));
        }

        private LineInfoViewModel _lineInfoView;
        public LineInfoViewModel LineInfoView
        {
            get { return _lineInfoView; }
            set
            {
                SetProperty(ref _lineInfoView, value);
                LineDescription = value?.Descritpion;
            }
        }

        private DriverInfoViewModel _driverInfoView;
        public DriverInfoViewModel DriverInfoView
        {
            get { return _driverInfoView; }
            set
            {
                SetProperty(ref _driverInfoView, value);
                DriverDescription = value?.Descritpion;
            }
        }

        private VehicleViewInfoModel _vehicleViewInfo;
        public VehicleViewInfoModel VehicleViewInfo
        {
            get { return _vehicleViewInfo; }
            set
            {
                SetProperty(ref _vehicleViewInfo, value);
                VehicleDescription = value?.Plate;
            }
        }

        private PackingListViewInfoModel _packingListViewInfo;
        public PackingListViewInfoModel PackingListViewInfo
        {
            get { return _packingListViewInfo; }
            set
            {
                SetProperty(ref _packingListViewInfo, value);
                BolAmountView = value?.TotalBillOfLading.ToString();
                PacksAmountView = value?.TotalPack.ToString();
            }
        }
    }
}