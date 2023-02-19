using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
            AvailableExchanges = new ElementCollection();
            AvailableMarkets = new ElementCollection();
            MinAverageSpreadRounded = new ReactiveProperty<double>(0.5);
            MinAverageSpread = new ReactiveProperty<double>(0.5);
            PremiumMembership = new ReactiveProperty<bool>();

            MinAverageSpread
                .Throttle(TimeSpan.FromMilliseconds(20))
                .Subscribe(_ =>
                {
                    MinAverageSpreadRounded.Value = Math.Round(_, 1);
                });
        }

        public ReactiveProperty<bool> PremiumMembership { get; set; }
        public ReactiveProperty<double> MinAverageSpread { get; set; }
        public ReactiveProperty<double> MinAverageSpreadRounded { get; set; }

        public ElementCollection AvailableExchanges { get; set; }
        public ElementCollection AvailableMarkets { get; set; }
        public ICommand PurchaseCommand => new Command(x => { Purchase(); });
        public ICommand CheckPurchasesCommand => new Command(x => { CheckPurchase(); });

        public void InitialiseSettings(List<SpreadModel> spreads)
        {
            if (AvailableExchanges.IsEmpty())
            {
                var x = SettingsHelper.GetExchangeElementList(spreads, LIVE_EXCHANGES);
                AvailableExchanges.SetValues(x);
            }
            if (AvailableMarkets.IsEmpty())
            {
                var x = SettingsHelper.GetMarketElementList(spreads, LIVE_MARKETS);
                AvailableMarkets.SetValues(x);
            }
        }

        public ICommand TapOnElement
        {
            get
            {
                return new Command(selectedItem =>
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
            }
        }

     
        
        private async Task Purchase()
        {
            var purchased = await PurchasesHelper.PurchaseItem(PurchasesHelper.ProductCode);
            if (purchased)
            {
                CheckPurchase();
            }


        }

        private async Task CheckPurchase()
        {
            var premium = await PurchasesHelper.WasItemPurchased(PurchasesHelper.ProductCode);
            PremiumMembership.Value = premium;
        }


    }


}