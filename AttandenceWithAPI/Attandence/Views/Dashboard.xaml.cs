using Attandence.ViewModels;
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
    public partial class Dashboard : ContentPage
    {
        public Dashboard()
        {
            InitializeComponent();
            BindingContext = new DashboardViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((DashboardViewModel)this.BindingContext).OnAppearingCommand.Execute(null);
        }
    }
}