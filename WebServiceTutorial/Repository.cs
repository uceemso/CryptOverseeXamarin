﻿using Newtonsoft.Json;
using System;

namespace CryptOverseeMobileApp
{
    public class Repository
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("html_url")]
        public Uri GitHubHomeUrl { get; set; }

        [JsonProperty("homepage")]
        public Uri Homepage { get; set; }

        [JsonProperty("watchers")]
        public int Watchers { get; set; }
    }
}
