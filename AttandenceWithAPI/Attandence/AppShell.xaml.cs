using Attandence.ViewModels;
using Attandence.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Attandence
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PunchIN), typeof(PunchIN));
            Routing.RegisterRoute(nameof(RegisterUser), typeof(RegisterUser));
            Routing.RegisterRoute(nameof(AppLogs), typeof(AppLogs));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {

        }
    }
}
