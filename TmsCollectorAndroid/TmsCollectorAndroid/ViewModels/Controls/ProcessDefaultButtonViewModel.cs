using Prism.Mvvm;
using Prism.Services;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.ViewModels.Controls
{
    public class ProcessDefaultButtonViewModel : BindableBase
    {
        public ProcessDefaultButtonViewModel(IPageDialogService dialogService,
            INotificationService notificationService,
            IUserService userService)
        {
            _dialogService = dialogService;
            _notificationService = notificationService;
            _userService = userService;
        }

        public readonly IPageDialogService _dialogService;
        public readonly INotificationService _notificationService;
        public readonly IUserService _userService;
    }
}