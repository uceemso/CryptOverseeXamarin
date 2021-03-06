using System;
using System.Collections.Generic;
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
    public class MyPopupPageViewModel : ViewModelBase
    {

        private CancellationTokenSource _token = new CancellationTokenSource();

        public MyPopupPageViewModel()
        {

            Spread = new ReactiveProperty<Spread>();
            
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
        
        public HistoricalSpreadModel HistSpread { get; set; }
        public string Symbol { get; set; }
        public bool IsOn { get; set; }
        public double MinAverageSpread { get; set; }

        public ReactiveProperty<ChocoExchangesApi.Models.Spread> Spread { get; set; }


        //public void RefreshSpread(SupportedExchangeName exchangeA, SupportedExchangeName exchangeB, string baseCcy, string quoteCcy)
        //{
        //    Task.Factory.StartNew(async () =>
        //    {
        //        while (true)
        //        {
        //            Spread.Value = await Helpers.Test(exchangeA, exchangeB, baseCcy, quoteCcy);
        //            await Task.Delay(new TimeSpan(0, 0, 1));
        //            Console.WriteLine($"UPDATED SPREADS");

        //        }
        //    });
        //}        
        
        public void RefreshSpread(HistoricalSpreadModel item)
        {
            Symbol = item.Symbol;
            HistSpread = item;
            MinAverageSpread = MinAverageSpread;
            var exchangeA = (SupportedExchangeName)Enum.Parse(typeof(SupportedExchangeName), item.BuyOn);
            var exchangeB = (SupportedExchangeName)Enum.Parse(typeof(SupportedExchangeName), item.SellOn);
            
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    Spread.Value = await Helpers.Test(exchangeA, exchangeB, item.BaseCurrency, item.QuoteCurrency);
                    Console.WriteLine($"UPDATED SPREADS");

                }
            }, _token.Token);
        }

        public void CancelTask()
        {
            _token.Cancel();
        }

    }
}