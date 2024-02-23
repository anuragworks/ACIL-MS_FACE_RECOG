using System;
using System.Collections.Generic;
using System.Text;

namespace Attandence.Services
{
    public interface IDeviceInformation
    {
        string GetDeviceID();
        string PrivateExternalFolder { get; }
    }
}
