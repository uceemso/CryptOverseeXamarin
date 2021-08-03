using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ChocoExchangesApi.Models;
using CryptOverseeMobileApp.Models;
using Reactive.Bindings;
using CryptOverseeMobileApp.Pages;
using CryptOverseeMobileApp.ViewModels.Settings;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace CryptOverseeMobileApp.ViewModels
{
    public class HistoricalSpreadsViewModel : ViewModelBase
    {
        private readonly HistoricalSettingsContentPage _settingPopup;
        private readonly HistoricalSettingsViewModel _settingViewModel;
        //private MyPopupPageViewModel _popupViewModel;
        
        readonly RestService _restService;
        private List<HistoricalSpreadModel> _unfilteredSpreads = new();
        private Dictionary<int,List<HistoricalSpreadModel>> _unfilteredSpreadsDic = new();

        public INavigation Navigation { get; set; }
        
        public HistoricalSpreadsViewModel()
        {
            InitialiseReactiveProperties();

            _settingViewModel = new HistoricalSettingsViewModel();
            _settingPopup = new HistoricalSettingsContentPage(_settingViewModel);
            //_popupViewModel = new MyPopupPageViewModel();

            _restService = new RestService();
            InitialiseDataOnStart();
        }

        public double MinAverageSpread => _settingViewModel.MinAverageSpread.Value;

        public void InitialiseReactiveProperties()
        {
            Spreads = new ReactiveProperty<ObservableCollection<HistoricalSpreadModel>>(new ObservableCollection<HistoricalSpreadModel>());

            SelectedSpread = new ReactiveProperty<HistoricalSpreadModel>();
            SelectedSpread.Where(_ => _ != null).Subscribe(newValue =>
            {
                //_popupViewModel.Symbol = newValue.Symbol;
                //_popupViewModel.HistSpread = newValue;
                //_popupViewModel.MinAverageSpread = MinAverageSpread;
            });

            NumberResultsAfterFiltering = new ReactiveProperty<int>(0);
            NumberResultsRaw = new ReactiveProperty<int>(0);
            IsRefreshing = new ReactiveProperty<bool>();
        }
        

        public ReactiveProperty<int> NumberResultsAfterFiltering { get; set; }
        public ReactiveProperty<int> NumberResultsRaw { get; set; }
        public ReactiveProperty<bool> IsRefreshing { get; set; }
        public ReactiveProperty<ObservableCollection<HistoricalSpreadModel>> Spreads { get; set; }
        public ReactiveProperty<HistoricalSpreadModel> SelectedSpread { get; set; }

        public ICommand RefreshHistSpreadCommand { get { return new Command(_ => RefreshData()); } }

        //public Command SelectedTagChanged
        //{
        //    get { return new(async () => { await Navigation.PushPopupAsync(new MyPopupPage(_popupViewModel)); }); }
        //}

        public ICommand SelectedPictureChangedCommand
        {
            get
            {
                return new Command(async selectedItem =>
                {
                    try
                    {
                        var item = (HistoricalSpreadModel)selectedItem;
                        var vm = new MyPopupPageViewModel();
                        //vm.Symbol = item.Symbol;
                        //vm.HistSpread = item;
                        //vm.MinAverageSpread = MinAverageSpread;
                        //var exchangeA = (SupportedExchangeName) Enum.Parse(typeof(SupportedExchangeName), item.BuyOn);
                        //var exchangeB = (SupportedExchangeName) Enum.Parse(typeof(SupportedExchangeName), item.SellOn);
                        //vm.RefreshSpread(exchangeA, exchangeB, item.BaseCurrency, item.QuoteCurrency);
                        vm.RefreshSpread(item);
                        await Navigation.PushPopupAsync(new MyPopupPage(vm));


                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }

        public void RefreshData()
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                IsRefreshing.Value = true;


                _unfilteredSpreads = _unfilteredSpreadsDic[_settingViewModel.NumberHours.Value];
                _settingViewModel.InitialiseSettings(new List<ISpread>(_unfilteredSpreads));
                NumberResultsRaw.Value = _unfilteredSpreads.Count;

                var filteredSpreads = _settingViewModel.ApplySettings(_unfilteredSpreads).ToList();

                foreach (var spread in filteredSpreads)
                {
                    spread.SpreadOccurence = spread.GetSpreadOccurence(_settingViewModel.MinAverageSpread.Value);
                }

                Spreads.Value = new ObservableCollection<HistoricalSpreadModel>(filteredSpreads);
                NumberResultsAfterFiltering.Value = filteredSpreads.Count;

                stopwatch.Stop();
                Console.WriteLine($"HistoricalSpread - Refreshed data in {stopwatch.ElapsedMilliseconds} ms");

                if (!_settingViewModel.ExchangesVM.Values.Value.Any())
                {
                    _settingViewModel.InitialiseSettings(new List<ISpread>(_unfilteredSpreads));
                }
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

        private void InitialiseDataOnStart()
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    IsRefreshing.Value = true;
                    var hours = new List<int> {1, 24, 48, 72, 168};
                    var stopwatch = Stopwatch.StartNew();
                    var spreads1 = _restService.GetHistoricalSpreadsAsync(1);
                    //var spreads24 = _restService.GetHistoricalSpreadsAsync(24);
                    var spreads48 = _restService.GetHistoricalSpreadsAsync(48);
                    //var spreads72 = _restService.GetHistoricalSpreadsAsync(72);
                    //var spreads168 = _restService.GetHistoricalSpreadsAsync(168);

                    //await Task.WhenAll(new List<Task>() {spreads1, spreads24, spreads48, spreads72, spreads168});
                    await Task.WhenAll(new List<Task>() {spreads1, spreads48});

                    _unfilteredSpreadsDic[1] = spreads1.Result;
                    //_unfilteredSpreadsDic[24] = spreads24.Result;
                    _unfilteredSpreadsDic[48] = spreads48.Result;
                    //_unfilteredSpreadsDic[72] = spreads72.Result;
                    //_unfilteredSpreadsDic[168] = spreads168.Result;

                    stopwatch.Stop();
                    Console.WriteLine($"Get data took {stopwatch.ElapsedMilliseconds} ms");

                    // _unfilteredSpreads = await _restService.GetHistoricalSpreadsAsync(_settingViewModel.NumberHours.Value);
                    _unfilteredSpreads = _unfilteredSpreadsDic[_settingViewModel.NumberHours.Value];


                    RefreshData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
                finally
                {
                    IsRefreshing.Value = false;
                }
            });
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
                        await navigation.PushModalAsync(_settingPopup);
                    }
                    else
                    {
                        var a = 1;
                    }
                });
            }
        }



    }
}