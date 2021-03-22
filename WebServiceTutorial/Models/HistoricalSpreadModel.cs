using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptOverseeMobileApp.Models
{
    public class HistoricalSpreadModel : ISpread
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("buyOn")]
        public string BuyOn { get; set; }

        [JsonProperty("sellOn")]
        public string SellOn { get; set; }

        [JsonProperty("nbDaysDataWasCollected")]
        public int NbDaysDataWasCollected { get; set; }

        [JsonProperty("nbDaysDataWasCollectedPercentage")]
        public int NbDaysDataWasCollectedPercentage { get; set; }

        [JsonProperty("nbBlanks")]
        public int NbBlanks { get; set; }

        [JsonProperty("totalNbDaysForTheseExchanges")]
        public int TotalNbDaysForTheseExchanges { get; set; }

        [JsonProperty("spreads")]
        public List<double> Spreads { get; set; }

        //[JsonProperty("positiveSpreadOccurence")]
        //public int PositiveSpreadOccurence { get; set; }

        //[JsonProperty("customSpreadOccurence")]
        //public int CustomSpreadOccurence { get; set; }

        [JsonProperty("nbDataPoints")]
        public int NbDataPoints { get; set; }

        [JsonProperty("averageSpreadValue")]
        public double AverageSpreadValue { get; set; }

        [JsonProperty("minSpreadValue")]
        public int MinSpreadValue { get; set; }

        [JsonProperty("maxSpreadValue")]
        public double MaxSpreadValue { get; set; }

        public double SpreadOccurence { get; set; }
        public double GetSpreadOccurence(double minSpread)
        {
            var count = Spreads.Count(_ => _ > minSpread);
            var occurence = Math.Round(count / (double)NbBlanks * 100);
            return occurence;
        }
    }
}