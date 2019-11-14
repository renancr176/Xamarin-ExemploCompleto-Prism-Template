using Android.Views;
using Plugin.CurrentActivity;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.Droid.Service
{
    public class StatusBarService : IStatusBarService
    {
        public WindowManagerLayoutParams DefaultAttributes { get; private set; }

        private readonly ICurrentActivity _currentActivity;

        public StatusBarService(ICurrentActivity currentActivity)
        {
            _currentActivity = currentActivity;
            DefaultAttributes = currentActivity.Activity.Window.Attributes;
        }
        
        public void HideStatusBar()
        {
            var attrs = DefaultAttributes;
            attrs.Flags |= WindowManagerFlags.Fullscreen;
            _currentActivity.Activity.Window.Attributes = attrs;
        }

        public void ResetToDefaultStatusBar()
        {
            _currentActivity.Activity.Window.Attributes = DefaultAttributes;
        }
    }
}