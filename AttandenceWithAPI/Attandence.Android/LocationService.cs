using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Attandence.Droid;
using Attandence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
[assembly: Xamarin.Forms.Dependency(typeof(LocationService))]
namespace Attandence.Droid
{
    class LocationService : ILocationService
    {
        public bool IsLocationServiceEnabled()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);

            try
            {
                return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
            }
            catch (Exception)
            {
                return false;
            }

        }

        //public bool OpenDeviceLocationSettingsPage()
        //{
        //    var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
        //    intent.AddFlags(ActivityFlags.NewTask);
        //    Android.App.Application.Context.StartActivity(intent);
        //    return true;
        //}
    }
}