using System;
using System.Collections.Generic;
using Prism.Mvvm;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Models.TmsApiExtendedModels;

namespace TmsCollectorAndroid.Models
{
    public class UniversalLandingModel : BindableBase
    {
        public UniversalLandingModel()
        {
            ClearModel();
        }

        #region Input Contols

        private bool _readingIsReadOnly;
        public bool ReadingIsReadOnly
        {
            get { return _readingIsReadOnly; }
            set
            {
                SetProperty(ref _readingIsReadOnly, value);
                if (!value)
                    LblReadVolumesMissingVolumes = "Consulta Volumes Lidos";
            }
        }

        private bool _ctrcIsReadOnly;
        public bool CtrcIsReadOnly
        {
            get { return _ctrcIsReadOnly; }
            set
            {
                SetProperty(ref _ctrcIsReadOnly, value);
                if (!value)
                    LblReadVolumesMissingVolumes = "Consulta Volumes Faltantes";
            }
        }

        private string _lblReadVolumesMissingVolumes;
        public string LblReadVolumesMissingVolumes
        {
            get { return _lblReadVolumesMissingVolumes; }
            set { SetProperty(ref _lblReadVolumesMissingVolumes, value); }
        }

        private bool _treeIsEnabled;
        public bool TreeIsEnabled
        {
            get { return _treeIsEnabled; }
            set { SetProperty(ref _treeIsEnabled, value); }
        }

        public Action ReadingFocus;
        public Action NumberFocus;
        public Action BtnConfirmFocus;

        #endregion

        private string _reading;
        public string Reading
        {
            get { return _reading; }
            set { SetProperty(ref _reading, value?.Trim()); }
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

        private IEnumerable<PackingListDetailViewInfoModel> _packingListDetailViewInfo;
        public IEnumerable<PackingListDetailViewInfoModel> PackingListDetailViewInfo
        {
            get { return _packingListDetailViewInfo; }
            set { SetProperty(ref _packingListDetailViewInfo, value); }
        }

        public void ClearModel()
        {
            ReadingIsReadOnly = false;
            CtrcIsReadOnly = true;
            TreeIsEnabled = false;

            Reading = Number = Digit = UnitEmission = String.Empty;

            PackingListDetailViewInfo = new List<PackingListDetailViewInfoModel>();
        }

        public bool CtrcIsValid()
        {
            return (Number.IsInt() && !string.IsNullOrEmpty(Digit) && UnitEmission.IsInt());
        }
    }
}