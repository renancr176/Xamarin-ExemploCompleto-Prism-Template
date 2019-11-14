using System;
using Prism.Mvvm;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Models
{
    public class PackingListLandingInputModel : BindableBase
    {
        public PackingListLandingInputModel()
        {
            ClearModel();
        }

        #region Controle Inputs

        private bool _teamIsReadOnly;
        public bool TeamIsReadOnly
        {
            get { return _teamIsReadOnly; }
            set { SetProperty(ref _teamIsReadOnly, value); }
        }

        private bool _carIsReadOnly;
        public bool CarIsReadOnly
        {
            get { return _carIsReadOnly; }
            set { SetProperty(ref _carIsReadOnly, value); }
        }

        private bool _readingIsReadOnly;
        public bool ReadingIsReadOnly
        {
            get { return _readingIsReadOnly; }
            set { SetProperty(ref _readingIsReadOnly, value); }
        }

        public Action CarNumberFocus;

        public Action ReadingFocus;

        #endregion

        private bool _finalized;
        public bool Finalized
        {
            get { return _finalized; }
            set { SetProperty(ref _finalized, value); }
        }

        private string _team;
        public string Team
        {
            get { return _team; }
            set { SetProperty(ref _team, value.Trim().ToUpper()); }
        }

        private string _teamDescription;
        public string TeamDescription
        {
            get { return _teamDescription; }
            set { SetProperty(ref _teamDescription, value); }
        }

        private string _carNumber;
        public string CarNumber
        {
            get { return _carNumber; }
            set { SetProperty(ref _carNumber, value.Trim()); }
        }

        private string _vehicleDescription;
        public string VehicleDescription
        {
            get { return _vehicleDescription; }
            set { SetProperty(ref _vehicleDescription, value); }
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

        private string _reading;
        public string Reading
        {
            get { return _reading; }
            set { SetProperty(ref _reading, value); }
        }

        public void ClearModel()
        {
            Finalized = false;

            TeamIsReadOnly = false;

            Team = TeamDescription = String.Empty;

            ClearModelAfterTeam();
        }

        public void ClearModelAfterTeam()
        {
            CarIsReadOnly = false;
            ReadingIsReadOnly = true;

            CarNumber = VehicleDescription =
                TrafficSchedule = Date = Line = PackAmount = PackReading = Reading = String.Empty;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Team)
                   && !string.IsNullOrEmpty(CarNumber)
                   && !string.IsNullOrEmpty(Reading);
        }

        private TeamViewInfoModel _teamViewInfo;
        public TeamViewInfoModel TeamViewInfo
        {
            get { return _teamViewInfo; }
            set { SetProperty(ref _teamViewInfo, value); }
        }

        private VehicleViewInfoModel _vehicleViewInfo;
        public VehicleViewInfoModel VehicleViewInfo
        {
            get { return _vehicleViewInfo; }
            set { SetProperty(ref _vehicleViewInfo, value); }
        }
    }
}