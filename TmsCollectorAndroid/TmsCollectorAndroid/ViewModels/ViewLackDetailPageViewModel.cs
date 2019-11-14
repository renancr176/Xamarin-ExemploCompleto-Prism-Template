using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models.TmsApiExtendedModels;

namespace TmsCollectorAndroid.ViewModels
{
    public class ViewLackDetailPageViewModel : ViewModelBase
    {
        public ViewLackDetailPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            PackingListDetailViewInfo = new List<PackingListDetailViewInfoModel>();
        }

        private IEnumerable<PackingListDetailViewInfoModel> _packingListDetailViewInfo;
        public IEnumerable<PackingListDetailViewInfoModel> PackingListDetailViewInfo
        {
            get { return _packingListDetailViewInfo; }
            set { SetProperty(ref _packingListDetailViewInfo, value); }
        }

        #region Navigation Methods

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("Title", out string title))
            {
                Title = title;
            }

            if (parameters.TryGetValue("PackingListDetailViewInfo", out IEnumerable<TmsApi.Models.Responses.PackingListDetailViewInfoModel> packingListDetailViewInfo))
            {
                PackingListDetailViewInfo = packingListDetailViewInfo.Select(p => p.ToDerivedClass());
            }
        }

        #endregion
    }
}
