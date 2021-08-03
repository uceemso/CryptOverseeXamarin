using System.Collections.Generic;
using System.Linq;
using CryptOverseeMobileApp.Models;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public static class SettingsHelper
    {
        public static bool ShouldDisplayBasedOnExchangeSettings(IEnumerable<SettingItemViewModel> entries, string exchangeNameA, string exchangeNameB)
        {
            var exchangeAEnabled = entries.FirstOrDefault(_ => _.Name == exchangeNameA)?.Enabled;
            var exchangeBEnabled = entries.FirstOrDefault(_ => _.Name == exchangeNameB)?.Enabled;
            
            if (exchangeBEnabled == null || exchangeAEnabled == null) return true;
            return exchangeAEnabled.Value && exchangeBEnabled.Value;
        }

        public static bool ShouldDisplayMarket(IEnumerable<SettingItemViewModel> entries, string marketSymbol)
        {
            var currencies = marketSymbol.Split('/');
            var baseCcy = currencies[0];
            var quoteCcy = currencies[1];

            //var symbolsAccepted = MarketsVM.GetValues().Where(_ => _.Enabled).Select(_ => _.Name).ToList();
            var symbolsAccepted = entries.Where(_ => _.Enabled).Select(_ => _.Name).ToList();

            return symbolsAccepted.Contains(baseCcy) && symbolsAccepted.Contains(quoteCcy);
        }

        public static bool ShouldDisplayBaseOnMarketSetting(IEnumerable<SettingItemViewModel> entries, string marketSymbol)
        {
            var currencies = marketSymbol.Split('/');
            var quoteCcy = currencies[1];

            var symbolsAccepted = entries.Where(_ => _.Enabled).Select(_ => _.Name).ToList();

            return symbolsAccepted.Contains(quoteCcy);
        }

        public static List<string> GetDistinctExchanges(List<SpreadModel> spreads)
        {
            var exchanges = spreads.Select(_ => _.BuyOn).Distinct().Union(spreads.Select(_ => _.SellOn).Distinct()).Distinct().ToList();
            return exchanges;
        }
    }
}