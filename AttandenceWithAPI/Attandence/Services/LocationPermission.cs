using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Attandence.Services
{
    public static class LocationPermission
    {
        public async static Task<bool> IsLocationPermissionGranted()
        {
            if (await Permissions.CheckStatusAsync<Permissions.LocationAlways>() == PermissionStatus.Granted ||
                await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>() == PermissionStatus.Granted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async static Task<PermissionStatus> CheckLocationPermission()
        {
            return await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        }
    }
}
