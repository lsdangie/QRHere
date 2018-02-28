using System;

using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace QRHere.Droid
{
    [Activity(Label = "QRHere", Icon = "@drawable/qrico", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Button button;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            
            base.OnCreate(bundle);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            SetContentView(Resource.Layout.LoginPage);

            button = FindViewById<Button>(Resource.Id.enterButton);
            button.Click += delegate {
                StartActivity(typeof(ScannerActivity));
            };


            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            
        }
    }
}

