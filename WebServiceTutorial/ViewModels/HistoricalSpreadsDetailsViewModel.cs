using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChocoExchangesApi.Models;
using ChocoExchangesApi.Services;
using CryptOverseeMobileApp.Models;
using CryptOverseeMobileApp.ViewModels.Settings;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Reactive.Bindings;
using Spread = ChocoExchangesApi.Models.Spread;

namespace CryptOverseeMobileApp.ViewModels
{
    public class HistoricalSpreadsDetailsViewModel : ViewModelBase
    {

        private CancellationTokenSource _token;

        public HistoricalSpreadsDetailsViewModel()
        {

            NbDataPoints = new ReactiveProperty<int>();
            NbBlanks = new ReactiveProperty<int>();
            NbDaysDataWasCollectedPercentage = new ReactiveProperty<int>();
            NbDaysDataWasCollected = new ReactiveProperty<int>();
            SpreadOccurence = new ReactiveProperty<double>();
            PositiveSpreadOccurence = new ReactiveProperty<double>();
            AverageSpreadValue = new ReactiveProperty<double>();
            MinAverageSpread = new ReactiveProperty<double>();
            MaxSpreadValue = new ReactiveProperty<double>();
            MinSpreadValue = new ReactiveProperty<double>();


            Spread = new ReactiveProperty<Spread>();
            LastUpdate = new ReactiveProperty<DateTime>();
            Symbol = new ReactiveProperty<string>();
            BaseCurrency = new ReactiveProperty<string>();
            QuoteCurrency = new ReactiveProperty<string>();
            ExchangeA = new ReactiveProperty<string>();
            ExchangeB = new ReactiveProperty<string>();
            HistoricalSpreadModel = new ReactiveProperty<HistoricalSpreadModel>();
            BuyPrice = new ReactiveProperty<double>();
            SellPrice = new ReactiveProperty<double>();

            HistoricalSpreadDataPlot = new PlotModel();
            HistoricalSpreadDataPlot.Series.Add(new LineSeries
            {
                MarkerFill = OxyColors.Blue,
                MarkerType = MarkerType.Circle,
                LineStyle = LineStyle.None
            });
            HistoricalSpreadDataPlot.Series.Add(new LineSeries
            {
                MarkerFill = OxyColors.Green,
                MarkerType = MarkerType.Circle,
                LineStyle = LineStyle.None
            });

            DataPlot = new PlotModel();
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


        }

        public PlotModel HistoricalSpreadDataPlot { get; set; }
        public PlotModel DataPlot { get; set; }

        public ReactiveProperty<HistoricalSpreadModel> HistoricalSpreadModel { get; private set; }
        public ReactiveProperty<string> Symbol { get; private set; }
        public ReactiveProperty<string> BaseCurrency { get; private set; }
        public ReactiveProperty<string> QuoteCurrency { get; private set; }
        public ReactiveProperty<string> ExchangeA { get; private set; }
        public ReactiveProperty<string> ExchangeB { get; private set; }
        public bool IsOn { get; set; }
        public ReactiveProperty<double> MinAverageSpread { get; set; }
        public ReactiveProperty<double> MaxSpreadValue { get; set; }
        public ReactiveProperty<double> MinSpreadValue { get; set; }
        public ReactiveProperty<Spread> Spread { get; set; }
        public ReactiveProperty<DateTime> LastUpdate { get; set; }
        public ReactiveProperty<double> BuyPrice { get; set; }
        public ReactiveProperty<double> SellPrice { get; set; }


        public ReactiveProperty<int> NbDataPoints { get; set; }
        public ReactiveProperty<int> NbBlanks { get; set; }
        public ReactiveProperty<int> NbDaysDataWasCollectedPercentage { get; set; }
        public ReactiveProperty<int> NbDaysDataWasCollected { get; set; }
        public ReactiveProperty<double> SpreadOccurence { get; set; }
        public ReactiveProperty<double> PositiveSpreadOccurence { get; set; }
        public ReactiveProperty<double> AverageSpreadValue { get; set; }

        public void StartLiveFeed(HistoricalSpreadModel item, HistoricalSettingsViewModel settings)
        {
            _token = new CancellationTokenSource();
            HistoricalSpreadModel.Value = item;

            var startDate = DateTime.Now.AddDays(-10);
            var endDate = DateTime.Now;

            var minValue = DateTimeAxis.ToDouble(item.Spreads.First().Date.Value);
            var maxValue = DateTimeAxis.ToDouble(item.Spreads.Last().Date.Value);
            HistoricalSpreadDataPlot.Axes.Clear();

            HistoricalSpreadDataPlot.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "hh:mm",
                Minimum = minValue, 
                //Maximum = maxValue,
            });


            foreach (var spreadPoint in item.Spreads)
            {
                var historicalSpreadSeries = HistoricalSpreadDataPlot.Series[0] as LineSeries;
                if (spreadPoint.Date.HasValue) historicalSpreadSeries?.Points.Add(new DataPoint(DateTimeAxis.ToDouble(spreadPoint.Date.Value), (double)spreadPoint.Value));
            }
            HistoricalSpreadDataPlot.InvalidatePlot(true);



            NbDataPoints.Value = item.NbDataPoints;
            NbBlanks.Value = item.NbBlanks;
            SpreadOccurence.Value = item.GetSpreadOccurence(settings.MinAverageSpread.Value);
            PositiveSpreadOccurence.Value = item.NbDataPoints / item.NbBlanks * 100;
            AverageSpreadValue.Value = item.AverageSpreadValue;
            NbDaysDataWasCollectedPercentage.Value = item.NbDaysDataWasCollectedPercentage;
            NbDaysDataWasCollected.Value = item.NbDaysDataWasCollected;
            MinAverageSpread.Value = settings.MinAverageSpread.Value;
            MaxSpreadValue.Value = item.MaxSpreadValue;
            MinSpreadValue.Value = item.MinSpreadValue;

            Symbol.Value = item.Symbol;
            BaseCurrency.Value = item.BaseCurrency;
            QuoteCurrency.Value = item.SellOn;
            ExchangeA.Value = item.BuyOn;
            ExchangeB.Value = item.SellOn;
            HistoricalSpreadModel.Value = item;
            MinAverageSpread = MinAverageSpread;
            Spread.Value = null;

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
                    //HistoricalSpreadModel.Value.SellPrice = (double) Spread.Value.BidB;
                    LastUpdate.Value = DateTime.Now;
                    var time = st.ElapsedMilliseconds / 1000;
                    Console.WriteLine($"[{LastUpdate.Value}] UPDATED SPREADS {item.Symbol}, time: {time} sec, " +
                                      $"BuyPrice.Value {BuyPrice.Value}, SellPrice.Value {SellPrice.Value}");





                    if (!_token.Token.IsCancellationRequested)
                    {
                        var buySeries = DataPlot.Series[0] as LineSeries;
                        var sellSeries = DataPlot.Series[1] as LineSeries;
                        buySeries?.Points.Add(new DataPoint(time, BuyPrice.Value));
                        sellSeries?.Points.Add(new DataPoint(time, SellPrice.Value));

                        //sellSeries?.Points.Add(new DataPoint(time, SellPrice.Value));
                        DataPlot.InvalidatePlot(true);

                        
                        if (Spread.Value != null)
                        {
                            var historicalSpreadSeries = HistoricalSpreadDataPlot.Series[1] as LineSeries;
                            historicalSpreadSeries?.Points.Add(new DataPoint(DateTimeAxis.ToDouble(LastUpdate.Value), (double)Spread.Value.SpreadBuyOnASellOnB));
                            HistoricalSpreadDataPlot.InvalidatePlot(true);
                        }
                    }


                }

                if (_token.Token.IsCancellationRequested)
                {
                    (DataPlot.Series[0] as LineSeries)?.Points.Clear();
                    (DataPlot.Series[1] as LineSeries)?.Points.Clear();
                    (HistoricalSpreadDataPlot.Series[0] as LineSeries)?.Points.Clear();
                    (HistoricalSpreadDataPlot.Series[1] as LineSeries)?.Points.Clear();

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