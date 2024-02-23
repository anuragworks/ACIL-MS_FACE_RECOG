using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Attandence.Services
{
    public class DeviceLocation
    {
        public Location location { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }

    }
}
