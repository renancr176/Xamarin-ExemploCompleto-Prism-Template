using System;
using Prism.Mvvm;

namespace TmsCollectorAndroid.Models
{
    public class BillOfLadingInformationsModel : BindableBase
    {
        public BillOfLadingInformationsModel()
        {
            ClearModel();
        }

        #region Controles Inputs

        private bool _readingIsReadOnly;
        public bool ReadingIsReadOnly
        {
            get { return _readingIsReadOnly; }
            set { SetProperty(ref _readingIsReadOnly, value); }
        }

        #endregion

        private string _cte;
        public string Cte
        {
            get { return _cte; }
            set { SetProperty(ref _cte, value); }
        }

        private string _origim;
        public string Origim
        {
            get { return _origim; }
            set { SetProperty(ref _origim, value); }
        }

        private string _destination;
        public string Destination
        {
            get { return _destination; }
            set { SetProperty(ref _destination, value); }
        }

        private string _invoice;
        public string Invoice
        {
            get { return _invoice; }
            set { SetProperty(ref _invoice, value); }
        }

        private string _vol;
        public string Vol
        {
            get { return _vol; }
            set { SetProperty(ref _vol, value); }
        }

        private string _weightVol;
        public string WeightVol
        {
            get { return _weightVol; }
            set { SetProperty(ref _weightVol, value); }
        }

        private string _totalVolume;
        public string TotalVolume
        {
            get { return _totalVolume; }
            set { SetProperty(ref _totalVolume, value); }
        }

        private string _totalBaseWeight;
        public string TotalBaseWeight
        {
            get { return _totalBaseWeight; }
            set { SetProperty(ref _totalBaseWeight, value); }
        }

        private string _totalRealWeight;
        public string TotalRealWeight
        {
            get { return _totalRealWeight; }
            set { SetProperty(ref _totalRealWeight, value); }
        }

        private string _totalWeightCubicated;
        public string TotalWeightCubicated
        {
            get { return _totalWeightCubicated; }
            set { SetProperty(ref _totalWeightCubicated, value); }
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
            ReadingIsReadOnly = false;

            Cte = Origim = Destination = Invoice = Vol = WeightVol = TotalVolume =
                TotalBaseWeight = TotalRealWeight = TotalWeightCubicated = Reading = String.Empty;
        }
    }
}