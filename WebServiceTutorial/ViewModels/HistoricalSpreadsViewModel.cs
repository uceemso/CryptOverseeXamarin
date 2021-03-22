using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    public class HistoricalSpreadsViewModel : ViewModelBase
    {
        private readonly HistoricalSettingsContentPage _settingPopup;
        private readonly HistoricalSettingsViewModel _settingViewModel;
        private MyPopupPageViewModel _popupViewModel;
        
        readonly RestService _restService;
        private List<HistoricalSpreadModel> _unfilteredSpreads = new();

        public INavigation Navigation { get; set; }
        
        public HistoricalSpreadsViewModel()
        {
            InitialiseReactiveProperties();

            _settingViewModel = new HistoricalSettingsViewModel();
            _settingPopup = new HistoricalSettingsContentPage(_settingViewModel);
            _popupViewModel = new MyPopupPageViewModel();

            _restService = new RestService();
            UpdateData();
        }

        public double MinAverageSpread => _settingViewModel.MinAverageSpread.Value;

        public void InitialiseReactiveProperties()
        {
            Spreads = new ReactiveProperty<ObservableCollection<HistoricalSpreadModel>>(new ObservableCollection<HistoricalSpreadModel>());

            SelectedSpread = new ReactiveProperty<HistoricalSpreadModel>();
            SelectedSpread.Where(_ => _ != null).Subscribe(newValue =>
            {
                _popupViewModel.Symbol = newValue.Symbol;
                _popupViewModel.HistSpread = newValue;
                _popupViewModel.MinAverageSpread = MinAverageSpread;
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
        
        public ICommand RefreshCommand
        {
            get { return new Command(_ => RefreshData()); }
        }

        public Command SelectedTagChanged
        {
            get { return new(async () => { await Navigation.PushPopupAsync(new MyPopupPage(_popupViewModel)); }); }
        }

        public void RefreshData()
        {
            var stopwatch = Stopwatch.StartNew();
            IsRefreshing.Value = true;

            var filteredSpreads = _settingViewModel.ApplySettings(_unfilteredSpreads).ToList();
            
            foreach (var spread in filteredSpreads)
            {
                spread.SpreadOccurence = spread.GetSpreadOccurence(_settingViewModel.MinAverageSpread.Value);
            }
            
            Spreads.Value = new ObservableCollection<HistoricalSpreadModel>(filteredSpreads);
            NumberResultsAfterFiltering.Value = filteredSpreads.Count;
            IsRefreshing.Value = false;
            stopwatch.Stop();
            Console.WriteLine($"HistoricalSpread - Refreshed data in {stopwatch.ElapsedMilliseconds} ms");
        }

        private void UpdateData()
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    _unfilteredSpreads = await _restService.GetHistoricalSpreadsAsync();
                    _settingViewModel.InitialiseSettings(new List<ISpread>(_unfilteredSpreads));
                    
                    NumberResultsRaw.Value = _unfilteredSpreads.Count;
                    RefreshData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            });
        }

        public ICommand NavigateToSettingsCommand
        {
            get
            {
                return new Command(async _ =>
                {
                    if (Navigation != null && Navigation.ModalStack.Count == 0)
                    {
                        await Navigation.PushModalAsync(_settingPopup);
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