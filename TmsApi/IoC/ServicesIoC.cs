using Prism.Ioc;
using TmsCollectorAndroid.Api_Old.Interfaces.Services;
using TmsCollectorAndroid.Api_Old.Services;

namespace TmsCollectorAndroid.Api_Old.IoC
{
    public class ServicesIoC
    {
        public static void Register(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IServiceBase, ServiceBase>();
            containerRegistry.Register<ICommonService, CommonService>();
        }
    }
}