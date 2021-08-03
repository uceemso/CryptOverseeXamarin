using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using CryptOverseeMobileApp.Models;
using Reactive.Bindings;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public class BitcoinSettingsViewModel : SettingsViewModel, ISettings
    {
        private const string BITCOIN_EXCHANGES = "BITCOIN_EXCHANGES";
        private const string BITCOIN_MARKETS = "BITCOIN_MARKETS";
        
        public BitcoinSettingsViewModel()
        {
            MinSpread = new ReactiveProperty<double>(0.1);
            MinSpreadRounded = new ReactiveProperty<double>(0.1);

            MinSpread
                .Throttle(TimeSpan.FromMilliseconds(50))
                .Subscribe(_ =>
                {
                    MinSpreadRounded.Value = Math.Round(_, 2);
                });
        }

        public ReactiveProperty<double> MinSpread { get; set; }
        public ReactiveProperty<double> MinSpreadRounded { get; set; }


        public IEnumerable<SpreadModel> ApplySettings(IEnumerable<SpreadModel> spreads)
        {
            var filteredSpreads = spreads.Where(_ =>
                SettingsHelper.ShouldDisplayBasedOnExchangeSettings(ExchangesVM.GetValues(), _.SellOn, _.BuyOn)
                && SettingsHelper.ShouldDisplayMarket(MarketsVM.GetValues(), _.Symbol) && _.SpreadValue >= MinSpread.Value);

            return filteredSpreads;
        }

        public override void InitialiseSettings(List<ISpread> spreads)
        {
            InitialiseSettingsWithKey(spreads, BITCOIN_EXCHANGES, BITCOIN_MARKETS);
        }

    }
}