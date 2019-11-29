using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Models;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Models.Requests;
using TmsCollectorAndroid.Views.PopupPages;

namespace TmsCollectorAndroid.ViewModels
{
    public class MaintenanceSealsInputPageViewModel : ViewModelBase
    {
        public MaintenanceSealsInputPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService,
            IPopupNavigation popupNavigation,
            IBoardingAccessoryService boardingAccessoryService,
            IWifiService wifiService) 
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;
            _popupNavigation = popupNavigation;
            _boardingAccessoryService = boardingAccessoryService;
            _wifiService = wifiService;

            Model = new MaintenanceSealsInputModel();
        }

        private readonly INotificationService _notificationService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IBoardingAccessoryService _boardingAccessoryService;
        private readonly IWifiService _wifiService;

        private MaintenanceSealsInputModel _model;
        public MaintenanceSealsInputModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public Dictionary<string, object> CallBackData { get; private set; }

        #region Navigation Methods

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("Title", out string title))
            {
                Title = title;
            }
            else
            {
                Title = "Lacre";
            }

            if (parameters.TryGetValue("MaintenanceSealsInputModel", out MaintenanceSealsInputModel model))
            {
                model.SealFocus = Model.SealFocus;
                model.BtnConfirmationFocus = Model.BtnConfirmationFocus;
                Model = model;
                LoadSeals();
            }

            if (parameters.TryGetValue("CallBackData", out Dictionary<string, object> callBackData))
            {
                CallBackData = callBackData;
            }
        }

        #endregion

        #region Private Methods

        private async void LoadSeals()
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());

            var getSeals = await _boardingAccessoryService.GetSeals(Model.PackingListAccessoryId);

            await _popupNavigation.PopAllAsync();

            if (getSeals.Response != null && getSeals.Response.Seals.Any())
                Model.Seals = new ObservableCollection<string>(getSeals.Response.Seals); 

            if (!Model.OnlyConference)
                Model.LvSeals = new ObservableCollection<string>(Model.Seals);
        }

        private async Task<bool> RemoveAllSeals()
        {
            await _popupNavigation.PushAsync(new LoadingPopupPage());

            foreach (var seal in Model.Seals)
            {
                var removeSeal =
                    await _boardingAccessoryService.RemoveSeal(new RemoveSealModel(Model.PackingListAccessoryId, seal,
                        _wifiService.MacAddress));

                if (removeSeal.Response == null || !removeSeal.Response.Valid)
                {
                    await _popupNavigation.PopAllAsync();

                    return false;
                }
            }

            await _popupNavigation.PopAllAsync();

            return true;
        }

        #endregion

        #region Commands

        private DelegateCommand _sealChangedCommand;
        public DelegateCommand SealChangedCommand =>
            _sealChangedCommand ?? (_sealChangedCommand = new DelegateCommand(SealChangedCommandHandler));

        private DelegateCommand _confirmationCommand;
        public DelegateCommand ConfirmationCommand =>
            _confirmationCommand ?? (_confirmationCommand = new DelegateCommand(ConfirmationCommandHandler));

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommandHandler));

        #endregion

        #region Command Handlers

        private async void SealChangedCommandHandler()
        {
            if (!string.IsNullOrEmpty(Model.Seal))
            {
                if (!Model.OnlyConference 
                && !Model.Seals.Any(seal => seal.Trim().ToUpper().Equals(Model.Seal.Trim().ToUpper())))
                {
                    await _popupNavigation.PushAsync(new LoadingPopupPage());

                    var packingListViewInfo = await _boardingAccessoryService.AddSeal(
                        new AddSealModel(Model.PackingListAccessoryId, Model.Seal, _wifiService.MacAddress));

                    await _popupNavigation.PopAllAsync();

                    if (packingListViewInfo.Response != null && packingListViewInfo.Response.Valid)
                    {
                        Model.Seals.Add(Model.Seal);
                        Model.LvSeals.Add(Model.Seal);
                    }
                    else if (packingListViewInfo.Response != null)
                    {
                        await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage,
                            SoundEnum.Erros);
                    }
                }
                else if (!Model.OnlyConference
                && await _notificationService.AskQuestionAsync("Lacre já informado. Deseja remover?", 
                    SoundEnum.Alert))
                {
                    Model.Seal = Model.Seals.FirstOrDefault(seal => seal.Trim().ToUpper().Equals(Model.Seal.Trim().ToUpper()));


                    await _popupNavigation.PushAsync(new LoadingPopupPage());

                    var packingListViewInfo = await _boardingAccessoryService.RemoveSeal(
                        new RemoveSealModel(Model.PackingListAccessoryId, Model.Seal, _wifiService.MacAddress));

                    await _popupNavigation.PopAllAsync();

                    if (packingListViewInfo.Response != null && packingListViewInfo.Response.Valid)
                    {
                        Model.Seals.Remove(Model.Seal);
                        Model.LvSeals.Remove(Model.Seal);
                    }
                    else if (packingListViewInfo.Response != null)
                    {
                        await _notificationService.NotifyAsync(packingListViewInfo.Response.ExceptionMessage, SoundEnum.Erros);
                    }
                }
                else if (Model.OnlyConference)
                {
                    if (Model.Seals.Any(seal => seal.Trim().ToUpper().Equals(Model.Seal.Trim().ToUpper())))
                    {
                        if (!Model.LvSeals.Any(seal => seal.Trim().ToUpper().Equals(Model.Seal.Trim().ToUpper())))
                        {
                            Model.LvSeals.Add(Model.Seals.FirstOrDefault(seal => seal.Trim().ToUpper().Equals(Model.Seal.Trim().ToUpper())));
                            if (Model.Seals.Count == Model.LvSeals.Count)
                            {
                                Model.Seal = String.Empty;
                                Model.BtnConfirmationFocus();
                                return;
                            }
                        }
                        else if (await _notificationService.AskQuestionAsync("Lacre já informado. Deseja remover?", 
                            SoundEnum.Alert))
                        {
                            Model.LvSeals.Remove(Model.Seals.FirstOrDefault(seal => seal.Trim().ToUpper().Equals(Model.Seal.Trim().ToUpper())));
                        }
                    }
                    else
                    {
                        await _notificationService.NotifyAsync(
                            "Lacre informado não confere com o(s) lacre(s) do romaneio de acessório (unitização).",
                            SoundEnum.Erros);
                    }
                }

                Model.Seal = String.Empty;
                Model.SealFocus();
            }
        }

        private async void ConfirmationCommandHandler()
        {
            if ((Model.OnlyConference && Model.Seals.Count == Model.LvSeals.Count)
            || (!Model.OnlyConference && Model.Seals.Count == Model.TransportAccessoryDoors))
            {
                await NavigationService.GoBackAsync(new NavigationParameters()
                {
                    { "MaintenanceSealsInputConfirmed", true },
                    { "CallBackData", CallBackData }
                });
            }
            else if (Model.OnlyConference 
            && await _notificationService.AskQuestionAsync(
                "Quantidade de lacre(s) informado difere da quantidade de lacres do romaneio de acessório (unitização).\r\nConfirma cancelamento da operação?",
                SoundEnum.Alert))
            {
                CancelCommand.Execute();
            }
            else if (!Model.OnlyConference
             && await _notificationService.AskQuestionAsync(
                 "Quantidade de lacre(s) informado difere da quantidade de portas do acessório (unitização).\r\nConfirma cancelamento da operação?",
                 SoundEnum.Alert))
            {
                if (await RemoveAllSeals())
                {
                    CancelCommand.Execute();
                }
                else
                {
                    await _notificationService.NotifyAsync("Falha ao remover os lacres.", SoundEnum.Erros);
                }
            }
        }

        private async void CancelCommandHandler()
        {
            if (!Model.OnlyConference && Model.Seals.Any())
            {
                await _popupNavigation.PushAsync(new LoadingPopupPage());

                foreach (var seal in Model.Seals)
                {
                    var packingListViewInfo = await _boardingAccessoryService.RemoveSeal(
                        new RemoveSealModel(Model.PackingListAccessoryId, Model.Seal, _wifiService.MacAddress));

                    if (packingListViewInfo.Response != null && packingListViewInfo.Response.Valid)
                    {
                        Model.Seals.Remove(Model.Seal);
                        Model.LvSeals.Remove(Model.Seal);
                    }
                }

                await _popupNavigation.PopAllAsync();

                if (Model.Seals.Any())
                {
                    await _notificationService.NotifyAsync("Não foi possível remover todos os lacres, tente novamente ou comunique a T.I.",
                        SoundEnum.Alert);

                    return;
                }
            }

            await NavigationService.GoBackAsync(new NavigationParameters()
            {
                {"MaintenanceSealsInputConfirmed", false},
                {"CallBackData", CallBackData}
            });
        }

        #endregion
    }
}
