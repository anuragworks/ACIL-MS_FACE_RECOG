using Attandence.Models;
using Attandence.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Attandence.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PunchIN : ContentPage
    {
        public PunchIN()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            XctCam.Shutter();
        }

        private async void XctCam_MediaCaptured(object sender, Xamarin.CommunityToolkit.UI.Views.MediaCapturedEventArgs e)
        {
            DeviceLocation location = null;
            await Utilities.ActivateLoaderAsync();
            string Name, Compressedimg = string.Empty;
            byte[] CompressedimgArr = null;
            Status oStatus = null;
            await Task.Run(async () =>
            {
                try
                {
                    location = await Utilities.GetCurrentLocation();
                    ErrorLog.LogError("Start Time: " + DateTime.Now);
                    Stream stream = new MemoryStream(e.ImageData);
                    string ImgFile = DependencyService.Get<IDeviceInformation>().PrivateExternalFolder + "/skia" + DateTime.Now.Ticks.ToString() + ".jpg";
                    using (var fileStream = new FileStream(ImgFile, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                    Compressedimg = Utilities.ImageCompression(ImgFile, 307200, 15, 1024, 1024);
                    //string size = Utilities.GetFileSize(File.ReadAllBytes(Compressedimg).Length);
                    if (XctCam.CameraOptions == Xamarin.CommunityToolkit.UI.Views.CameraOptions.Front)
                        CompressedimgArr = DependencyService.Get<IImageCompressionService>().ResizeAndRotate(Compressedimg, 1024, 1024, 15, 270);
                    else
                        CompressedimgArr = DependencyService.Get<IImageCompressionService>().ResizeAndRotate(Compressedimg, 1024, 1024, 15, 90);

                    oStatus = await Utilities.DetectFace(CompressedimgArr);
                    ErrorLog.LogError("End Time: " + DateTime.Now);

                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("PunchIN: ", ex.ToString(), "OK");
                }
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Utilities.DeactivateLoaderAsync();
                    img.Source = ImageSource.FromStream(() => new MemoryStream(CompressedimgArr));
                    if (location.Status == "Fail")
                    {
                        await App.Current.MainPage.DisplayAlert("Attandence: ", location.Message, "OK");
                    }
                    else if (oStatus.Success && location.Status == "Success")
                    {
                        await App.Current.MainPage.DisplayAlert("Attandence: ", "Persons: " + oStatus.FailureMessage + "\nLocation: " + location.Address, "OK");
                    }
                    else if (oStatus.Success)
                        await App.Current.MainPage.DisplayAlert("Attandence: ", oStatus.FailureMessage, "OK");
                    else
                        await App.Current.MainPage.DisplayAlert("Attandence: ", oStatus.FailureMessage, "OK");
                });
            });
            //await Task.Run(async () =>
            //{
            //    try
            //    {
            //        ErrorLog.LogError("Start Time: " + DateTime.Now);
            //            //Stream stream = new MemoryStream(e.ImageData);
            //            string ImgFile = DependencyService.Get<IDeviceInformation>().PrivateExternalFolder + "/a.jpg";
            //        MemoryStream ms = new MemoryStream();
            //        using (FileStream file = new FileStream(ImgFile, FileMode.Open, FileAccess.Read))
            //            file.CopyTo(ms);
            //            //Compressedimg = Utilities.ImageCompression(ImgFile, 307200, 15, 1024, 1024);

            //            //string size = Utilities.GetFileSize(File.ReadAllBytes(Compressedimg).Length);
            //            //CompressedimgArr = DependencyService.Get<IImageCompressionService>().ResizeAndRotate(Compressedimg, 1024, 1024, 15);
            //            oStatus = await Utilities.DetectFace(ms.ToArray());
            //        ErrorLog.LogError("End Time: " + DateTime.Now);

            //    }
            //    catch (Exception ex)
            //    {
            //        await App.Current.MainPage.DisplayAlert("PunchIN: ", ex.ToString(), "OK");
            //    }
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await Utilities.DeactivateLoaderAsync();
            //        img.Source = ImageSource.FromStream(() => new MemoryStream(CompressedimgArr));
            //        if (oStatus.Success)
            //            await App.Current.MainPage.DisplayAlert("Attandence: ", oStatus.FailureMessage, "OK");
            //        else
            //            await App.Current.MainPage.DisplayAlert("Attandence: ", oStatus.FailureMessage, "OK");
            //    });
            //});
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (XctCam.CameraOptions == Xamarin.CommunityToolkit.UI.Views.CameraOptions.Back)
                XctCam.CameraOptions = Xamarin.CommunityToolkit.UI.Views.CameraOptions.Front;
            else
                XctCam.CameraOptions = Xamarin.CommunityToolkit.UI.Views.CameraOptions.Back;
        }
    }
}