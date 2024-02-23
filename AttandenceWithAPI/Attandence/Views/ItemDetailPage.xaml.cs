using Attandence.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Attandence.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}