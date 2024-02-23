using Attandence.Models;
using Attandence.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Attandence.ViewModels
{
	public class SettingsViewModel : ContentPage
    {
        public Command OnAppearingCommand { get; set; }
        IList<Item> _Lst;
        public IList<Item> Lst
        {
            get { return _Lst; }
            set
            {
                _Lst = value;
                OnPropertyChanged();
            }
        }
        private string _txtContent;
        public string txtContent
        {
            get { return _txtContent; }
            set
            {
                _txtContent = value;
                OnPropertyChanged(nameof(txtContent));
            }
        }
        private bool _IsEmpty;
        public bool IsEmpty
        {
            get { return _IsEmpty; }
            set
            {
                _IsEmpty = value;
                OnPropertyChanged(nameof(IsEmpty));
            }
        }
        private bool _isListViewEnable;
        public bool isListViewEnable
        {
            get { return _isListViewEnable; }
            set
            {
                _isListViewEnable = value;
                OnPropertyChanged(nameof(isListViewEnable));
            }
        }
        Item _SelectedListItem;

        public Item SelectedListItem
        {
            get { return _SelectedListItem; }
            set
            {
                if (_SelectedListItem != value)
                {

                    _SelectedListItem = value;
                    isListViewEnable = false;
                    string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _SelectedListItem.Id);
                    ErrorLog.LogDeviceDetail(file);
                    Share.RequestAsync(new ShareFileRequest
                    {
                        Title = _SelectedListItem.Id,
                        File = new ShareFile(file)
                    });
                    //UploadLogs(file);
                    isListViewEnable = true;
                    //_SelectedListItem = null;
                    OnPropertyChanged(nameof(SelectedListItem));
                }
            }
        }
        public SettingsViewModel()
        {
            OnAppearingCommand = new Command(() => OnAppearing());
           
        }
        private void OnAppearing()
        {
            isListViewEnable = true;
            IsEmpty = true;
            List<Item> oLst = new List<Item>();
            foreach (var file in System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))
            {
                if (file.Contains(".txt"))
                {
                    IsEmpty = false;
                    oLst.Add(new Item() { Id = Path.GetFileName(file), Text = Path.GetFileName(file) });
                }
            }
            Lst = oLst.OrderByDescending(n => Convert.ToInt64(Path.GetFileName(n.Text).Split('_')[0] + Path.GetFileName(n.Text).Split('_')[1] + Path.GetFileName(n.Text).Split('_')[2].Split('.')[0])).ToList();
        }

    }
}