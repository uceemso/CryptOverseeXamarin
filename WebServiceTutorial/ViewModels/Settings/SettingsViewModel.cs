using System.Collections.Generic;
using System.Linq;
using CryptOverseeMobileApp.Models;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public abstract class SettingsViewModel : ViewModelBase, ISettings
    {
        public SettingsViewModel()
        {
            MarketsVM = new ObjectSelectorViewModel();
            ExchangesVM = new ObjectSelectorViewModel();
        }

        public ObjectSelectorViewModel MarketsVM { get; set; }
        public ObjectSelectorViewModel ExchangesVM { get; set; }

        public abstract void InitialiseSettings(List<ISpread> spreads);
        //public abstract IEnumerable<Spread> ApplySettings(IEnumerable<Spread> spreads);


        public void InitialiseSettingsWithKey(List<ISpread> spreads, string keyExchanges, string keyMarkets)
        {
            if (keyExchanges != null && !ExchangesVM.GetValues().Any())
            {
                var exchanges = spreads.Select(_ => _.BuyOn).Distinct().Union(spreads.Select(_ => _.SellOn).Distinct()).Distinct().ToList();
                var x = exchanges.Select(_ => new SettingItemViewModel(keyExchanges, _)).OrderBy(_ => _.Name);
                ExchangesVM.UpdateMarkets(x);
            }

            if (keyMarkets != null && !MarketsVM.GetValues().Any())
            {
                var baseCcies = spreads.Select(_ => _.Symbol.Split('/')[0]).Distinct().ToList();
                var quoteCcies = spreads.Select(_ => _.Symbol.Split('/')[1]).Distinct().ToList();
                var currencies = baseCcies.Union(quoteCcies).Distinct().ToList();
                var x = currencies.Select(_ => new SettingItemViewModel(keyMarkets, _)).OrderBy(_ => _.Name);
                MarketsVM.UpdateMarkets(x);
            }
        }

    }

}