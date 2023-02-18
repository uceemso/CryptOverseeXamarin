using System;
using System.Linq;
using Newtonsoft.Json;

namespace CryptOverseeMobileApp.Models
{
    public class SpreadModel : ISpread
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("buyOnExchange")]
        public string BuyOn { get; set; }

        [JsonProperty("sellOnExchange")]
        public string SellOn { get; set; }

        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; }

        [JsonProperty("spreadValue")]
        public double SpreadValue { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("sameBaseQuote")]
        public string SameBaseQuote { get; set; }

        [JsonProperty("volumeInQuoteCcy")]
        public double? VolumeInQuoteCcy { get; set; }

        [JsonProperty("spot")]
        public double Spot { get; set; }

        [JsonProperty("buyPrice")]
        public double BuyPrice { get; set; }

        [JsonProperty("sellPrice")]
        public double SellPrice { get; set; }

        public string BaseCurrency => Symbol.Split('/').First();
        public string QuoteCurrency => Symbol.Split('/').Last();

        public string Warning { get; set; }
        public bool HasWarning { get; set; }

        public override string ToString()
        {
            return $"{Symbol} {BuyOn}-{SellOn} Spd: {SpreadValue}, Vol: {VolumeInQuoteCcy}";
        }
    }
}