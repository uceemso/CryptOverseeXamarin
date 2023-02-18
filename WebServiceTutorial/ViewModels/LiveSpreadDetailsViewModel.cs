using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChocoExchangesApi.Models;
using ChocoExchangesApi.Services;
using CryptOverseeMobileApp.Models;
using OxyPlot;
using OxyPlot.Series;
using Reactive.Bindings;
using Spread = ChocoExchangesApi.Models.Spread;

namespace CryptOverseeMobileApp.ViewModels
{
    public class LiveSpreadDetailsViewModel : ViewModelBase
    {

        private CancellationTokenSource _token;

        public LiveSpreadDetailsViewModel()
        {

            Spread = new ReactiveProperty<Spread>();
            LastUpdate = new ReactiveProperty<DateTime>();
            Symbol = new ReactiveProperty<string>();
            BaseCurrency = new ReactiveProperty<string>();
            QuoteCurrency = new ReactiveProperty<string>();
            ExchangeA = new ReactiveProperty<string>();
            ExchangeB = new ReactiveProperty<string>();
            Note = new ReactiveProperty<string>();
            HasWarning = new ReactiveProperty<bool>();
            SpreadModel = new ReactiveProperty<SpreadModel>();
            BuyPrice = new ReactiveProperty<double>();
            SellPrice = new ReactiveProperty<double>();
            OtherDirectionBuyPrice = new ReactiveProperty<double>();
            OtherDirectionSellPrice = new ReactiveProperty<double>();

            DataPlot = new PlotModel();
            //DataPlot.Series.Add(new LineSeries( ));
            DataPlot.Series.Add(new LineSeries
            {
                Color = OxyColors.CornflowerBlue,
                Title = "Value1",
                DataFieldX = "Date",
                DataFieldY = "Value"
            });
            DataPlot.Series.Add(new LineSeries
            {
                Color = OxyColors.IndianRed,
                Title = "Value2",
                DataFieldX = "Date",
                DataFieldY = "Value"
            });
            //DataPlot.Series.Add(new LineSeries { Color = OxyColor.FromArgb(1, 255, 0, 0) });

        }

        public PlotModel DataPlot { get; set; }

        public ReactiveProperty<SpreadModel> SpreadModel { get; private set; }
        public ReactiveProperty<string> Symbol { get; private set; }
        public ReactiveProperty<string> BaseCurrency { get; private set; }
        public ReactiveProperty<string> QuoteCurrency { get; private set; }
        public ReactiveProperty<string> ExchangeA { get; private set; }
        public ReactiveProperty<string> ExchangeB { get; private set; }
        public bool IsOn { get; set; }
        public double MinAverageSpread { get; set; }
        public ReactiveProperty<Spread> Spread { get; set; }
        public ReactiveProperty<DateTime> LastUpdate { get; set; }
        public ReactiveProperty<double> BuyPrice { get; set; }
        public ReactiveProperty<double> SellPrice { get; set; }
        public ReactiveProperty<double> OtherDirectionBuyPrice { get; set; }
        public ReactiveProperty<double> OtherDirectionSellPrice { get; set; }
        public ReactiveProperty<string> Note { get; set; }
        public ReactiveProperty<bool> HasWarning { get; set; }

        public void StartLiveFeed(SpreadModel item, List<SpreadNote> notes)
        {
            _token = new CancellationTokenSource();
            Symbol.Value = item.Symbol;
            BaseCurrency.Value = item.BaseCurrency;
            QuoteCurrency.Value = item.QuoteCurrency;
            ExchangeA.Value = item.BuyOn;
            ExchangeB.Value = item.SellOn;
            SpreadModel.Value = item;
            MinAverageSpread = MinAverageSpread;
            Spread.Value = null;



            Note.Value = item.Warning;
            HasWarning.Value = item.HasWarning;

            var exchangeA = (SupportedExchangeName)Enum.Parse(typeof(SupportedExchangeName), item.BuyOn);
            var exchangeB = (SupportedExchangeName)Enum.Parse(typeof(SupportedExchangeName), item.SellOn);
            
            Task.Factory.StartNew(async () =>
            {
                var st = Stopwatch.StartNew();
                while (!_token.Token.IsCancellationRequested)
                {
                    Spread.Value = await Helpers.Test(exchangeA, exchangeB, item.BaseCurrency, item.QuoteCurrency);

                    BuyPrice.Value = (double) Spread.Value.AskA;
                    SellPrice.Value = (double) Spread.Value.BidB;
                    OtherDirectionBuyPrice.Value = (double)Spread.Value.AskB;
                    OtherDirectionSellPrice.Value = (double)Spread.Value.BidA;

                    SpreadModel.Value.SellPrice = (double) Spread.Value.BidB;
                    LastUpdate.Value = DateTime.Now;
                    var time = st.ElapsedMilliseconds / 1000;
                    Console.WriteLine($"[{LastUpdate.Value}] UPDATED SPREADS {item.Symbol}, time: {time} sec, " +
                                      $"BuyPrice.Value {BuyPrice.Value}, SellPrice.Value {SellPrice.Value}");

                    var buySeries = DataPlot.Series[0] as LineSeries;
                    var sellSeries = DataPlot.Series[1] as LineSeries;

                    if (!_token.Token.IsCancellationRequested)
                    {
                        buySeries?.Points.Add(new DataPoint(time, BuyPrice.Value));
                        sellSeries?.Points.Add(new DataPoint(time, SellPrice.Value));
                        //sellSeries?.Points.Add(new DataPoint(time, SellPrice.Value));
                        DataPlot.InvalidatePlot(true);
                    }


                }

                if (_token.Token.IsCancellationRequested)
                {
                    (DataPlot.Series[0] as LineSeries)?.Points.Clear();
                    (DataPlot.Series[1] as LineSeries)?.Points.Clear();
                }
            }, _token.Token);
        }

        public void Dispose()
        {
            Console.WriteLine($"Disposing of live stream for {Symbol.Value}");
            _token.Cancel();
        }

    }
}