using Android.App;
using Android.OS;
using System.Threading;
using CryptOverseeMobileApp.Droid;

namespace WebServiceTutorial.Droid
{
    //Set MainLauncher = true makes this Activity Shown First on Running this Application  
    //Theme property set the Custom Theme for this Activity  
    //No History= true removes the Activity from BackStack when user navigates away from the Activity  
    [Activity(Label = "CryptOversee", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            StartActivity(typeof(MainActivity));
        }


    }
}