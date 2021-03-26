using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using CryptOverseeMobileApp.Exceptions;
using CryptOverseeMobileApp.Models;

namespace CryptOverseeMobileApp
{
    public class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<List<PhotoVote>> GetPhotoVotes()
        {
            var url = $"{Constants.GetPhotoVotesEndpoint}";
            var content = await GetResponse(url);
            try
            {
                return JsonConvert.DeserializeObject<List<PhotoVote>>(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't parse the response of {url}", ex);
            }
        }

        public async Task<List<HistoricalSpreadModel>> GetHistoricalSpreadsAsync(int nberHours)
        {
            var url = Constants.HistoricalSpreadEndpoint + nberHours;
            var content = await GetResponse(url);
            try
            {
                return JsonConvert.DeserializeObject<List<HistoricalSpreadModel>>(content);
            }
            catch (Exception ex)
            {
                throw new RestCallParsingException($"Couldn't parse the response of {Constants.HistoricalSpreadEndpoint}", ex);
            }
        }

        public async Task<List<Spread>> GetSpreadsAsync(string uri)
        {
            var content = await GetResponse(uri);
            try
            {
                return JsonConvert.DeserializeObject<List<Spread>>(content);
            }
            catch (Exception ex)
            {
                throw new RestCallParsingException($"Couldn't parse the response of {uri}", ex);
            }
        }

        private async Task<string> GetResponse(string uri)
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                throw new RestCallException($"Error when calling {uri} StatusCode: {response.StatusCode}");
            }

        }
    }
}
