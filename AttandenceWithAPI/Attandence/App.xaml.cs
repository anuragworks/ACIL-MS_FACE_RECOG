using Attandence.Services;
using Attandence.Views;
using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Attandence
{
    public partial class App : Application
    {
        public static CancellationTokenSource cts = null;
        public App()
        {
            InitializeComponent();
            cts = new CancellationTokenSource();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
        }

        protected override void OnResume()
        {
            cts = new CancellationTokenSource();
        }
    }
}
