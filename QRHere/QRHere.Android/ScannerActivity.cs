﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZXing;
using ZXing.Mobile;

namespace QRHere.Droid
{
    [Activity(Label = "ScannerActivity")]
    public class ScannerActivity : Activity
    {
        Button buttonScanCustomView;
        Button buttonScanDefaultView;
        Button buttonContinuousScan;
        Button buttonFragmentScanner;
        Button buttonGenerate;

        MobileBarcodeScanner scanner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            MobileBarcodeScanner.Initialize(Application);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.QrScanner);

            buttonScanDefaultView = this.FindViewById<Button>(Resource.Id.buttonScanDefaultView);
            buttonScanDefaultView.Click += async delegate {

                //Tell our scanner to use the default overlay
                scanner.UseCustomOverlay = false;

                //We can customize the top and bottom text of the default overlay
                scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
                scanner.BottomText = "Wait for the barcode to automatically scan!";

                //Start scanning
                var result = await scanner.Scan();

                HandleScanResult(result);
            };

            buttonContinuousScan = FindViewById<Button>(Resource.Id.buttonScanContinuous);
            buttonContinuousScan.Click += delegate {

                scanner.UseCustomOverlay = false;

                //We can customize the top and bottom text of the default overlay
                scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
                scanner.BottomText = "Wait for the barcode to automatically scan!";

                var opt = new MobileBarcodeScanningOptions();
                opt.DelayBetweenContinuousScans = 3000;

                //Start scanning
                scanner.ScanContinuously(opt, HandleScanResult);
            };

            Button flashButton;
            View zxingOverlay;

            buttonScanCustomView = this.FindViewById<Button>(Resource.Id.buttonScanCustomView);
            buttonScanCustomView.Click += async delegate {

                //Tell our scanner we want to use a custom overlay instead of the default
                scanner.UseCustomOverlay = true;

                //Inflate our custom overlay from a resource layout
                zxingOverlay = LayoutInflater.FromContext(this).Inflate(Resource.Layout.ZxingOverlay, null);

                //Find the button from our resource layout and wire up the click event
                flashButton = zxingOverlay.FindViewById<Button>(Resource.Id.buttonZxingFlash);
                flashButton.Click += (sender, e) => scanner.ToggleTorch();

                //Set our custom overlay
                scanner.CustomOverlay = zxingOverlay;

                //Start scanning!
                var result = await scanner.Scan(new MobileBarcodeScanningOptions { AutoRotate = true });

                HandleScanResult(result);
            };

            buttonFragmentScanner = FindViewById<Button>(Resource.Id.buttonFragment);
            buttonFragmentScanner.Click += delegate {
                StartActivity(typeof(FragmentActivity));
            };

            buttonGenerate = FindViewById<Button>(Resource.Id.buttonGenerate);
            buttonGenerate.Click += delegate {
                StartActivity(typeof(ImageActivity));
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (ZXing.Net.Mobile.Android.PermissionsHandler.NeedsPermissionRequest(this))
                ZXing.Net.Mobile.Android.PermissionsHandler.RequestPermissionsAsync(this);
        }

        void HandleScanResult(ZXing.Result result)
        {
            string msg = "";

            if (result != null && !string.IsNullOrEmpty(result.Text))
                msg = "Found Barcode: " + result.Text;
            else
                msg = "Scanning Canceled!";

            this.RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Short).Show());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [Java.Interop.Export("UITestBackdoorScan")]
        public Java.Lang.String UITestBackdoorScan(string param)
        {
            var expectedFormat = BarcodeFormat.QR_CODE;
            Enum.TryParse(param, out expectedFormat);
            var opts = new MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<BarcodeFormat> { expectedFormat }
            };
            var barcodeScanner = new MobileBarcodeScanner();

            Console.WriteLine("Scanning " + expectedFormat);

            //Start scanning
            barcodeScanner.Scan(opts).ContinueWith(t => {

                var result = t.Result;

                var format = result?.BarcodeFormat.ToString() ?? string.Empty;
                var value = result?.Text ?? string.Empty;

                RunOnUiThread(() => {

                    AlertDialog dialog = null;
                    dialog = new AlertDialog.Builder(this)
                                    .SetTitle("Barcode Result")
                                    .SetMessage(format + "|" + value)
                                    .SetNeutralButton("OK", (sender, e) => {
                                        dialog.Cancel();
                                    }).Create();
                    dialog.Show();
                });
            });

            return new Java.Lang.String();
        }
    }
}