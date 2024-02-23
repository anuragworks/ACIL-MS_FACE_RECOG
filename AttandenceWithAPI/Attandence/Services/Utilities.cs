using Attandence.Models;
using Attandence.Views.Loader;
using Microsoft.ProjectOxford.Face;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Attandence.Services
{
    public static class Utilities
    {
        #region Get File Size
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public static string GetFileSize(Int64 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + GetFileSize(-value, decimalPlaces); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
        #endregion
        /// <summary>
        /// Image Compression
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <param name="Size"></param>
        /// <param name="QualityDecreased"></param>
        /// <returns></returns>
        public static string ImageCompression(string ImagePath, int SizeInBytes, int QualityDecreased, int ImageWidth, int ImageHeight)
        {
            byte[] Compressedimg = null;
            string CompressedImgPath = string.Empty;
            try
            {
                if (File.ReadAllBytes(ImagePath).Length > SizeInBytes)
                {
                    string aa = GetFileSize(File.ReadAllBytes(ImagePath).Length);
                    Compressedimg = DependencyService.Get<IImageCompressionService>().CmpressImage(File.ReadAllBytes(ImagePath), (100 - QualityDecreased), ImageWidth, ImageHeight);
                    CompressedImgPath = DependencyService.Get<IDeviceInformation>().PrivateExternalFolder + "/CropedCompressimg" + DateTime.Now.Ticks.ToString() + ".jpg";
                    File.WriteAllBytes(CompressedImgPath, Compressedimg);
                    string aaff = GetFileSize(File.ReadAllBytes(CompressedImgPath).Length);
                    ErrorLog.LogMessage("Compression Iteration : Width :" + (ImageWidth - 50).GetString() + " Image Size :" + GetFileSize(File.ReadAllBytes(CompressedImgPath).Length));
                    File.Delete(ImagePath);
                    if (File.ReadAllBytes(CompressedImgPath).Length > SizeInBytes)
                    {
                        return ImageCompression(CompressedImgPath, SizeInBytes, QualityDecreased, (ImageWidth - 50), (ImageHeight - 50));
                    }
                }
                else
                    return ImagePath;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("ImageCompression: " + ex.GetString());
            }
            return CompressedImgPath;
        }

        public static async Task ActivateLoaderAsync()
        {

            if (PopupNavigation.Instance.PopupStack.Where(n => n.Title.ToUpper() == "LOADERPAGE").FirstOrDefault() == null)
                await PopupNavigation.Instance.PushAsync(new Loader());
        }
        public static async Task DeactivateLoaderAsync()
        {

            PopupPage Loader = null;
            if (PopupNavigation.Instance.PopupStack.Any())
                Loader = PopupNavigation.Instance.PopupStack.Where(n => n.Title.ToUpper() == "LOADERPAGE").FirstOrDefault();
            if (Loader != null)
                await PopupNavigation.Instance.RemovePageAsync(Loader);
        }
        public static async Task<Status> RegisterUser(byte[] img, string Name)
        {
            Status oStatus = new Status();
            try
            {

                IFaceServiceClient faceServiceClient = new FaceServiceClient("faa71a2abab046c1a257664873a8e892", "https://eastus.api.cognitive.microsoft.com/face/v1.0/");
                string personGroupId = "pg1";
                // await faceServiceClient.CreatePersonGroupAsync(personGroupId, "Xamarin Employees7");

                // Step 2 - Add people to face list
                //foreach (var employee in Employees)
                //{
                var p = await faceServiceClient.CreatePersonAsync(personGroupId, Name);
                //await faceServiceClient.AddPersonFaceAsync(personGroupId, p.PersonId, "http://static4.businessinsider.com/image/559d359decad04574c42a3c4-480/xamarin-nat-friedman.jpg");
                await faceServiceClient.AddPersonFaceAsync(personGroupId, p.PersonId, new MemoryStream(img));
                //}

                // Step 3 - Train face group
                await faceServiceClient.TrainPersonGroupAsync(personGroupId);

                oStatus.Success = true;

            }
            catch (FaceAPIException ex)
            {
                ErrorLog.LogError(ex.GetString());
                oStatus.Success = false;
                oStatus.FailureMessage = ex.ErrorMessage;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.GetString());
                oStatus.Success = false;
                oStatus.FailureMessage = ex.Message;
            }
            return oStatus;
        }
        public static async Task<Status> DetectFace(byte[] img)
        {
            Status oStatus = new Status();
            string Persions = "";
            try
            {
                string personGroupId = "pg1";
                IFaceServiceClient faceServiceClient = new FaceServiceClient("faa71a2abab046c1a257664873a8e892", "https://eastus.api.cognitive.microsoft.com/face/v1.0/");
                var PersonGroup = await faceServiceClient.ListPersonGroupsAsync();
                var Person = await faceServiceClient.GetPersonsAsync(personGroupId);

                // Step 4 - Upload our photo and see who it is!
                //var faces = await faceServiceClient.DetectAsync("http://static4.businessinsider.com/image/559d359decad04574c42a3c4-480/xamarin-nat-friedman.jpg");
                var faces = await faceServiceClient.DetectAsync(new MemoryStream(img));
                var faceIds = faces.Select(face => face.FaceId).ToArray();

                var results = await faceServiceClient.IdentifyAsync(personGroupId, faceIds);
                foreach (var p in results)
                {
                    var person = p.Candidates[0].PersonId;
                    var personNames = await faceServiceClient.GetPersonAsync(personGroupId, person);
                    Persions += personNames.Name + " ,";
                }
                if (Persions.Length > 0)
                    Persions = Persions.Substring(0, Persions.Length - 1);
                oStatus.Success = true;
                oStatus.FailureMessage = Persions;

            }
            catch (FaceAPIException ex)
            {
                ErrorLog.LogError(ex.GetString());
                oStatus.Success = false;
                oStatus.FailureMessage = ex.ErrorMessage;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.GetString());
                oStatus.Success = false;
                oStatus.FailureMessage = ex.ToString();
            }
            return oStatus;
        }
        /// <summary>
        /// Get Geolocation
        /// </summary>
        /// <returns></returns>
        async public static Task<DeviceLocation> GetCurrentLocation()
        {
            DeviceLocation oDeviceLocation = new DeviceLocation();
            bool IsEnableLocationService = DependencyService.Get<ILocationService>().IsLocationServiceEnabled();
            PermissionStatus oLocationPermission = await LocationPermission.CheckLocationPermission();
            if (!IsEnableLocationService)
            {
                oDeviceLocation.Status = "Fail";
                oDeviceLocation.Message = "Please Enable Location";
                return oDeviceLocation;
            }
            else if (oLocationPermission == PermissionStatus.Denied)
            {
                oDeviceLocation.Status = "Fail";
                oDeviceLocation.Message = "Insufficient Permission";
                return oDeviceLocation;
            }
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(5));
                if (App.cts == null)
                    App.cts = new CancellationTokenSource();
                if (App.cts.IsCancellationRequested)
                    App.cts = new CancellationTokenSource();

                oDeviceLocation.location = await Geolocation.GetLocationAsync(request, App.cts.Token);

                var loc = await Geocoding.GetPlacemarksAsync(oDeviceLocation.location.Latitude, oDeviceLocation.location.Longitude);
                var aloc = loc?.FirstOrDefault();
                oDeviceLocation.Address = aloc.Locality + " " + aloc.AdminArea + " " + aloc.SubLocality;
                oDeviceLocation.Status = "Success";

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                ErrorLog.LogError(fnsEx.GetString());
                oDeviceLocation.Status = "Fail";
                oDeviceLocation.Message = "Feature not supported on this device";
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                ErrorLog.LogError(fneEx.GetString());
                oDeviceLocation.Status = "Fail";
                oDeviceLocation.Message = "Please enable device location and try again";
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                ErrorLog.LogError(pEx.GetString());
                oDeviceLocation.Status = "Fail";
                oDeviceLocation.Message = "Insufficient Permission";
            }
            catch (Exception ex)
            {
                // Unable to get location
                ErrorLog.LogError(ex.GetString());
                oDeviceLocation.Status = "Fail";
                oDeviceLocation.Message = "Unable to get location please try again";
            }
            return oDeviceLocation;
        }
    }
}
