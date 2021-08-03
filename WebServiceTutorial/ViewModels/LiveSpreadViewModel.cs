using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ChocoExchangesApi.Models;
using ChocoExchangesCommon;
using CryptOverseeMobileApp.Pages;
using CryptOverseeMobileApp.ViewModels.Settings;
using Reactive.Bindings;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using SpreadModel = CryptOverseeMobileApp.Models.SpreadModel;

namespace CryptOverseeMobileApp.ViewModels
{
    public class LiveSpreadViewModel : ViewModelBase
    {
        RestService _restService;
        private SpreadModel _selectedSpread;
        private readonly CryptoWatchNewOnce _cryptoWatch = new();
        private readonly LiveSpreadSettingsViewModel _settingViewModel;
        private string _searchBar;
        private List<SpreadModel> _unfilteredSpreads;
        private List<SpreadModel> _settingsFilteredSpreads;
        private readonly MyPopupPageLiveSpreadViewModel _popupViewModel;


        public LiveSpreadViewModel()
        {
            _restService = new RestService();

            _settingViewModel = new LiveSpreadSettingsViewModel();
            _popupViewModel = new MyPopupPageLiveSpreadViewModel();

            IsRefreshing = new ReactiveProperty<bool>();
            CryptoWatchTargets = new ReactiveProperty<ObservableCollection<CryptoWatchTarget>>(new ObservableCollection<CryptoWatchTarget>());
            Spreads = new ReactiveProperty<ObservableCollection<SpreadModel>>(new ObservableCollection<SpreadModel>());
            LastUpdate = new ReactiveProperty<DateTime>();
            NumberResultsAfterFiltering = new ReactiveProperty<int>(0);
            NumberResultsRaw = new ReactiveProperty<int>(0);
            Spreads.Subscribe(spreads =>
            {
                NumberResultsAfterFiltering.Value = spreads.Count;
            });

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await RefreshSpreads();
                    //var results = _cryptoWatch.RunOnce();
                    //var filtered = results.Where(_ => _.SpreadBuyOnASellOnB > (decimal?) 0.5 || _.SpreadBuyOnBSellOnA > (decimal?) 0.5).ToList().OrderByDescending(_ => _.SpreadBuyOnASellOnB);
                    //CryptoWatchTargets.Value = new ObservableCollection<CryptoWatchTarget>(filtered);

                    await Task.Delay(new TimeSpan(0, 0, 300));
                }
            });
        }

        public INavigation Navigation { get; set; }
        private async Task RefreshSpreads()
        {
            //if (IsRefreshing.Value)
            //{
            //    Console.WriteLine($"Already refreshing LiveSpread grid...");
            //    return;
            //}

            try
            {
                IsRefreshing.Value = true;
                var serverResult = await _restService.GetSpreadsAsync(Constants.GetRecentSpreads);
                LastUpdate.Value = serverResult.DateTime;

                _unfilteredSpreads = serverResult.Data.OrderByDescending(_ => _.SpreadValue).ToList();
                NumberResultsRaw.Value = _unfilteredSpreads.Count;


                _settingViewModel.InitialiseSettings(_unfilteredSpreads);
                _settingsFilteredSpreads = _settingViewModel.ApplySettings(_unfilteredSpreads).ToList();

                ApplySearchBar(_searchBar);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                IsRefreshing.Value = false;
            }
        }

        public ICommand PerformSearch => new Command<string>(( query) =>
        {
            ApplySearchBar(query);
        });

        public void ApplySearchBar(string searchBar)
        {
            _searchBar = searchBar;
            
            if (string.IsNullOrEmpty(_searchBar))
            {
                Spreads.Value = new ObservableCollection<SpreadModel>(_settingsFilteredSpreads);
            }
            else
            {
                var spreads = _settingsFilteredSpreads.Where(_ => _.Symbol.ToLower().Contains(_searchBar.ToLower())).ToList();
                Spreads.Value = new ObservableCollection<SpreadModel>(spreads);
            }

        }

        public ReactiveProperty<ObservableCollection<CryptoWatchTarget>> CryptoWatchTargets { get; set; }
        public ReactiveProperty<ObservableCollection<SpreadModel>> Spreads { get; set; }
        public ReactiveProperty<DateTime> LastUpdate { get; set; }
        public ReactiveProperty<int> NumberResultsAfterFiltering { get; set; }
        public ReactiveProperty<int> NumberResultsRaw { get; set; }

        private async void OnButtonClicked()
        {
            UpdateSpreadsOnce();
        }

        private async void UpdateSpreadsOnce()
        {
            //var spreads = await _restService.GetSpreadsAsync(Constants.SpreadEndpoint);
            //Spreads.Value = new ObservableCollection<Spread>(spreads);
        }

        private void OnClearButtonClicked()
        {
            Spreads.Value.Clear();
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

        public SpreadModel SelectedSpread
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

        public ReactiveProperty<bool> IsRefreshing { get; set; }

        public ICommand RefreshCommand { get { return new Command(_ => RefreshGrid()); } }

        public async void RefreshGrid()
        {
            await RefreshSpreads();
        }

        public ICommand NavigateToSettingsCommand
        {
            get
            {
                return new Command(async _ =>
                {
                    var navigation = Application.Current.MainPage.Navigation;

                    if (navigation != null && navigation.ModalStack.Count == 0)
                    {
                        var popup = new LiveSpreadSettingsContentPage(_settingViewModel);
                        await navigation.PushModalAsync(popup);
                    }
                    else
                    {
                        var a = 1;
                    }
                });
            }
        }

        public ICommand DisplayLiveSpreadPopupCommand
        {
            get
            {
                return new Command(async selectedItem =>
                {
                    try
                    {
                        if (_popupViewModel.IsOn) return; //There is already one popup displayed
                        
                        var item = (SpreadModel)selectedItem;
                        _popupViewModel.StartLiveFeed(item);
                        await Navigation.PushPopupAsync(new MyPopupPageLiveSpread(_popupViewModel));
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }

    }
}