using System.Collections.Generic;
using System.Linq;
using CryptOverseeMobileApp.Models;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public abstract class SettingsViewModel : ViewModelBase, ISettings
    {
        public SettingsViewModel()
        {
            MarketsVM = new ElementCollection();
            ExchangesVM = new ElementCollection();
        }

        public ElementCollection MarketsVM { get; set; }
        public ElementCollection ExchangesVM { get; set; }

        public abstract void InitialiseSettings(List<ISpread> spreads);


        public void InitialiseSettingsWithKey(List<ISpread> spreads, string pageName)
        {
            if (ExchangesVM.IsEmpty())
            {
                var exchanges = spreads.Select(_ => _.BuyOn).Distinct().Union(spreads.Select(_ => _.SellOn).Distinct()).Distinct().ToList();
                var x = SettingsHelper.GetElementList(exchanges, pageName);
                ExchangesVM.SetValues(x);
            }
        }

    }

}