using System;
using System.Collections.Generic;
using System.Linq;
using CryptOverseeMobileApp.Models;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public static class SettingsHelper
    {
        
        public static bool ShouldDisplayBasedOnExchangeSettings(ReactiveList collection, ISpread spread)
        {
            var elements = collection.GetValues();
            var exchangeAEnabled = elements.FirstOrDefault(_ => _.Name == spread.BuyOn)?.Enabled;
            var exchangeBEnabled = elements.FirstOrDefault(_ => _.Name == spread.SellOn)?.Enabled;
            
            if (exchangeBEnabled == null || exchangeAEnabled == null) return true;
            return exchangeAEnabled.Value && exchangeBEnabled.Value;
        }

        public static bool ShouldDisplayMarket(IEnumerable<Element> elements, string marketSymbol)
        {
            var currencies = marketSymbol.Split('/');
            var baseCcy = currencies[0];
            var quoteCcy = currencies[1];

            //var symbolsAccepted = MarketsVM.GetValues().Where(_ => _.Enabled).Select(_ => _.Name).ToList();
            var symbolsAccepted = elements.Where(_ => _.Enabled).Select(_ => _.Name).ToList();

            return symbolsAccepted.Contains(baseCcy) && symbolsAccepted.Contains(quoteCcy);
        }

        public static bool ShouldDisplayBaseOnMarketSetting(ReactiveList collection, ISpread spread)
        {
            var symbolsAccepted = collection.GetValues().Where(_ => _.Enabled).Select(_ => _.Name).ToList();
            return symbolsAccepted.Contains(spread.QuoteCurrency);
        }

        public static List<string> GetDistinctExchanges(List<SpreadModel> spreads)
        {
            var exchanges = spreads.Select(_ => _.BuyOn).Distinct().Union(spreads.Select(_ => _.SellOn).Distinct()).Distinct().ToList();
            return exchanges;
        }

        public static List<Element> GetElementList(List<string> names, string pageName)
        {
            var x = names.Select(_ => new Element(pageName, _)).OrderBy(_ => _.Name);
            return x.ToList();
        }

        public static List<Element> GetExchangeElementList(List<SpreadModel> spreads, string pageName)
        {
            var exchanges = GetDistinctExchanges(spreads);
            var x = exchanges.Select(_ => new Element(pageName, _)).OrderBy(_ => _.Name);
            return x.ToList();
        }

        public static List<Element> GetMarketElementList(List<SpreadModel> spreads, string pageName)
        {
            var quoteCcies = spreads.Select(_ => _.QuoteCurrency).Distinct().ToList();
            var x = GetElementList(quoteCcies, pageName);
            return x.ToList();
        }

        public static List<SpreadModel> FilterSpreadsAccordingToSettings(LiveSpreadSettingsViewModel settings, IEnumerable<SpreadModel> spreads)
        {
            try
            {
                var filteredSpreads = spreads.Where(_ => _.SpreadValue > settings.MinAverageSpread.Value
                                                         && ShouldDisplayBasedOnExchangeSettings(settings.AvailableExchanges, _)
                                                         && ShouldDisplayBaseOnMarketSetting(settings.AvailableMarkets, _)).ToList();

                //filteredSpreads = filteredSpreads.OrderByDescending(_ => _.SpreadValue).ToList();
                return filteredSpreads;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}