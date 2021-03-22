using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using CryptOverseeMobileApp.Models;
using Reactive.Bindings;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public class HistoricalSettingsViewModel : SettingsViewModel, ISettings
    {
        private const string HIST_EXCHANGES = "HIST_EXCHANGES";
        private const string HIST_MARKETS = "HIST_MARKETS";
        
        public HistoricalSettingsViewModel()
        {
            MinAverageSpreadRounded = new ReactiveProperty<double>(0.5);
            MinAverageSpread = new ReactiveProperty<double>(0.5);
            MinOccurence = new ReactiveProperty<double>(10);
            NumberDays = new ReactiveProperty<int>(2);

            MinAverageSpread
                .Throttle(TimeSpan.FromMilliseconds(20))
                .Subscribe(_ =>
                {
                    MinAverageSpreadRounded.Value = Math.Round(_, 1);
                });
        }

        public ReactiveProperty<double> MinAverageSpread { get; set; }
        public ReactiveProperty<double> MinAverageSpreadRounded { get; set; }
        public ReactiveProperty<double> MinOccurence { get; set; }
        public ReactiveProperty<int> NumberDays { get; set; }


        public override void InitialiseSettings(List<ISpread> spreads)
        {
            InitialiseSettingsWithKey(spreads, HIST_EXCHANGES, null);
        }

        public IEnumerable<HistoricalSpreadModel> ApplySettings(IEnumerable<HistoricalSpreadModel> spreads)
        {
            var filteredSpreads = spreads.
                Where(_ => _.GetSpreadOccurence(MinAverageSpread.Value) >= MinOccurence.Value && 
                           ShouldDisplayExchange(ExchangesVM.GetValues(), _.SellOn) && ShouldDisplayExchange(ExchangesVM.GetValues(), _.BuyOn)
                   // && ShouldDisplayMarket(_.Symbol)
                           );

            filteredSpreads = filteredSpreads.OrderByDescending(_ => _.SpreadOccurence).ToList();
            return filteredSpreads;
        }
    }


}