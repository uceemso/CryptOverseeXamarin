using System;
using Newtonsoft.Json;

namespace CryptOverseeMobileApp.Models
{
    public class Spread : ISpread
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

        public override string ToString()
        {
            return $"{Symbol} {BuyOn}-{SellOn} Spd: {SpreadValue}, Vol: {VolumeInQuoteCcy}";
        }
    }
}