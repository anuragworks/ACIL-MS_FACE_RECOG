using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Attandence.Services
{
    public class ErrorLog
    {
        static string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CommomnExtension.GetTodayDateYYYY_MM_DD() + ".txt");
        public static void LogError(string logMessage)
        {
            try
            {
                StreamWriter sw = new StreamWriter(LogFile, true);

                sw.WriteLine("Date Time: " + DateTime.Now.GetString());
                sw.WriteLine("TimeZone: " + TimeZoneInfo.Local.DisplayName.GetString());
                sw.WriteLine("Exception: " + logMessage);
                sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------");
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        public static void LogMessage(string logMessage)
        {
            try
            {
                StreamWriter sw = new StreamWriter(LogFile, true);

                sw.WriteLine("Date Time: " + DateTime.Now.GetString());
                sw.WriteLine("TimeZone: " + TimeZoneInfo.Local.DisplayName.GetString());
                sw.WriteLine("Message: " + logMessage);
                sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------");
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        public static void LogDeviceDetail(string filePath)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filePath, true);

                sw.WriteLine("Date Time: " + DateTime.Now.GetString());
                //sw.WriteLine("AppVersion: " + AppInfo.VersionString);
                sw.WriteLine("TimeZone: " + TimeZoneInfo.Local.DisplayName.ToString());
                //sw.WriteLine("DeviceModel: " + CrossDeviceInfo.Current.Model);
                //sw.WriteLine("Manufacturer: " + CrossDeviceInfo.Current.Manufacturer);
                //sw.WriteLine("DeviceName: " + CrossDeviceInfo.Current.DeviceName);
                //sw.WriteLine("Platfrom: " + CrossDeviceInfo.Current.Platform.GetString() + " " + CrossDeviceInfo.Current.VersionNumber.GetString());
                //sw.WriteLine("Idiom: " + CrossDeviceInfo.Current.Idiom.GetString());
                sw.WriteLine("DeviceType: Physical");
                sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------");
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

    }
}
