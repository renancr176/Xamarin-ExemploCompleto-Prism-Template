using System;
using Prism.Commands;
using Prism.Navigation;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.ViewModels.PopupPages
{
    public class RequestUnitInputPopupPageViewModel : ViewModelBase
    {
        public RequestUnitInputPopupPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService,
            ICommonService commonService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;
            _commonService = commonService;

            Text = "Unidade";
        }

        private readonly INotificationService _notificationService;
        private readonly ICommonService _commonService;

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("Text", out string text))
                Text = text;

            if (parameters.TryGetValue("ValidUnitType", out ValidUnitTypeEnum validUnitType))
                ValidUnitType = validUnitType;
        }

        private ValidUnitTypeEnum _validUnitType;
        public ValidUnitTypeEnum ValidUnitType
        {
            get { return _validUnitType; }
            set { SetProperty(ref _validUnitType, value); }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private string _unit;
        public string Unit
        {
            get { return _unit; }
            set { SetProperty(ref _unit, value); }
        }

        private string _unitDescription;
        public string UnitDescription
        {
            get { return _unitDescription; }
            set { SetProperty(ref _unitDescription, value); }
        }

        public Action BtnConfirmationFocus;

        private UnitViewInfoModel _unitViewInfo;
        public UnitViewInfoModel UnitViewInfo
        {
            get { return _unitViewInfo; }
            set { SetProperty(ref _unitViewInfo, value); }
        }

        #region Commands

        private DelegateCommand _unitChangedCommand;
        public DelegateCommand UnitChangedCommand =>
            _unitChangedCommand ?? (_unitChangedCommand = new DelegateCommand(UnitChangedCommandHandler));

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommandHandler));

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand =>
            _confirmCommand ?? (_confirmCommand = new DelegateCommand(ConfirmCommandHandler));

        #endregion

        #region Command Handlers

        private async void UnitChangedCommandHandler()
        {
            if (!string.IsNullOrEmpty(Unit))
            {
                var validUnit = await _commonService.ValidUnit(Unit);

                UnitViewInfo = validUnit.Response;

                if (validUnit.IsSuccessStatusCode
                    && UnitViewInfo != null
                    && UnitViewInfo.Code == Unit)
                {
                    UnitDescription = UnitViewInfo.Description;
                    BtnConfirmationFocus();
                }
                else
                {
                    await _notificationService.NotifyAsync($"Unidade {Unit} inválida.", SoundEnum.Erros);
                    Unit = String.Empty;
                }
            }
        }

        private async void CancelCommandHandler()
        {
            await NavigationService.GoBackAsync(
                new NavigationParameters() { { "UnitInputConfirmed", false } });
        }

        private async void ConfirmCommandHandler()
        {
            if (!string.IsNullOrEmpty(Unit))
            {

                await NavigationService.GoBackAsync(
                    new NavigationParameters()
                    {
                        { "UnitInputConfirmed", true },
                        { "UnitViewInfo", UnitViewInfo }
                    });
            }
        }

        #endregion
    }
}
