using Prism.Ioc;
using TmsCollectorAndroid.Interfaces.Services;
using TmsCollectorAndroid.Services;

namespace TmsCollectorAndroid.IoC
{
    public class ServicesIoC
    {
        public static void Register(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IEnvironmentConfigurationService, EnvironmentConfigurationService>();
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.Register<ISoundService, SoundService>();
            containerRegistry.Register<INotificationService, NotificationService>();
            containerRegistry.RegisterSingleton<ILabelValidationService, LabelValidationService>();
        }
    }
}