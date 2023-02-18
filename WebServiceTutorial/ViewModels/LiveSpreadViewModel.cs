using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ChocoExchangesApi.Models;
using ChocoExchangesApi.Services;
using CryptOverseeMobileApp.Pages;
using CryptOverseeMobileApp.ViewModels.Settings;
using Reactive.Bindings;
using Xamarin.Forms;
using SpreadModel = CryptOverseeMobileApp.Models.SpreadModel;

namespace CryptOverseeMobileApp.ViewModels
{
    public class LiveSpreadViewModel : ViewModelBase
    {
        private readonly RestService _restService;
        private readonly LiveSpreadSettingsViewModel _settingViewModel;
        private readonly LiveSpreadDetails _liveSpreadDetails;
        private bool _premiumMembership;

        private bool _googlePlayConnected;
        private string _searchBar;
        private List<SpreadNote> _notes = new List<SpreadNote>();
        private List<SpreadModel> _unfilteredSpreads;
        private List<SpreadModel> _settingsFilteredSpreads;
        

        public LiveSpreadViewModel()
        {
            
            _restService = new RestService();
            _settingViewModel = new LiveSpreadSettingsViewModel();
            _liveSpreadDetails = new LiveSpreadDetails();

            SearchText = new ReactiveProperty<string>();
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
                _premiumMembership = await PurchasesHelper.WasItemPurchased(PurchasesHelper.ProductCode);
                _settingViewModel.PremiumMembership.Value = _premiumMembership;
            });

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await RefreshSpreads();
                    await Task.Delay(new TimeSpan(0, 0, 300));
                }
            });
        }

        public INavigation Navigation { get; set; }
        private async Task RefreshSpreads()
        {
            try
            {
                IsRefreshing.Value = true;
                //var serverResult = await _restService.GetSpreadsAsync(Constants.GetRecentSpreads);
                _notes = await _restService.GetExchangeNotesFromFileShare();
                _liveSpreadDetails.Notes = _notes;

                var serverResult = await _restService.GetSpreadsFromFileShare();
                LastUpdate.Value = serverResult.DateTime;


                _unfilteredSpreads = serverResult.Data.OrderByDescending(_ => _.SpreadValue).ToList();
                foreach (var sp in _unfilteredSpreads)
                {
                    if (Enum.TryParse(sp.BuyOn, out SupportedExchangeName buyOnExchange) &&
                        Enum.TryParse(sp.SellOn, out SupportedExchangeName sellOnExchange))
                    {
                        var notes = ExchangesData.FilterNotes(sp.BaseCurrency, buyOnExchange, sellOnExchange);
                        if (notes.Any())
                        {
                            var warning = string.Join('\n', notes.Select(_ => _.Note).ToList()) ;
                            sp.Warning = warning;
                            sp.HasWarning = true;
                        }

                    }
                    else
                    {
                        var xx = 1;
                    }
                    ;
                    ;
                }

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
        public ReactiveProperty<string> SearchText { get; set; }
        public ReactiveProperty<int> NumberResultsAfterFiltering { get; set; }
        public ReactiveProperty<int> NumberResultsRaw { get; set; }
        public ReactiveProperty<bool> IsRefreshing { get; set; }

        public ICommand PerformSearch => new Command<string>(ApplySearchBar);

        public ICommand RefreshCommand => new Command(_ => RefreshGrid());

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

        public ICommand DisplayLiveSpreadDetailsPage
        {
            get
            {
                return new Command(async selectedItem =>
                {
                    try
                    {
                        var item = (SpreadModel)selectedItem;
                        _liveSpreadDetails.SpreadModel = item;
                        await Navigation.PushAsync(_liveSpreadDetails);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            }
        }

        

    }
}