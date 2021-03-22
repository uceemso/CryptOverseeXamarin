using System.Collections.Generic;
using CryptOverseeMobileApp.Models;
using Microcharts;
using Microcharts.Forms;
using OxyPlot;
using OxyPlot.Series;
using SkiaSharp;
using Xamarin.Forms;

namespace CryptOverseeMobileApp.ViewModels
{
    public class MyPopupPageViewModel : ViewModelBase
    {
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
        
        

        public Model Model { get; set; }
        public Model Model2 { get; set; }
        public Model Model3 { get; set; }
        public Chart Chart { get; set; }
        
        public HistoricalSpreadModel HistSpread { get; set; }
        public string Symbol { get; set; }
        public bool IsOn { get; set; }
        public double MinAverageSpread { get; set; }


    }
}