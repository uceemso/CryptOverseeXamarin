using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using CryptOverseeMobileApp.Models;
using Reactive.Bindings;
using Xamarin.Forms;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public class LiveSpreadSettingsViewModel
    {
        private const string LIVE_EXCHANGES = "LIVE_EXCHANGES";
        private const string LIVE_MARKETS = "LIVE_MARKETS";

        public LiveSpreadSettingsViewModel()
        {
            ExchangesVM = new ObjectSelectorViewModel();
            MarketsVM = new ObjectSelectorViewModel();
            MinAverageSpreadRounded = new ReactiveProperty<double>(0.5);
            MinAverageSpread = new ReactiveProperty<double>(0.5);

            MinAverageSpread
                .Throttle(TimeSpan.FromMilliseconds(20))
                .Subscribe(_ =>
                {
                    MinAverageSpreadRounded.Value = Math.Round(_, 1);
                });
        }

        public ReactiveProperty<double> MinAverageSpread { get; set; }
        public ReactiveProperty<double> MinAverageSpreadRounded { get; set; }

        public ObjectSelectorViewModel ExchangesVM { get; set; }
        public ObjectSelectorViewModel MarketsVM { get; set; }

        public void InitialiseSettings(List<SpreadModel> spreads)
        {
            if (!ExchangesVM.GetValues().Any())
            {
                var exchanges = SettingsHelper.GetDistinctExchanges(spreads);
                var x = exchanges.Select(_ => new SettingItemViewModel(LIVE_EXCHANGES, _)).OrderBy(_ => _.Name);
                ExchangesVM.UpdateMarkets(x);
            }
            if (!MarketsVM.GetValues().Any())
            {
                var quoteCcies = spreads.Select(_ => _.Symbol.Split('/')[1]).Distinct().ToList();
                var x = quoteCcies.Select(_ => new SettingItemViewModel(LIVE_MARKETS, _)).OrderBy(_ => _.Name);
                MarketsVM.UpdateMarkets(x);
            }
        }

        public IEnumerable<SpreadModel> ApplySettings(IEnumerable<SpreadModel> spreads)
        {
            try
            {
                var filteredSpreads = spreads.Where(_ => _.SpreadValue > MinAverageSpread.Value
                                                         && SettingsHelper.ShouldDisplayBasedOnExchangeSettings(ExchangesVM.GetValues(), _.SellOn, _.BuyOn)
                                                         && SettingsHelper.ShouldDisplayBaseOnMarketSetting(MarketsVM.GetValues(), _.Symbol));

                //filteredSpreads = filteredSpreads.OrderByDescending(_ => _.SpreadValue).ToList();
                return filteredSpreads;
            } catch (Exception ex)
            {
                return null;
            }
            

        }

        public ICommand SelectedPictureChangedCommand
        {
            get
            {
                return new Command(selectedItem =>
                {
                    try
                    {
                        var item = (SettingItemViewModel) selectedItem;
                        item.Toggle();
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }

    }


}