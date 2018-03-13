using System;

using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using SQLite;
using System.IO;

namespace QRHere.Droid
{
    [Activity(Label = "QRHere", Icon = "@drawable/qrico", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Button button, registerbtn;
        EditText usertxt, passtxt;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            
            base.OnCreate(bundle);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            SetContentView(Resource.Layout.LoginPage);

            usertxt = FindViewById<EditText>(Resource.Id.usertxt);
            passtxt = FindViewById<EditText>(Resource.Id.passtxt);
            button = FindViewById<Button>(Resource.Id.enterButton);
            registerbtn = FindViewById<Button>(Resource.Id.registerBtn);

            button.Click += enterButton_Click;
            

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            

        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "UsuariosDB.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                db.CreateTable<Usuarios>();
                var data = db.Table<Usuarios>(); //Call Table  
                var data1 = data.Where(x => x.Username == usertxt.Text && x.Password == passtxt.Text).FirstOrDefault(); //Linq Query  
                if (data1 != null)
                {
                    Toast.MakeText(this, "Login Success", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Username or Password invalid", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }

        
    }
}

