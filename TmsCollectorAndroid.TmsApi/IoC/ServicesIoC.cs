using Prism.Ioc;
using TmsCollectorAndroid.TmsApi.Interfaces.Services;
using TmsCollectorAndroid.TmsApi.Services;

namespace TmsCollectorAndroid.TmsApi.IoC
{
    public class ServicesIoC
    {
        public static void Register(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IServiceBase, ServiceBase>();
            containerRegistry.Register<ICommonService, CommonService>();
            containerRegistry.Register<IBillOfLadingService, BillOfLadingService>();
            containerRegistry.Register<IBoardingService, BoardingService>();
            containerRegistry.Register<IBoardingAccessoryService, BoardingAccessoryService>();
            containerRegistry.Register<ILandingService, LandingService>();
            containerRegistry.Register<ISorterService, SorterService>();
            containerRegistry.Register<IBoardingDeliveryPackService, BoardingDeliveryPackService>();
            containerRegistry.Register<ILandingDeliveryPackService, LandingDeliveryPackService>();
        }
    }
}