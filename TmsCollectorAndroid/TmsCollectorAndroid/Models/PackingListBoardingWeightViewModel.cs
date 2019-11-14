using Prism.Mvvm;

namespace TmsCollectorAndroid.Models
{
    public class PackingListBoardingWeightViewModel : BindableBase
    {
        private string _carNumber;
        public string CarNumber
        {
            get { return _carNumber; }
            set { SetProperty(ref _carNumber, value); }
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

        private string _trafficSchedule;
        public string TrafficSchedule
        {
            get { return _trafficSchedule; }
            set { SetProperty(ref _trafficSchedule, value); }
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

        private string _packAmount;
        public string PackAmount
        {
            get { return _packAmount; }
            set { SetProperty(ref _packAmount, value); }
        }

        private string _packReading;
        public string PackReading
        {
            get { return _packReading; }
            set { SetProperty(ref _packReading, value); }
        }

        private string _vehicleTotalCapacity;
        public string VehicleTotalCapacity
        {
            get { return _vehicleTotalCapacity; }
            set { SetProperty(ref _vehicleTotalCapacity, value); }
        }

        private string _packinglistTotalProportionalWeight;
        public string PackinglistTotalProportionalWeight
        {
            get { return _packinglistTotalProportionalWeight; }
            set { SetProperty(ref _packinglistTotalProportionalWeight, value); }
        }
    }
}