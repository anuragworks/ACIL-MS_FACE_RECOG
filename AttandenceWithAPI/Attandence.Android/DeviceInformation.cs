using Android.App;
using Android.Content;
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
using Xamarin.Forms;
using Attandence.Services;
using static Android.Provider.Settings;

[assembly: Dependency(typeof(DeviceInformation))]
namespace Attandence.Droid
{
    public class DeviceInformation : IDeviceInformation
    {
        public DeviceInformation() { }

        public string GetDeviceID()
        {
            var context = Android.App.Application.Context;
            string id = Secure.GetString(context.ContentResolver, Secure.AndroidId);
            return id;
        }
      
        public string PrivateExternalFolder
        {
            get
            {
                return Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            }
        }
    }
}