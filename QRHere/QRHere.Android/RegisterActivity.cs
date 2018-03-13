using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace QRHere.Droid
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        EditText usertxt;
        EditText passtxt;
        Button registerbtn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegisterActivity);

            usertxt = FindViewById<EditText>(Resource.Id.usertxt);
            passtxt = FindViewById<EditText>(Resource.Id.passtxt);
            registerbtn = FindViewById<Button>(Resource.Id.registerBtn);

            registerbtn.Click += registerbtn_Click;


            // Create your application here
        }

        private void registerbtn_Click(object sender, EventArgs e)
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "UsuariosDB.db3");
                var db = new SQLiteConnection(dpPath);
                db.CreateTable<Usuarios>();
                Usuarios tbl = new Usuarios();
                tbl.Username = usertxt.Text;
                tbl.Password = passtxt.Text;
                db.Insert(tbl);
                Toast.MakeText(this, "Record Added Successfully...,", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }
    }
}