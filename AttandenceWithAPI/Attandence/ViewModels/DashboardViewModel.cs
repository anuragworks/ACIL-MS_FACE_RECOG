using Attandence.Models;
using Attandence.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Attandence.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public Command OnAppearingCommand { get; set; }
        private List<DashboardDetails> lstItems;

        public List<DashboardDetails> ItemList
        {
            get { return lstItems; }
            set { lstItems = value; OnPropertyChanged(nameof(ItemList)); }
        }

        private bool IsClicked { get; set; } = false;
        DashboardDetails _SelectedListItem;
        public DashboardDetails SelectedListItem
        {
            get { return _SelectedListItem; }
            set
            {
                if (_SelectedListItem != value)
                {
                    try
                    {
                        if (!IsClicked && value != null)
                        {
                            _SelectedListItem = value;
                            NavigatetoPages(_SelectedListItem.Id);
                            OnPropertyChanged(nameof(SelectedListItem));
                        }
                    }
                    catch { }
                }
            }
        }
        private async void NavigatetoPages(int PageID)
        {
            IsClicked = true;
            switch (PageID)
            {
                case 1:
                    NavigateToBillHistory();
                    break;
                case 2:
                    NavigateToGenerateBill();
                    break;

                //case 7:
                //    NavigateToEKYC();
                //    break;
                default:
                    IsClicked = false;
                    break;
            }
        }
        public DashboardViewModel()
        {
            OnAppearingCommand = new Command(() => OnAppearing());



            List<DashboardDetails> odb = new List<DashboardDetails>()
            {
                new DashboardDetails() { Id = 1, Image = "BillHistory.png", Title = "Register User", Description = "Register User",TabColor=Color.Gray },
                new DashboardDetails() { Id = 2, Image = "Finger.png", Title = "Attandence", Description = "Attandence",TabColor=Color.Gray }
                //new DashboardDetails() { Id = 3, Image = "PaymentHistory.png", Title = "Payment History", Description = "Payment History",TabColor=Color.Gray},
                //new DashboardDetails() { Id = 4, Image = "Payments.png", Title = "Make Payments", Description = "Make Payments",TabColor=Color.Gray },
                //new DashboardDetails() { Id = 6, Image = "AddConsumers.png", Title = "Add Consumers", Description = "Add Consumers",TabColor=Color.Gray },
                //new DashboardDetails() { Id = 5, Image = "HelpDesk.png", Title = "Help Desk", Description = "Help Desk",TabColor=Color.Gray }
                //new DashboardDetails() { Id = 7, Image = "HelpDesk.png", Title = "E-KYC", Description = "E-KYC" }
            };

            ItemList = odb;
        }
        private void OnAppearing()
        {
            IsClicked = false;
            SelectedListItem = new DashboardDetails();
        }
        public async void NavigateToBillHistory()
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterUser)}");
        }
        public async void NavigateToGenerateBill()
        {
            await Shell.Current.GoToAsync($"{nameof(PunchIN)}");
        }

    }
}