using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CryptOverseeMobileApp.Models;
using Reactive.Bindings;
using Xamarin.Forms;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public class LiveSpreadSettingsViewModel : SettingsViewModel
    {
        private const string LIVE_EXCHANGES = "LIVE_EXCHANGES";
        private const string LIVE_MARKETS = "LIVE_MARKETS";

        public LiveSpreadSettingsViewModel()
        {
            AvailableExchanges = new ReactiveList();
            AvailableMarkets = new ReactiveList();

            MinAverageSpreadRounded = new ReactiveProperty<double>(0.5);
            MinAverageSpread = new ReactiveProperty<double>(0.5);
            MaxAverageSpreadRounded = new ReactiveProperty<double>(100);
            MaxAverageSpread = new ReactiveProperty<double>(100);
            IgnoreCap = new ReactiveProperty<bool>();
            HidePairsWithWarning = new ReactiveProperty<bool>();

            MinAverageSpread
                .Throttle(TimeSpan.FromMilliseconds(20))
                .Subscribe(_ =>
                {
                    MinAverageSpreadRounded.Value = Math.Round(_, 1);
                });

            MaxAverageSpread
                .Throttle(TimeSpan.FromMilliseconds(20))
                .Subscribe(_ =>
                {
                    MaxAverageSpreadRounded.Value = Math.Round(_, 1);
                });
        }

        public ReactiveProperty<bool> HidePairsWithWarning { get; set; }
        public ReactiveProperty<bool> IgnoreCap { get; set; }

        public ReactiveProperty<double> MinAverageSpread { get; set; }
        public ReactiveProperty<double> MinAverageSpreadRounded { get; set; }
        public ReactiveProperty<double> MaxAverageSpread { get; set; }
        public ReactiveProperty<double> MaxAverageSpreadRounded { get; set; }


        public ReactiveList AvailableExchanges { get; set; }
        public ReactiveList AvailableMarkets { get; set; }
        public ICommand CheckPurchasesCommand => new Command(x => { CheckPurchase(); });

        public void InitialiseSettings(List<SpreadModel> spreads)
        {
            if (AvailableExchanges.IsEmpty())
            {
                var x = SettingsHelper.GetExchangeElementList(spreads, LIVE_EXCHANGES, PremiumMembership.Value);
                AvailableExchanges.SetValues(x);
            }
            if (AvailableMarkets.IsEmpty())
            {
                var x = SettingsHelper.GetMarketElementList(spreads, LIVE_MARKETS);
                AvailableMarkets.SetValues(x);
            }
        }

        public ICommand TapOnElement =>
            new Command(selectedItem =>
            {
                try
                {
                    var item = (Element) selectedItem;
                    item.Toggle();
                }
                catch (Exception ex)
                {

                }
            });


        private async Task CheckPurchase()
        {
            var premium = await PurchasesHelper.WasItemPurchased(PurchasesHelper.ProductCode);
            PremiumMembership.Value = premium;
        }


    }


}