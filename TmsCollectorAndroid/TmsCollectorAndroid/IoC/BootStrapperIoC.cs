using Prism.Ioc;
using Prism.Plugin.Popups;
using Prism.Unity;
using TmsCollectorAndroid.Interfaces.Services;
using Unity;

namespace TmsCollectorAndroid.IoC
{
    public class BootStrapperIoC
    {
        public static void Register(IContainerRegistry containerRegistry)
        {
            MvvmIoC.Register(containerRegistry);
            ServicesIoC.Register(containerRegistry);
            containerRegistry.RegisterPopupNavigationService();

            #region External Services

            #region TmsAPI

            TmsApi.IoC.ServicesIoC.Register(containerRegistry);
            var env = containerRegistry.GetContainer().Resolve<IEnvironmentConfigurationService>();
            var tmsApiServiceBase = containerRegistry.GetContainer().Resolve<TmsApi.Interfaces.Services.IServiceBase>();
            tmsApiServiceBase.SetApiUrl(env.Configuration.ApiUrls);

            #endregion

            #endregion
        }
    }
}