using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Attandence.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            string Password = txtPassword.Text;
            if (UserName == "Test" && Password == "Test@12345")
            {
                await Shell.Current.GoToAsync($"//{nameof(Dashboard)}");
            }

            else
            {
                await App.Current.MainPage.DisplayAlert("Login", "Invalid Username and password", "OK");
                return;
            }

        }
    }
}