using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CryptOverseeMobileApp.Models;
using Reactive.Bindings;
using CryptOverseeMobileApp.Pages;
using CryptOverseeMobileApp.ViewModels.Settings;
using Xamarin.Forms;
using ChocoExchangesApi.Models;
using ChocoExchangesApi.Services;

namespace CryptOverseeMobileApp.ViewModels
{
    public class HistoricalSpreadsViewModel : ViewModelBase
    {
        private readonly HistoricalSettingsViewModel _settingViewModel;
        private readonly HistoricalSpreadsDetails _historicalSpreadDetails;


        private readonly RestService _restService;
        private DateTime _lastUpdateThreeHours = new();
        private DateTime _lastUpdateOneHours = new();
        private List<SpreadNote> _notes = new();
        private List<HistoricalSpreadModel> _unfilteredSpreads = new();
        private readonly Dictionary<int,List<HistoricalSpreadModel>> _unfilteredSpreadsDic = new();

        public INavigation Navigation { get; set; }
        
        public HistoricalSpreadsViewModel()
        {
            _restService = new RestService();
            _settingViewModel = new HistoricalSettingsViewModel();
            _historicalSpreadDetails = new HistoricalSpreadsDetails();

            InitialiseReactiveProperties();

            //Task.Factory.StartNew(async () =>
            //{
            //    _settingViewModel.PremiumMembership.Value = await PurchasesHelper.WasItemPurchased(PurchasesHelper.ProductCode);
            //});

            InitialiseDataOnStart();
        }

        private void InitialiseReactiveProperties()
        {
            Spreads = new ReactiveProperty<ObservableCollection<HistoricalSpreadModel>>(new ObservableCollection<HistoricalSpreadModel>());
            LastUpdate = new ReactiveProperty<DateTime>();
            NumberResultsAfterFiltering = new ReactiveProperty<int>(0);
            NumberResultsRaw = new ReactiveProperty<int>(0);
            IsRefreshing = new ReactiveProperty<bool>();
        }

        public double MinAverageSpread => _settingViewModel.MinAverageSpread.Value;
        public ReactiveProperty<int> NumberResultsAfterFiltering { get; set; }
        public ReactiveProperty<int> NumberResultsRaw { get; set; }
        public ReactiveProperty<bool> IsRefreshing { get; set; }
        public ReactiveProperty<ObservableCollection<HistoricalSpreadModel>> Spreads { get; set; }
        public ReactiveProperty<DateTime> LastUpdate { get; set; }


        public ICommand RefreshHistSpreadCommand => new Command(_ =>
        {
            RefreshData(true);
        });
        

        public ICommand SelectedPictureChangedCommand
        {
            get =>
                new Command(async selectedItem =>
                {
                    try
                    {
                        var item = (HistoricalSpreadModel)selectedItem;

                        _historicalSpreadDetails.HistoricalSpreadModel = item;
                        _historicalSpreadDetails.HistoricalSettingsViewModel = _settingViewModel;

                        await Navigation.PushAsync(_historicalSpreadDetails);
                    }
                    catch (Exception ex)
                    {
                    }
                });
        }


        public async Task RefreshData(bool refreshDataFromFileShare)
        {
            try
            {
                _notes = await _restService.GetExchangeNotesFromFileShare();
                _historicalSpreadDetails.Notes = _notes;

                if (refreshDataFromFileShare)
                {
                    var stopwatch = Stopwatch.StartNew();
                    var historicalSpreadsOneHour = await _restService.GetHistoricalSpreadsFromFileShare(1);
                    _unfilteredSpreadsDic[1] = historicalSpreadsOneHour.Data;
                    _lastUpdateOneHours = historicalSpreadsOneHour.DateTime;

                    var howLong = DateTime.Now - _lastUpdateThreeHours;
                    if (howLong > TimeSpan.FromMinutes(15))
                    {
                        var historicalSpreadsThreeHour = await _restService.GetHistoricalSpreadsFromFileShare(3);
                        _unfilteredSpreadsDic[3] = historicalSpreadsThreeHour.Data;
                        _lastUpdateThreeHours = historicalSpreadsThreeHour.DateTime;
                    }
                    
                    stopwatch.Stop();
                    Console.WriteLine($"Get historical data from file share took {stopwatch.ElapsedMilliseconds} ms");
                }

                var nbHours = _settingViewModel.NumberHours.Value;
                _unfilteredSpreads = _unfilteredSpreadsDic[nbHours];
                if (nbHours == 1)
                {
                    LastUpdate.Value = _lastUpdateOneHours;
                } else if (nbHours == 1)
                {
                    LastUpdate.Value = _lastUpdateThreeHours;
                }


                var stopwatch2 = Stopwatch.StartNew();
                //IsRefreshing.Value = true;

                _settingViewModel.InitialiseSettings(new List<ISpread>(_unfilteredSpreads));
                NumberResultsRaw.Value = _unfilteredSpreads.Count;

                var filteredSpreads = _settingViewModel.ApplySettings(_unfilteredSpreads).ToList();

                foreach (var spread in filteredSpreads)
                {
                    spread.SpreadOccurence = spread.GetSpreadOccurence(_settingViewModel.MinAverageSpread.Value);

                    if (Enum.TryParse(spread.BuyOn, out SupportedExchangeName buyOnExchange) &&
                        Enum.TryParse(spread.SellOn, out SupportedExchangeName sellOnExchange))
                    {
                        //var notes = ExchangesData.FilterNotes(sp.BaseCurrency, buyOnExchange, sellOnExchange);
                        var notes = _notes.FilterNotes(spread.BaseCurrency, buyOnExchange, sellOnExchange);
                        if (notes.Any())
                        {
                            var warning = string.Join('\n', notes.Select(_ => _.Note).ToList());
                            spread.Warning = warning;
                            spread.HasWarning = true;
                        }

                    }
                    else
                    {
                        var xx = 1;
                    }
                }

                Spreads.Value = new ObservableCollection<HistoricalSpreadModel>(filteredSpreads);
                NumberResultsAfterFiltering.Value = filteredSpreads.Count;

                stopwatch2.Stop();
                Console.WriteLine($"HistoricalSpread - Refreshed data in {stopwatch2.ElapsedMilliseconds} ms");

                if (_settingViewModel.AvailableExchanges.IsEmpty())
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
                    RefreshData(true);
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
                        var popup = new HistoricalSettingsContentPage(_settingViewModel);
                        await navigation.PushModalAsync(popup);
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