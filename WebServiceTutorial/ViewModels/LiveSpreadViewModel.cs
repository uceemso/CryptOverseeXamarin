using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CryptOverseeMobileApp.Models;
using Xamarin.Forms;

namespace CryptOverseeMobileApp.ViewModels
{
    public class LiveSpreadViewModel : INotifyPropertyChanged
    {
        RestService _restService;
        private List<Spread> _spreads;
        private Spread _selectedSpread;
        private bool _isRefreshing;

        public LiveSpreadViewModel()
        {
            _restService = new RestService();
            RefreshCommand = new Command(CmdRefresh);

        }

        private async void OnButtonClicked()
        {
            UpdateSpreadsOnce();
        }

        private async void UpdateSpreadsOnce()
        {
            List<Spread> spreads = await _restService.GetSpreadsAsync(Constants.SpreadEndpoint);
            Spreads = spreads;
        }

        private void OnClearButtonClicked()
        {
            Spreads = new List<Spread>();
            //collectionView.ItemsSource = new List<Spread>();
        }

        public ICommand ButtonSelectedCommand
        {
            get { return new Command(x => OnButtonClicked()); }
        }

        public ICommand ButtonClearSelectedCommand
        {
            get { return new Command(x => OnClearButtonClicked()); }
        }

        public List<Spread> Spreads
        {
            get
            {
                return _spreads;
            }
            set
            {
                _spreads = value;
                OnPropertyChanged(nameof(Spreads));
            }
        }
        public Spread SelectedSpread
        {
            get
            {
                return _selectedSpread;
            }
            set
            {
                _selectedSpread = value;
                OnPropertyChanged(nameof(SelectedSpread));
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }        
        

        public ICommand RefreshCommand { get; set; }

      
        private async void CmdRefresh()
        {
            IsRefreshing = true;
            await Task.Delay(3000);
            IsRefreshing = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}