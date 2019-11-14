using System.Linq;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.TmsApi.Models.Responses;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.ViewModels
{
    public class PacksDetailViewPageViewModel : ViewModelBase
    {
        public PacksDetailViewPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService,
            IPopupNavigation popupNavigation,
            IBillOfLadingService billOfLadingService,
            IWifiService wifiService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;
            _popupNavigation = popupNavigation;
            _billOfLadingService = billOfLadingService;
            _wifiService = wifiService;

            Model = new PacksDetailViewModel();
        }

        private readonly INotificationService _notificationService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IBillOfLadingService _billOfLadingService;
        private readonly IWifiService _wifiService;

        private PacksDetailViewModel _model;
        public PacksDetailViewModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        #region Navigation Methods

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("BillOfLadingCollectorViewInfo", out BillOfLadingCollectorViewInfoModel billOfLadingCollectorViewInfo))
            {
                Model.BillOfLadingCollectorViewInfo = billOfLadingCollectorViewInfo;
                Model.Ctrc = $"CTRC: {Model.BillOfLadingCollectorViewInfo.Number}-{Model.BillOfLadingCollectorViewInfo.Digit}-{Model.BillOfLadingCollectorViewInfo.UnitEmissionCode}";
            }
        }

        #endregion

        #region Commands

        private DelegateCommand _checkAllItensCommand;
        public DelegateCommand CheckAllItensCommand =>
            _checkAllItensCommand ?? (_checkAllItensCommand = new DelegateCommand(CheckAllItensCommandHandler));

        private DelegateCommand _unCheckAllItensCommand;
        public DelegateCommand UnCheckAllItensCommand =>
            _unCheckAllItensCommand ?? (_unCheckAllItensCommand = new DelegateCommand(UnCheckAllItensCommandHandler));

        private DelegateCommand _confirmationCommand;
        public DelegateCommand ConfirmationCommand =>
            _confirmationCommand ?? (_confirmationCommand = new DelegateCommand(ConfirmationCommandHandler));

        #endregion

        #region Command Handlers

        private void CheckAllItensCommandHandler()
        {
            foreach (var item in Model.BillOfLadingPackLvItems.Where(item => !item.Checked))
            {
                item.Checked = true;
            }

            Model.SetAmountSelected();
        }

        private void UnCheckAllItensCommandHandler()
        {
            foreach (var item in Model.BillOfLadingPackLvItems.Where(item => item.Checked))
            {
                item.Checked = false;
            }

            Model.SetAmountSelected();
        }

        private async void ConfirmationCommandHandler()
        {
            if (Model.BillOfLadingPackLvItems.Any(item => item.Checked))
            {
                if (await _notificationService.AskQuestionAsync(
                    "Confirma solicitação de impressão do(s) volume(s) selecionado(s)?", SoundEnum.Alert))
                {
                    await _popupNavigation.PushAsync(new LoadingPopupPage());

                    var result = await _billOfLadingService.SendListBillOfLadingPack(
                        new SendListBillOfLadingPackModel(_wifiService.MacAddress,
                            Model.BillOfLadingPackLvItems.Where(item => item.Checked)
                                .Select(item => item.BillOfLadingPack.Id.ToString())));

                    await _popupNavigation.PopAllAsync();

                    if (result.IsSuccessStatusCode)
                    {
                        await _notificationService.NotifyAsync("Solicitação enviada com sucesso.", SoundEnum.Alert);
                        await NavigationService.GoBackAsync();
                    }
                }
            }
            else
            {
                await _notificationService.NotifyAsync("Selecione ao menos um volume.", SoundEnum.Erros);
            }
        }

        #endregion
    }
}
