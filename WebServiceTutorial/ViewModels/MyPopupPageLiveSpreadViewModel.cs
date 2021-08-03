using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChocoExchangesApi;
using ChocoExchangesApi.Exchanges.Binance;
using ChocoExchangesApi.Exchanges.Bitfinex;
using ChocoExchangesApi.Exchanges.Bittrex;
using ChocoExchangesApi.Exchanges.HitBtc;
using ChocoExchangesApi.Exchanges.Huobi;
using ChocoExchangesApi.Exchanges.Kraken;
using ChocoExchangesApi.Exchanges.Kucoin;
using ChocoExchangesApi.Models;
using ChocoExchangesApi.Services;
using CryptOverseeMobileApp.Models;
using Microcharts;
using OxyPlot;
using OxyPlot.Series;
using Reactive.Bindings;
using SkiaSharp;
using Spread = ChocoExchangesApi.Models.Spread;

namespace CryptOverseeMobileApp.ViewModels
{
    public class MyPopupPageLiveSpreadViewModel : ViewModelBase
    {

        private CancellationTokenSource _token;

        public MyPopupPageLiveSpreadViewModel()
        {

            Spread = new ReactiveProperty<Spread>();
            LastUpdate = new ReactiveProperty<DateTime>();

            Model = new PlotModel {Title = "Hello, Forms!"};
            Model2 = LineSeries();
            Model3 = LineSeriesExample3();

                
            var entries = new List<ChartEntry>()
            {
                new ChartEntry(50)
                {
                    Label = "Android",
                    ValueLabel = "248",
                    Color = SKColor.Parse("#77d065")
                },
                new ChartEntry(100)
                {
                    Label = "iOS",
                    ValueLabel = "128",
                    Color = SKColor.Parse("#b455b6")
                },
                new ChartEntry(150)
                {
                    Label = "Shared",
                    ValueLabel = "514",
                    Color = SKColor.Parse("#3498db")
                }
            };
            
            Chart = new LineChart()
            {
                Entries = entries
            };
        }

        private static PlotModel LineSeries()
        {
            var model = new PlotModel { Title = "LineSeries", Subtitle = "Smooth = true" };
            var lineSeries = new LineSeries();
            lineSeries.Points.Add(new DataPoint(0, 0));
            lineSeries.Points.Add(new DataPoint(10, 4));
            lineSeries.Points.Add(new DataPoint(30, 2));
            lineSeries.Points.Add(new DataPoint(40, 12));
            model.Series.Add(lineSeries);
            return model;
        }


        private static PlotModel LineSeriesExample3()
        {
            var model = new PlotModel { Title = "LineSeries", Subtitle = "MarkerType = Circle" };
            var lineSeries = new LineSeries { MarkerType = MarkerType.Circle };
            lineSeries.Points.Add(new DataPoint(0, 0));
            lineSeries.Points.Add(new DataPoint(10, 4));
            lineSeries.Points.Add(new DataPoint(30, 2));
            lineSeries.Points.Add(new DataPoint(40, 12));
            model.Series.Add(lineSeries);
            return model;
        }

        public Model Model { get; set; }
        public Model Model2 { get; set; }
        public Model Model3 { get; set; }
        public Chart Chart { get; set; }
        public SpreadModel SpreadModel { get; private set; }
        public string Symbol { get; private set; }
        public string BaseCurrency { get; private set; }
        public string QuoteCurrency { get; private set; }
        public string ExchangeA { get; private set; }
        public string ExchangeB { get; private set; }
        public bool IsOn { get; set; }
        public double MinAverageSpread { get; set; }
        public ReactiveProperty<Spread> Spread { get; set; }
        public ReactiveProperty<DateTime> LastUpdate { get; set; }



        public void StartLiveFeed(SpreadModel item)
        {
            _token = new CancellationTokenSource();
            Symbol = item.Symbol;
            BaseCurrency = item.BaseCurrency;
            QuoteCurrency = item.SellOn;
            ExchangeA = item.BuyOn;
            SpreadModel = item;
            MinAverageSpread = MinAverageSpread;
            Spread.Value = null;

            var exchangeA = (SupportedExchangeName)Enum.Parse(typeof(SupportedExchangeName), item.BuyOn);
            var exchangeB = (SupportedExchangeName)Enum.Parse(typeof(SupportedExchangeName), item.SellOn);
            
            Task.Factory.StartNew(async () =>
            {
                while (!_token.Token.IsCancellationRequested)
                {
                    Spread.Value = await Helpers.Test(exchangeA, exchangeB, item.BaseCurrency, item.QuoteCurrency);
                    LastUpdate.Value = DateTime.Now;
                    Console.WriteLine($"[{LastUpdate.Value}] UPDATED SPREADS {item.Symbol}");

                }
            }, _token.Token);
        }

        public void Dispose()
        {
            _token.Cancel();
        }

    }
}