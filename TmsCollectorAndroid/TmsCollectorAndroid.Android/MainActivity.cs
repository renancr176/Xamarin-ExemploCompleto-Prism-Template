using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;
using TmsCollectorAndroid.Droid.Service;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.Droid
{
    [Activity(Label = "Coletor", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ICurrentActivity>(CrossCurrentActivity.Current);
            containerRegistry.RegisterSingleton<IBarcodeReaderService, BarcodeReaderService>();
            containerRegistry.Register<IStatusBarService, StatusBarService>();
            containerRegistry.Register<ITextToSpeechService, TextToSpeechService>();
            containerRegistry.Register<IWifiService, WifiService>();
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            CrossCurrentActivity.Current.Init(this, bundle);

            base.OnCreate(bundle);

            Rg.Plugins.Popup.Popup.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(this));
        }
    }
}

