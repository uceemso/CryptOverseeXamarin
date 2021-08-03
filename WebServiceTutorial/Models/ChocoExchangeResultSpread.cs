using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CryptOverseeMobileApp.Models
{
    public class ChocoExchangeResultSpread
    {
        [JsonProperty("data")]
        public List<SpreadModel> Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("dateTime")]
        public DateTime DateTime { get; set; }


    }
}