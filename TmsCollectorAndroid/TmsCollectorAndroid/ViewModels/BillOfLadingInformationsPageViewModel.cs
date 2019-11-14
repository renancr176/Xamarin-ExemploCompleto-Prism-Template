using System;
using System.Linq;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.ViewModels
{
    public class BillOfLadingInformationsPageViewModel : ViewModelBase
    {
        public BillOfLadingInformationsPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService,
            IPopupNavigation popupNavigation,
            IBillOfLadingService billOfLadingService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;
            _popupNavigation = popupNavigation;
            _billOfLadingService = billOfLadingService;

            Model = new BillOfLadingInformationsModel();
        }

        private readonly INotificationService _notificationService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IBillOfLadingService _billOfLadingService;

        private BillOfLadingInformationsModel _model;
        public BillOfLadingInformationsModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        #region Commands

        private DelegateCommand _confirmationCommand;
        public DelegateCommand ConfirmationCommand =>
            _confirmationCommand ?? (_confirmationCommand = new DelegateCommand(ConfirmationCommandHandler));

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearCommandHandler));

        #endregion

        #region Command Handlers

        private async void ConfirmationCommandHandler()
        {
            if (!string.IsNullOrEmpty(Model.Reading))
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var getBillOfLadingInformationsByBarCode = await _billOfLadingService.GetBillOfLadingInformationsByBarCode(Model.Reading);

                await _popupNavigation.PopAllAsync();

                if (getBillOfLadingInformationsByBarCode.Response?.BillOfLadingInformations != null 
                    && getBillOfLadingInformationsByBarCode.Response.BillOfLadingInformations.Any())
                {
                    var informations = getBillOfLadingInformationsByBarCode.Response.BillOfLadingInformations.ToArray();

                    Model.Cte = informations[0].ToString() + "-" + informations[1].ToString();
                    Model.Origim = informations[2].ToString();
                    Model.Destination = informations[3].ToString();
                    Model.Invoice = informations[4].ToString();
                    Model.Vol = informations[5].ToString();
                    Model.WeightVol = informations[6].ToString() + " kg";
                    Model.TotalVolume = informations[7].ToString();
                    Model.TotalBaseWeight = informations[8].ToString() + " kg";
                    Model.TotalRealWeight = informations[9].ToString() + " kg";
                    Model.TotalWeightCubicated = informations[10].ToString() + " kg";

                    Model.ReadingIsReadOnly = true;
                }
                else
                {
                    Model.Reading = String.Empty;
                    await _notificationService.NotifyAsync("Código de barras não encontrado.",
                        SoundEnum.Erros);
                    Model.ReadingFocus();
                }
            }
            else
            {
                await _notificationService.NotifyAsync("Realize a leitura do código de barras.", SoundEnum.Alert);
            }
        }

        private void ClearCommandHandler()
        {
            Model.ClearModel();
        }

        #endregion
    }
}
