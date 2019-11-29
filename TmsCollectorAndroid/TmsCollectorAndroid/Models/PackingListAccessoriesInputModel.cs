using System;
using Prism.Mvvm;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Models
{
    public class PackingListAccessoriesInputModel : BindableBase
    {

        public PackingListAccessoriesInputModel()
        {
            ClearModel();
        }

        #region Controles Inputs

        private bool _destinationIsReadOnly;
        public bool DestinationIsReadOnly
        {
            get { return _destinationIsReadOnly; }
            set { SetProperty(ref _destinationIsReadOnly, value); }
        }

        private bool _cobolNumberIsReadOnly;
        public bool CobolNumberIsReadOnly
        {
            get { return _cobolNumberIsReadOnly; }
            set { SetProperty(ref _cobolNumberIsReadOnly, value); }
        }

        private bool _accessoryIsReadOnly;
        public bool AccessoryIsReadOnly
        {
            get { return _accessoryIsReadOnly; }
            set { SetProperty(ref _accessoryIsReadOnly, value); }
        }

        private bool _readingIsReadOnly;
        public bool ReadingIsReadOnly
        {
            get { return _readingIsReadOnly; }
            set { SetProperty(ref _readingIsReadOnly, value); }
        }

        #endregion

        private string _destination;
        public string Destination
        {
            get { return _destination; }
            set { SetProperty(ref _destination, value); }
        }

        public Action DestinationFocus;

        private string _cobolNumber;
        public string CobolNumber
        {
            get { return _cobolNumber; }
            set { SetProperty(ref _cobolNumber, value); }
        }

        private string _accessory;
        public string Accessory
        {
            get { return _accessory; }
            set { SetProperty(ref _accessory, value?.Trim().ToUpper()); }
        }

        public Action AccessoryFocus;

        private string _accessoryDescription;
        public string AccessoryDescription
        {
            get { return _accessoryDescription; }
            set { SetProperty(ref _accessoryDescription, value); }
        }

        private string _ctrc;
        public string Ctrc
        {
            get { return _ctrc; }
            set { SetProperty(ref _ctrc, value); }
        }

        private string _pack;
        public string Pack
        {
            get { return _pack; }
            set { SetProperty(ref _pack, value); }
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
            DestinationIsReadOnly = false;
            CobolNumberIsReadOnly = true;

            Destination = CobolNumber = String.Empty;

            UnitViewInfo = null;

            ClearModelAfterDetination();
        }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(Destination) && !string.IsNullOrEmpty(Accessory) &&
                    !string.IsNullOrEmpty(Reading));
        }

        public void ClearModelAfterDetination()
        {
            AccessoryIsReadOnly = false;
            ReadingIsReadOnly = true;

            CobolNumber = Accessory = AccessoryDescription = Ctrc = Pack = Reading = String.Empty;

            TransportAccessoryViewInfo = null;
            PackingListViewInfo = null;
        }

        private UnitViewInfoModel _unitViewInfo;
        public UnitViewInfoModel UnitViewInfo  
        {
            get { return _unitViewInfo; }
            set { SetProperty(ref _unitViewInfo, value); }
        }

        private TransportAccessoryViewInfoModel _transportAccessoryViewInfo;
        public TransportAccessoryViewInfoModel TransportAccessoryViewInfo
        {
            get { return _transportAccessoryViewInfo; }
            set
            {
                SetProperty(ref _transportAccessoryViewInfo, value);
                Accessory = value?.CobolNumber;
                AccessoryDescription = value?.Description;
            }
        }

        private PackingListViewInfoModel _packingListViewInfo;
        public PackingListViewInfoModel PackingListViewInfo
        {
            get { return _packingListViewInfo; }
            set { SetProperty(ref _packingListViewInfo, value); }
        }
    }
}