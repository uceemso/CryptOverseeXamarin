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
    public class HistoricalSettingsViewModel : SettingsViewModel
    {
        private const string HIST_EXCHANGES = "HIST_EXCHANGES";
        private const string HIST_MARKETS = "HIST_MARKETS";
        
        public HistoricalSettingsViewModel()
        {
            MinAverageSpreadRounded = new ReactiveProperty<double>(0.5);
            MinAverageSpread = new ReactiveProperty<double>(0.5);
            MinOccurence = new ReactiveProperty<double>(10);
            DurationIsChecked1 = new ReactiveProperty<bool>();
            DurationIsChecked3 = new ReactiveProperty<bool>();
            NumberHours = new ReactiveProperty<int>(1);
            AvailableExchanges = new ReactiveList();
            AvailableMarkets = new ReactiveList();

            NumberHours.Subscribe(_ =>
            {
                DurationIsChecked1.Value = _ == 1;
                DurationIsChecked3.Value = _ == 3;
            });

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
        public ReactiveProperty<int> NumberHours { get; set; }
        public ReactiveProperty<bool> DurationIsChecked1 { get; set; }
        public ReactiveProperty<bool> DurationIsChecked3 { get; set; }

        public ReactiveList AvailableExchanges { get; set; }
        public ReactiveList AvailableMarkets { get; set; }

        public void InitialiseSettings(List<ISpread> spreads)
        {
            //InitialiseSettingsWithKey(spreads, HIST_EXCHANGES);
            if (AvailableExchanges.IsEmpty())
            {
                var x = SettingsHelper.GetExchangeElementList(spreads, HIST_EXCHANGES, PremiumMembership.Value);
                AvailableExchanges.SetValues(x);
            }
        }

        public IEnumerable<HistoricalSpreadModel> ApplySettings(IEnumerable<HistoricalSpreadModel> spreads)
        {
            try
            {
                var filteredSpreads = spreads.
                    Where(_ => _.GetSpreadOccurence(MinAverageSpread.Value) >= MinOccurence.Value &&
                               SettingsHelper.ShouldDisplayBasedOnExchangeSettings(AvailableExchanges, _)
                        // && ShouldDisplayMarket(_.Symbol)
                    );

                filteredSpreads = filteredSpreads.OrderByDescending(_ => _.SpreadOccurence).ToList();
                return filteredSpreads;
            } catch (Exception ex)
            {
                return null;
            }
        }

    }


}