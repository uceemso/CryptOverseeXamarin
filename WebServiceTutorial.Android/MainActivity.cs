using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CryptOverseeMobileApp.Droid
{
    [Activity(Label = "CryptOverseeMobileApp", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            Xamarin.Essentials.Platform.Init(this, savedInstanceState); // https://devblogs.microsoft.com/xamarin/persisting-settings-preferences-mobile-apps-xamarin-essentials/

            Rg.Plugins.Popup.Popup.Init(this); // https://github.com/rotorgames/Rg.Plugins.Popup/wiki/Getting-started

            // Remove the status bar and icons from top of screen
            //Window?.AddFlags(WindowManagerFlags.Fullscreen);
            //Window?.ClearFlags(WindowManagerFlags.ForceNotFullscreen);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Android: OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();

            LoadApplication(new App());
        }

        public override void OnBackPressed() // https://github.com/rotorgames/Rg.Plugins.Popup/wiki/Getting-started
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


    }
}