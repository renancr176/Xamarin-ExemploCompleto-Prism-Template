using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;
using TmsCollectorAndroid.TmsApi.Models.Responses;
using Xamarin.Forms.Internals;

namespace TmsCollectorAndroid.Models
{
    public class PacksDetailViewModel : BindableBase
    {
        public PacksDetailViewModel()
        {
            BillOfLadingPackLvItems = new ObservableCollection<BillOfLadingPackLvItem>();
            ClearModel();
        }
        
        private BillOfLadingCollectorViewInfoModel _billOfLadingCollectorViewInfo;
        public BillOfLadingCollectorViewInfoModel BillOfLadingCollectorViewInfo
        {
            get { return _billOfLadingCollectorViewInfo; }
            set
            {
                SetProperty(ref _billOfLadingCollectorViewInfo, value);

                if (value != null && value.BillOfLadingPacks != null)
                    BillOfLadingPackLvItems = new ObservableCollection<BillOfLadingPackLvItem>(value.BillOfLadingPacks
                        .Select(billOfLadingPack => new BillOfLadingPackLvItem(billOfLadingPack,
                            $"{(value.BillOfLadingPacks.IndexOf(billOfLadingPack) + 1)}/{value.BillOfLadingPacks.Count()}"))
                        .ToList());
            }
        }

        private string _ctrc;
        public string Ctrc
        {
            get { return _ctrc; }
            set { SetProperty(ref _ctrc, value); }
        }

        private ObservableCollection<BillOfLadingPackLvItem> _billOfLadingPackLvItems;
        public ObservableCollection<BillOfLadingPackLvItem> BillOfLadingPackLvItems
        {
            get { return _billOfLadingPackLvItems; }
            set { SetProperty(ref _billOfLadingPackLvItems, value); }
        }

        private string _amountSelected;
        public string AmountSelected
        {
            get { return _amountSelected; }
            set { SetProperty(ref _amountSelected, value); }
        }

        public void ClearModel()
        {
            AmountSelected = "0";
        }

        public void SetAmountSelected()
        {
            AmountSelected = BillOfLadingPackLvItems.Count(item => item.Checked).ToString();
        }
    }

    public class BillOfLadingPackLvItem : BindableBase
    {
        public BillOfLadingPackModel BillOfLadingPack { get; private set; }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set { SetProperty(ref _checked, value); }
        }

        public BillOfLadingPackLvItem(BillOfLadingPackModel billOfLadingPack, string text)
        {
            BillOfLadingPack = billOfLadingPack;
            Text = text;
            Checked = false;
        }

        public void Toggle()
        {
            Checked = !Checked;
        }
    }
}