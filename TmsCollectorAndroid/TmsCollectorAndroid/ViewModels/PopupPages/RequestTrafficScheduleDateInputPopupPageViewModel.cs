using System;
using Prism.Commands;
using Prism.Navigation;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.ViewModels.PopupPages
{
    public class RequestTrafficScheduleDateInputPopupPageViewModel : ViewModelBase
    {
        public RequestTrafficScheduleDateInputPopupPageViewModel(INavigationService navigationService,
            IBarcodeReaderService barcodeReaderService,
            IStatusBarService statusBarService,
            INotificationService notificationService)
            : base(navigationService, barcodeReaderService, statusBarService)
        {
            _notificationService = notificationService;

            Text = "Escala";
        }

        private readonly INotificationService _notificationService;

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("Text", out string text))
                Text = text;
            if (parameters.TryGetValue("TrafficScheduleDateTime", out DateTime trafficScheduleDateTime))
                TrafficScheduleDateTime = trafficScheduleDateTime;
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private DateTime _trafficScheduleDateTime;
        public DateTime TrafficScheduleDateTime
        {
            get { return _trafficScheduleDateTime; }
            set { SetProperty(ref _trafficScheduleDateTime, value); }
        }

        private string _date;
        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private string _time;
        public string Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        #region Commands
        
        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelCommandHandler));

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand =>
            _confirmCommand ?? (_confirmCommand = new DelegateCommand(ConfirmCommandHandler));

        #endregion

        #region Comman Handlers
        
        private async void CancelCommandHandler()
        {
            await NavigationService.GoBackAsync(
                new NavigationParameters() { { "TrafficScheduleDateConfirmed", false } });
        }

        private async void ConfirmCommandHandler()
        {
            if (Date.IsDateFormated() && DateTime.TryParse(Date, out DateTime date)
            && Time.IsTimeFormated() && TimeSpan.TryParse(Time, out TimeSpan time))
            {
                date = date.Add(time);
                if (date.Equals(TrafficScheduleDateTime))
                {
                    await NavigationService.GoBackAsync(new NavigationParameters() {{ "TrafficScheduleDateConfirmed", true}});
                }
                else
                {
                    await _notificationService.NotifyAsync(
                        $"Data digitada {date.ToString("dd/MM/yyyy HH:mm")} não é igual a Data da Escala.",
                        SoundEnum.Erros);
                }
            }
            else
            {
                await _notificationService.NotifyAsync("Informe a data e hora.", SoundEnum.Erros);
            }
        }

        #endregion
    }
}