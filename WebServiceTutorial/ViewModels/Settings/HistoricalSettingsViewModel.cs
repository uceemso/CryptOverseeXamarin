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
    public class HistoricalSettingsViewModel : SettingsViewModel, ISettings
    {
        private const string HIST_EXCHANGES = "HIST_EXCHANGES";
        private const string HIST_MARKETS = "HIST_MARKETS";
        
        public HistoricalSettingsViewModel()
        {
            MinAverageSpreadRounded = new ReactiveProperty<double>(0.5);
            MinAverageSpread = new ReactiveProperty<double>(0.5);
            MinOccurence = new ReactiveProperty<double>(10);
            DurationIsChecked1 = new ReactiveProperty<bool>();
            DurationIsChecked24 = new ReactiveProperty<bool>();
            DurationIsChecked48 = new ReactiveProperty<bool>();
            DurationIsChecked72 = new ReactiveProperty<bool>();
            DurationIsChecked168 = new ReactiveProperty<bool>();
            NumberHours = new ReactiveProperty<int>(48);

            NumberHours.Subscribe(_ =>
            {
                DurationIsChecked1.Value = _ == 1;
                DurationIsChecked24.Value = _ == 24;
                DurationIsChecked48.Value = _ == 48;
                DurationIsChecked72.Value = _ == 72;
                DurationIsChecked168.Value = _ == 168;
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
        public ReactiveProperty<bool> DurationIsChecked24 { get; set; }
        public ReactiveProperty<bool> DurationIsChecked48 { get; set; }
        public ReactiveProperty<bool> DurationIsChecked72 { get; set; }
        public ReactiveProperty<bool> DurationIsChecked168 { get; set; }


        public override void InitialiseSettings(List<ISpread> spreads)
        {
            InitialiseSettingsWithKey(spreads, HIST_EXCHANGES, null);
        }

        public IEnumerable<HistoricalSpreadModel> ApplySettings(IEnumerable<HistoricalSpreadModel> spreads)
        {
            try
            {
                var filteredSpreads = spreads.
                    Where(_ => _.GetSpreadOccurence(MinAverageSpread.Value) >= MinOccurence.Value &&
                               ShouldDisplayExchange(ExchangesVM.GetValues(), _.SellOn) && ShouldDisplayExchange(ExchangesVM.GetValues(), _.BuyOn)
                        // && ShouldDisplayMarket(_.Symbol)
                    );

                filteredSpreads = filteredSpreads.OrderByDescending(_ => _.SpreadOccurence).ToList();
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