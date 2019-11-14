using Prism.Mvvm;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.ViewModels.Controls
{
    public class TimeEntryViewModel : BindableBase
    {
        public TimeEntryViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public readonly INotificationService _notificationService;
    }
}