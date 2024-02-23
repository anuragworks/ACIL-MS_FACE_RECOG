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
    public partial class RegisterUser : ContentPage
    {
        public RegisterUser()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            XctCam.Shutter();
        }

        private async void XctCam_MediaCaptured(object sender, Xamarin.CommunityToolkit.UI.Views.MediaCapturedEventArgs e)
        {
            Status status = null;
            byte[] CompressedimgArr = null;
            await Utilities.ActivateLoaderAsync();
            await Task.Run(async () =>
            {
                try
                {
                    Stream stream = new MemoryStream(e.ImageData);

                    string ImgFile = DependencyService.Get<IDeviceInformation>().PrivateExternalFolder + "/skia" + DateTime.Now.Ticks.ToString() + ".jpg";
                    using (var fileStream = new FileStream(ImgFile, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                    string Compressedimg = Utilities.ImageCompression(ImgFile, 307200, 15, 1024, 1024);

                     CompressedimgArr = DependencyService.Get<IImageCompressionService>().ResizeAndRotate(Compressedimg, 1024, 1024, 15,270);
                    //string size = Utilities.GetFileSize(File.ReadAllBytes(Compressedimg).Length);

                     status = await Utilities.RegisterUser(CompressedimgArr, txtName.Text);
                }
                catch (Exception ex)
                {
                    ErrorLog.LogError("RegisterUser: " + ex.ToString());
                }
                Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Utilities.DeactivateLoaderAsync();
                        img.Source = ImageSource.FromStream(() => new MemoryStream(CompressedimgArr));
                        if (status.Success)
                        {
                            await App.Current.MainPage.DisplayAlert("Register", "User registered", "OK");
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Register", status.FailureMessage, "OK");
                        }
                    });

            });
        }

    }
}