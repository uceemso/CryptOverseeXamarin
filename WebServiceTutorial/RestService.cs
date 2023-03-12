using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CryptOverseeMobileApp.Exceptions;
using CryptOverseeMobileApp.Models;
using Newtonsoft.Json.Linq;
using ChocoExchangesApi.Models;

namespace CryptOverseeMobileApp
{
    public class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
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

        public async Task<ChocoExchangeResultSpread> GetSpreadsAsync(string uri)
        {
            try
            {
                var content = await GetResponse(uri);
                return JsonConvert.DeserializeObject<ChocoExchangeResultSpread>(content);
            }
            catch (Exception ex)
            {
                
                return new ChocoExchangeResultSpread
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<ChocoExchangeResultHistoricalSpread> GetHistoricalSpreadsFromFileShare(int i)
        {
            try
            {
                var line = await ChocoExchangesApi.Services.AzureHelpers.ReadFileToAzure($"GetHistoricalSpreads_{i}.txt");
                var result = JObject.Parse(line).ToObject<ChocoExchangeResultHistoricalSpread>();
                return result;
            }
            catch (Exception ex)
            {
                return new ChocoExchangeResultHistoricalSpread
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<ChocoExchangeResultSpread> GetSpreadsFromFileShare()
        {
            try
            {
                var line = await ChocoExchangesApi.Services.AzureHelpers.ReadFileToAzure("spreads2.txt");
                var result = JObject.Parse(line).ToObject<ChocoExchangeResultSpread>();
                return result;
            }
            catch (Exception ex)
            {
                return new ChocoExchangeResultSpread
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<List<SpreadNote>> GetExchangeNotesFromFileShare()
        {
            try
            {
                var line = await ChocoExchangesApi.Services.AzureHelpers.ReadFileToAzure("SpreadNotes.txt");
                var result = JArray.Parse(line).ToObject<List<SpreadNote>>();
                //var result = JObject.Parse(line).ToObject<List<SpreadNote>>();
                return result;
            }
            catch (Exception ex)
            {
                return new List<SpreadNote>();
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
