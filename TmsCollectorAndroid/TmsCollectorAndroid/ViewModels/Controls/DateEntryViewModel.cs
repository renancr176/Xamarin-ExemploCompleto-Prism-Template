using Prism.Mvvm;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.ViewModels.Controls
{
    public class DateEntryViewModel : BindableBase
    {
        public DateEntryViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public readonly INotificationService _notificationService;
    }
}