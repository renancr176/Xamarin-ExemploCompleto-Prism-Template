using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.ViewModels
{
    public class ViewLackPageViewModel : ViewModelBase
    {
        public ViewLackPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService,
            IBoardingService boardingService,
            ILandingService landingService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;
            _boardingService = boardingService;
            _landingService = landingService;

            Model = new ViewLackModel();
        }

        private readonly INotificationService _notificationService;
        private readonly IBoardingService _boardingService;
        private readonly ILandingService _landingService;

        private ViewLackModel _model;
        public ViewLackModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        #region Navigation Methods

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("ViewLackModel", out ViewLackModel model)
            && model.IsValidDefaultValues())
            {
                Model = model;
            }
            else
            {
                NavigationService.GoBackAsync();
            }
        }

        #endregion

        #region Private Methods

        private async Task<PackingListDetailViewInfoModel> GetViewLack()
        {
            switch (Model.LackType)
            {
                case LackTypeEnum.Boarding:
                    var getViewLackBoarding = await _boardingService.GetViewLackBoarding(Model.PackingListId.Value, Model.Number.ToInt(), Model.Digit, Model.UnitEmission);
                    return getViewLackBoarding.Response;
                case LackTypeEnum.Landing:
                     var getViewLackLanding = await _landingService.GetViewLackLanding(Model.TrafficScheduleDetailId.Value, Model.Number.ToInt(), Model.Digit, Model.UnitEmission);
                    return getViewLackLanding.Response;
            }

            return null;
        }

        #endregion

        #region Commands

        private DelegateCommand _consultCommand;
        public DelegateCommand ConsultCommand =>
            _consultCommand ?? (_consultCommand = new DelegateCommand(ConsultCommandHandler));

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearCommandHandler));

        #endregion

        #region Command Handlers

        private async void ConsultCommandHandler()
        {
            if (Model.IsValid())
            {
                var packingListDetailViewInfo = await GetViewLack();
                if (packingListDetailViewInfo != null)
                {
                    var viewLackAmountModel = new ViewLackAmountModel()
                    {
                        PackingListDetailViewInfo = packingListDetailViewInfo,
                        Vehicle = ((!string.IsNullOrEmpty(Model.Vehicle))? Model.Vehicle:Model.UnitSent),
                        VehicleDescription = ((!string.IsNullOrEmpty(Model.VehicleDescription))? Model.VehicleDescription:Model.UnitSentDescription),
                        TrafficSchedule = Model.TrafficSchedule,
                        Line = Model.Line,
                        Date = Model.Date,
                        UnitDestination = Model.UnitDestination,
                        UnitDescription = Model.UnitDestinationDescription,
                        BillOfLading = Model.BillOfLading
                    };

                    await NavigationService.NavigateAsync("ViewLackAmountPage", new NavigationParameters()
                    {
                        {"ViewLackAmountModel", viewLackAmountModel}
                    });
                }
                else
                {
                    await _notificationService.NotifyAsync("CTRC inválido.", SoundEnum.Erros);
                }
            }
            else
            {
                var msg = ((string.IsNullOrEmpty(Model.Number))
                    ? "Nessesário informar o número."
                    : ((string.IsNullOrEmpty(Model.Digit))
                        ? "Nessesário informar o digito."
                        : ((string.IsNullOrEmpty(Model.UnitEmission)) 
                            ? "Nessesário informar a unidade de emissão."
                            : "Erro interno, informe a T.I.")));

                await _notificationService.NotifyAsync(msg, SoundEnum.Erros);
            }
        }

        private void ClearCommandHandler()
        {
            Model.ClearModel();
        }

        #endregion
    }
}
