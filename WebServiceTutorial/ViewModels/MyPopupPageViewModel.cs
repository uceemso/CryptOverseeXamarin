using System;
using System.Collections.Generic;
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
using Microcharts.Forms;
using OxyPlot;
using OxyPlot.Series;
using SkiaSharp;
using Xamarin.Forms;
using Spread = CryptOverseeMobileApp.Models.Spread;

namespace CryptOverseeMobileApp.ViewModels
{
    public class MyPopupPageViewModel : ViewModelBase
    {

        
        public MyPopupPageViewModel()
        {
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

        public static PlotModel LineSeries()
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


        public static PlotModel LineSeriesExample3()
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

        //private void Test()
        //{
        //    var clientManager = new ExchangeClientsManager();

        //    var clientA = GetClient(exchangeA);
        //    var marketsA = clientA.GetExchangeMarketsAsync().Result.Data.ToList();
        //    var marketA = marketsA.FirstOrDefault(_ => _.BaseCurrencyGlobal == baseCcy && _.QuoteCurrencyGlobal == quoteCcy);
        //    var pA = clientManager.GetTask(marketA).Result;

        //    var clientB = GetClient(exchangeB);
        //    var marketsB = clientB.GetExchangeMarketsAsync().Result.Data.ToList();
        //    var marketB = marketsB.FirstOrDefault(_ => _.BaseCurrencyGlobal == baseCcy && _.QuoteCurrencyGlobal == quoteCcy);
        //    var pB = clientManager.GetTask(marketB).Result;

        //    var spread = new Spread(pA, pB, true);
        //}

        //private IChocoExchangeApi GetClient(SupportedExchangeName name)
        //{
        //    switch (name)
        //    {
        //        case SupportedExchangeName.Kraken:
        //            return new MyKrakenClient();
        //        case SupportedExchangeName.Huobi:
        //            return new MyHuobiClient();
        //        case SupportedExchangeName.HitBTC:
        //            return new MyHitBtcClient();
        //        case SupportedExchangeName.KuCoin:
        //            return new MyKucoinClient();
        //        case SupportedExchangeName.Binance:
        //            return new MyBinanceClient(BinanceRegion.BinanceUk);
        //        case SupportedExchangeName.BinanceUs:
        //            return new MyBinanceClient(BinanceRegion.BinanceUs);
        //        case SupportedExchangeName.Bitfinex:
        //            return new MyBitfinexClient();
        //        case SupportedExchangeName.Bittrex:
        //            return new MyBitrexClient();
        //        default:
        //            throw new Exception("You need to add the new client in method GetClient");
        //    }

        //}
    }
}