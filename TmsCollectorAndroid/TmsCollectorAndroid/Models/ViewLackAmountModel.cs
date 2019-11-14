using System.Linq;
using Prism.Mvvm;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Models
{
    public class ViewLackAmountModel : BindableBase
    {
        private PackingListDetailViewInfoModel _packingListDetailViewInfo;
        public PackingListDetailViewInfoModel PackingListDetailViewInfo
        {
            get { return _packingListDetailViewInfo; }
            set
            {
                SetProperty(ref _packingListDetailViewInfo, value);
                AmoutUnConfirmed = $"{value?.Packs.Count(p => p.Confirmed == false)} de {value?.Amount}";
            }
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

        private string _billOfLading;
        public string BillOfLading
        {
            get { return _billOfLading; }
            set { SetProperty(ref _billOfLading, value); }
        }

        private string _amoutUnConfirmed;
        public string AmoutUnConfirmed
        {
            get { return _amoutUnConfirmed; }
            set { SetProperty(ref _amoutUnConfirmed, value); }
        }
    }
}