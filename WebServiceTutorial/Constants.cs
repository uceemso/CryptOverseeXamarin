namespace CryptOverseeMobileApp
{
    public static class Constants
    {
        public const string SpreadEndpoint = "https://cryptoverseeserver.azurewebsites.net/spread/getSpreadsForGivenSymbolAndExchanges/binance/binanceUs/HNT_USDT/1";
        public const string HistoricalSpreadEndpoint = "https://cryptoverseeserver.azurewebsites.net/spread/spreadanalyserallexchanges/";

        //public const string GetRecentSpreads = "https://localhost:6001/spread/getRecentSpreads";
        public const string GetRecentSpreads = "https://cryptoverseeserver.azurewebsites.net/spread/getRecentSpreads360";

        public const string GetRecentSpreads2 = "https://cryptoverseeserver.azurewebsites.net/spread/getRecentSpreads2";
        
        public const string MessagingCenter_PremiumOn = "premium_on";
        public const string MessagingCenter_PremiumOff = "premium_off";
        public const string MessagingCenter_HistoricalSettingsClosed = "popped2";
        public const string MessagingCenter_LiveSettingsClosed = "live_spread_settings_popped";


    }
}
