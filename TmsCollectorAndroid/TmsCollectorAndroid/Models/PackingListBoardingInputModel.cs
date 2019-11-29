using System;
using Prism.Mvvm;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Models
{
    public class PackingListBoardingInputModel : BindableBase
    {
        public PackingListBoardingInputModel()
        {
            ClearModel();
        }

        private ProcessoEnum _processo;
        public ProcessoEnum Processo
        {
            get { return _processo; }
            set { SetProperty(ref _processo, value); }
        }

        private bool _finalized;
        public bool Finalized
        {
            get { return _finalized; }
            set { SetProperty(ref _finalized, value); }
        }

        #region Controles Inputs

        private bool _teamIsReadOnly;
        public bool TeamIsReadOnly
        {
            get { return _teamIsReadOnly; }
            set { SetProperty(ref _teamIsReadOnly, value); }
        }

        private bool _carNumberLineIsVisible;
        public bool CarNumberLineIsVisible
        {
            get { return _carNumberLineIsVisible; }
            set { SetProperty(ref _carNumberLineIsVisible, value); }
        }

        private bool _carNumberIsReadOnly;
        public bool CarNumberIsReadOnly
        {
            get { return _carNumberIsReadOnly; }
            set { SetProperty(ref _carNumberIsReadOnly, value); }
        }

        private bool _unitLineIsVisible;
        public bool UnitLineIsVisible
        {
            get { return _unitLineIsVisible; }
            set { SetProperty(ref _unitLineIsVisible, value); }
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

        private string _team;
        public string Team
        {
            get { return _team; }
            set { SetProperty(ref _team, value?.Trim().ToUpper()); }
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
            set { SetProperty(ref _carNumber, value?.Trim()); }
        }

        private string _vehicleDescription;
        public string VehicleDescription
        {
            get { return _vehicleDescription; }
            set { SetProperty(ref _vehicleDescription, value); }
        }

        private string _unit;
        public string Unit
        {
            get { return _unit; }
            set { SetProperty(ref _unit, value); }
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

        private string _transportAccessoriesAmount;
        public string TransportAccessoriesAmount
        {
            get { return _transportAccessoriesAmount; }
            set { SetProperty(ref _transportAccessoriesAmount, value); }
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
            set { SetProperty(ref _reading, value?.Trim()); }
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
            CarNumberIsReadOnly = false;

            ReadingIsReadOnly = true;

            CarNumber = VehicleDescription = Unit = UnitDestination = TrafficSchedule =
                Date = Line = TransportAccessoriesAmount = PackAmount = PackReading = Reading = String.Empty;
        }

        public bool IsValid()
        {
            switch (Processo)
            {
                case ProcessoEnum.LeituraUnica:
                    if (!string.IsNullOrEmpty(Team) && !string.IsNullOrEmpty(Reading))
                    {
                        return true;
                    }
                    return false;
                default:
                    if (!string.IsNullOrEmpty(Team) 
                        && !string.IsNullOrEmpty(CarNumber) 
                        && !string.IsNullOrEmpty(Reading))
                    {
                        return true;
                    }
                    return false;
            }
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
            set
            {
                SetProperty(ref _vehicleViewInfo, value);
            }
        }

        private PackingListViewInfoModel _packingListViewInfo;
        public PackingListViewInfoModel PackingListViewInfo
        {
            get { return _packingListViewInfo; }
            set
            {
                SetProperty(ref _packingListViewInfo, value);
                SetPropertiesFromPackingListViewInfo();
            }
        }

        private int _trafficScheduleDetailId;
        public int TrafficScheduleDetailId
        {
            get { return _trafficScheduleDetailId; }
            set { SetProperty(ref _trafficScheduleDetailId, value); }
        }

        private int _trafficScheduleDetailVersionId;
        public int TrafficScheduleDetailVersionId
        {
            get { return _trafficScheduleDetailVersionId; }
            set { SetProperty(ref _trafficScheduleDetailVersionId, value); }
        }
        private int _trafficScheduleDetailSequence;
        public int TrafficScheduleDetailSequence
        {
            get { return _trafficScheduleDetailSequence; }
            set { SetProperty(ref _trafficScheduleDetailSequence, value); }
        }

        private string _warehousePassword;
        public string WarehousePassword
        {
            get { return _warehousePassword; }
            set { SetProperty(ref _warehousePassword, value); }
        }

        private int _warehousePasswordId;
        public int WarehousePasswordId
        {
            get { return _warehousePasswordId; }
            set { SetProperty(ref _warehousePasswordId, value); }
        }

        private UnitViewInfoModel _unitDestinationViewInfo;
        public UnitViewInfoModel UnitDestinationViewInfo
        {
            get { return _unitDestinationViewInfo; }
            set { SetProperty(ref _unitDestinationViewInfo, value); }
        }

        public void SetPropertiesFromVehicleViewInfo()
        {
            TrafficSchedule = VehicleViewInfo?.TrafficScheduleId.ToString() ?? String.Empty;
            Line = VehicleViewInfo?.LineCode;
            VehicleDescription = VehicleViewInfo?.Plate;
            Unit = VehicleViewInfo?.UnitSendCode;
            Date = VehicleViewInfo?.TrafficScheduleDateTime.ToString("dd/MM/yyyy HH:mm");
            UnitDestination = ((Processo != ProcessoEnum.LeituraUnica)? VehicleViewInfo?.UnitSendCode : VehicleViewInfo?.UnitSendDescription);
        }

        private void SetPropertiesFromPackingListViewInfo()
        {
            TransportAccessoriesAmount = PackingListViewInfo?.TransportAccessoriesAmount.ToString();
            PackAmount = PackingListViewInfo?.TotalBillOfLading.ToString() ?? String.Empty;
            PackReading = PackingListViewInfo?.TotalPack.ToString() ?? String.Empty;
        }
    }
}