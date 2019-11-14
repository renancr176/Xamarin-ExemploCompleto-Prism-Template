using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Navigation;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;

namespace TmsCollectorAndroid.ViewModels
{
    public class ProcessTypeMenuPageViewModel : ViewModelBase
    {
        public ProcessTypeMenuPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            IUserService userService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _userService = userService;

            Title = "PROCESSOS";

            SetMenu();
        }

        private readonly IUserService _userService;

        private ObservableCollection<ProcessoModel> _lvProcessos;
        public ObservableCollection<ProcessoModel> LvProcessos
        {
            get { return _lvProcessos; }
            set { SetProperty(ref _lvProcessos, value); }
        }

        private void SetMenu()
        {
            LvProcessos = new ObservableCollection<ProcessoModel>()
            {
                new ProcessoModel(ProcessoEnum.Embarque,"PackingListBoardingInputPage"),
                new ProcessoModel(ProcessoEnum.Desembarque,"PackingListLandingInputPage"),
                new ProcessoModel(ProcessoEnum.EtiquetaMae,"PackingListAccessoriesInputPage"),
                new ProcessoModel(ProcessoEnum.ConsultaRapida,"BillOfLadingInformationsPage"),
                new ProcessoModel(ProcessoEnum.EntregaEmbarque,"PackingListDeliveryBoardingInputPage"),
                new ProcessoModel(ProcessoEnum.EntregaRetorno,"PackingListDeliveryReturnInputPage")
            };

            if (_userService.User.Unit.IsJointUnit)
            {
                LvProcessos.Add(new ProcessoModel(ProcessoEnum.LeituraUnica, "PackingListBoardingInputPage"));
            }

            if (_userService.User.Unit.IsUniversalLanding)
            {
                LvProcessos.Add(new ProcessoModel(ProcessoEnum.DesembarqueUniversal, "UniversalLandingPage"));
            }
        }

        #region Commands
        
        private DelegateCommand _voltarCommand;
        public DelegateCommand VoltarCommand =>
            _voltarCommand ?? (_voltarCommand = new DelegateCommand(VoltarCommandHandler));

        private DelegateCommand<ProcessoModel> _goToProcesso;
        public DelegateCommand<ProcessoModel> GoToProcessoCommand =>
            _goToProcesso ?? (_goToProcesso = new DelegateCommand<ProcessoModel>(GoToProcessoCommandHandler));

        #endregion

        #region Command Handlers

        void VoltarCommandHandler()
        {
            _userService.LogOut();
        }

        void GoToProcessoCommandHandler(ProcessoModel processo)
        {
            if (processo != null && !string.IsNullOrEmpty(processo.Pagina))
            {
                NavigationService.NavigateAsync(processo.Pagina, processo.NavigationParameters);
            }
        }
        
        #endregion
    }
}
