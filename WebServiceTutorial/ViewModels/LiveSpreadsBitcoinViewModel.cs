using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CryptOverseeMobileApp.Models;
using Reactive.Bindings;
using CryptOverseeMobileApp.Pages;
using CryptOverseeMobileApp.ViewModels.Settings;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace CryptOverseeMobileApp.ViewModels
{
    public class LiveSpreadsBitcoinViewModel : ViewModelBase, IDisposable
    {
        private readonly SettingsContentPage _settingPopup;
        private readonly BitcoinSettingsViewModel _settingViewModel;

        readonly RestService _restService;
        private bool _isRefreshing;
        private string _someString;
        private MyPopupPageViewModel _popupViewModel;
        private bool _doNotShowPopupOnSelectedItemUpdate;

        private List<Spread> _unfilteredSpreads = new();

        public INavigation Navigation { get; set; }


        public LiveSpreadsBitcoinViewModel()
        {
            _settingViewModel = new BitcoinSettingsViewModel();
            _settingPopup = new SettingsContentPage(_settingViewModel);

            Spreads = new ReactiveProperty<ObservableCollection<Spread>>(new ObservableCollection<Spread>());
            SelectedSpread = new ReactiveProperty<Spread>();
            SelectedSpread.Subscribe(newValue =>
            {
                PreviouslySelectedSpread = newValue;
                _popupViewModel = new MyPopupPageViewModel();
                _popupViewModel.Symbol = newValue?.Symbol;
            });


            _restService = new RestService();
            SomeText = "test";
            
            UpdateSpreadsAsync();
        }

        public void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            //Debug.WriteLine("LastVisibleItemIndex: " + e.LastVisibleItemIndex);
        }

        public void RefreshData()
        {
            var orderedSpreads = _unfilteredSpreads.OrderByDescending(_ => _.SpreadValue);
            var filteredSpreads = _settingViewModel.ApplySettings(orderedSpreads).ToList();
            Spreads.Value = new ObservableCollection<Spread>(filteredSpreads);
        }

        private async void UpdateBitcoinSpreadsOnce()
        {
            try
            {
                _unfilteredSpreads = await _restService.GetSpreadsAsync(Constants.BitcoinSpreadEndpoint);

                _settingViewModel.InitialiseSettings(new List<ISpread>(_unfilteredSpreads));

                RefreshData();

                if (PreviouslySelectedSpread != null)
                {
                    var item = Spreads.Value.FirstOrDefault(_ => _.BuyOn == PreviouslySelectedSpread.BuyOn
                                                                 && _.SellOn == PreviouslySelectedSpread.SellOn
                                                                 && _.Symbol == PreviouslySelectedSpread.Symbol);
                    if (item != null)
                    {
                        _doNotShowPopupOnSelectedItemUpdate = true;
                        SelectedSpread.Value = item;
                        PreviouslySelectedSpread = item;
                    }
                    else
                    {
                        //SelectedSpread.Value = null;
                        PreviouslySelectedSpread = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }


        }

        private void OnClearButtonClicked()
        {
            Spreads.Value = new ObservableCollection<Spread>();
        }


        public ICommand ButtonClearSelectedCommand
        {
            get { return new Command(x => OnClearButtonClicked()); }
        }        
        
        public ICommand NavigateToSettingsCommand
        {
            get { return new Command(async _ =>
            {
                if (Navigation != null && Navigation.ModalStack.Count == 0) {
                    await Navigation.PushModalAsync(_settingPopup);
                }
                else
                {
                    var a = 1;
                }
            }); }
        }

        public ICommand RefreshCommand
        {
            get { return new Command(x => RefreshButtonPushed()); }
        }

        public ReactiveProperty<ObservableCollection<Spread>> Spreads { get; }
        public ReactiveProperty<Spread> SelectedSpread { get; }
        public Spread PreviouslySelectedSpread { get; set; }


        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }        
        
        public string SomeText
        {
            get => _someString;
            set
            {
                _someString = value;
                OnPropertyChanged(nameof(SomeText));
            }
        }




        public Command SelectedTagChanged
        {
            get
            {
                return new Command(async () =>
                {
                    if (!_doNotShowPopupOnSelectedItemUpdate)
                    {
                        await Navigation.PushPopupAsync(new MyPopupPage(_popupViewModel));
                        //await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new MyPopupPage(_popupViewModel));
                    }

                    _doNotShowPopupOnSelectedItemUpdate = false;
                });
            }
        }

        private void RefreshButtonPushed()
        {
            IsRefreshing = true;
            SomeText = "refreshing";
            UpdateBitcoinSpreadsOnce();
            IsRefreshing = false;
            SomeText = "refresh done ";
        }

        private void UpdateSpreadsAsync()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    UpdateBitcoinSpreadsOnce();
                    await Task.Delay(new TimeSpan(0, 0, 10));
                }
            });

        }


        public void Dispose()
        {

        }
    }
}