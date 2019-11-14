using System;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.ViewModels
{
    public class SendLabelInputPageViewModel : ViewModelBase
    {
        public SendLabelInputPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService,
            IBillOfLadingService billOfLadingService,
            IWifiService wifiService,
            IPopupNavigation popupNavigation) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;
            _billOfLadingService = billOfLadingService;
            _wifiService = wifiService;
            _popupNavigation = popupNavigation;

            Model = new SendLabelInputModel();
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
        }

        private readonly INotificationService _notificationService;
        private readonly IBillOfLadingService _billOfLadingService;
        private readonly IWifiService _wifiService;
        private readonly IPopupNavigation _popupNavigation;

        private SendLabelInputModel _model;
        public SendLabelInputModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        private Dictionary<string, DelegateCommand> _menuAdtionalButtons;
        public Dictionary<string, DelegateCommand> MenuAdtionalButtons
        {
            get { return _menuAdtionalButtons; }
            set { SetProperty(ref _menuAdtionalButtons, value); }
        }

        #region Navigation Methods

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("SendLabelInputModel", out SendLabelInputModel model))
            {
                model.PackFocus = Model.PackFocus;
                model.BtnConfirmFocus = Model.BtnConfirmFocus;
                model.ClearModel();
                Model = model;
            }
        }

        #endregion

        #region Private Methods

        private void AddMenuAdtionalBunttons()
        {
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>()
            {
                { "Selecionar Volumes", PacksDetailViewCommand }
            };
        }

        #endregion

        #region Commands

        private DelegateCommand _ctrcChangedCommand;
        public DelegateCommand CtrcChangedCommand =>
            _ctrcChangedCommand ?? (_ctrcChangedCommand = new DelegateCommand(CtrcChangedCommandHandler));

        private DelegateCommand _packChangedCommand;
        public DelegateCommand PackChangedCommand =>
            _packChangedCommand ?? (_packChangedCommand = new DelegateCommand(PackChangedCommandHandler));

        private DelegateCommand _confirmationCommand;
        public DelegateCommand ConfirmationCommand =>
            _confirmationCommand ?? (_confirmationCommand = new DelegateCommand(ConfirmationCommandHandler));

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearCommandHandler));

        #region Menu Commands

        private DelegateCommand _packsDetailViewCommand;
        public DelegateCommand PacksDetailViewCommand =>
            _packsDetailViewCommand ?? (_packsDetailViewCommand = new DelegateCommand(PacksDetailViewCommandHandler));

        #endregion

        #endregion

        #region Command Handlers

        private async void CtrcChangedCommandHandler()
        {
            if (Model.Number.IsInt() 
            && !string.IsNullOrEmpty(Model.Digit) 
            && Model.UnitEmission.IsInt())
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                var billOfLadingCollectorView = await _billOfLadingService.GetBillOfLading(Model.Number.ToInt(), Model.Digit, Model.UnitEmission);

                await _popupNavigation.PopAllAsync();

                Model.BillOfLadingCollectorViewInfo = billOfLadingCollectorView.Response;

                if (Model.BillOfLadingCollectorViewInfo != null)
                {
                    Model.PackAmount = Model.BillOfLadingCollectorViewInfo.PackAmount.ToString();

                    Model.NumberIsReadOnly = Model.DigitIsReadOnly = Model.UnitEmissionIsReadOnly = true;
                    Model.PackIsReadOnly = false;
                    Model.PackFocus();

                    AddMenuAdtionalBunttons();
                }
                else
                {
                    await _notificationService.NotifyAsync("CTRC inválido.", SoundEnum.Erros);
                }
            }
            else
            {
                Model.BillOfLadingCollectorViewInfo = null;
                MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
            }
        }

        private async void PackChangedCommandHandler()
        {
            if (Model.PackIsValid())
            {
                Model.BtnConfirmFocus();
            }
            else if (!string.IsNullOrEmpty(Model.Pack))
            {
                var msg = ((!Model.Pack.IsInt())
                        ? "Formato inválido."
                        : "Volume inexistente.");

                await _notificationService.NotifyAsync(msg, SoundEnum.Erros);

                Model.Pack = String.Empty;
            }
        }

        private async void ConfirmationCommandHandler()
        {
            if (Model.BillOfLadingCollectorViewInfo != null
            && Model.PackIsValid())
            {
                if (await _notificationService.AskQuestionAsync("Confirma solicitação de impressão do volume?", SoundEnum.Alert))
                {
                    await _popupNavigation.PushAsync(new LoadingPopupPage());

                    var getBillOfLadingPackId = await _billOfLadingService.GetBillOfLadingPackId(Model.Number.ToInt(),
                        Model.Digit, Model.UnitEmission, Model.Pack.ToInt());

                    if (getBillOfLadingPackId.Response != null && getBillOfLadingPackId.Response.PackId > 0)
                    {
                        var sendBillOfLadingPack = await _billOfLadingService.SendBillOfLadingPack(
                            new SendBillOfLadingPackModel(_wifiService.MacAddress,
                                getBillOfLadingPackId.Response.PackId));

                        await _popupNavigation.PopAllAsync();

                        if (sendBillOfLadingPack.IsSuccessStatusCode)
                        {
                            await _notificationService.NotifyAsync("Solicitação enviada com sucesso.", SoundEnum.Alert);
                        }
                    }
                    else
                    {
                        await _popupNavigation.PopAllAsync();
                    }
                }
            }
            else
            {
                await _notificationService.NotifyAsync("Informe todos os campos obrigatórios.", SoundEnum.Erros);
            }

            Model.Pack = String.Empty;
        }

        void ClearCommandHandler()
        {
            Model.ClearModel();
            MenuAdtionalButtons = new Dictionary<string, DelegateCommand>();
        }

        #region Menu Command Handlers

        private async void PacksDetailViewCommandHandler()
        {
            if (Model.BillOfLadingCollectorViewInfo != null)
            {
                Model.ClearModel();
                await NavigationService.NavigateAsync("PacksDetailViewPage", new NavigationParameters()
                {
                    {"BillOfLadingCollectorViewInfo", Model.BillOfLadingCollectorViewInfo}
                });
            }
        }

        #endregion

        #endregion
    }
}
