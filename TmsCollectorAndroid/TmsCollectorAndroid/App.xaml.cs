using System.Globalization;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using TmsCollectorAndroid.IoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TmsCollectorAndroid
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");

            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();

            BootStrapperIoC.Register(containerRegistry);

            App.Current.Resources.Add("IoC", containerRegistry.GetContainer());
        }
    }
}
