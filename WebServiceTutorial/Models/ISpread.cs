namespace CryptOverseeMobileApp.Models
{
    public interface ISpread
    {
        public string BuyOn { get; set; }
        public string SellOn { get; set; }
        public string QuoteCurrency { get; }
        public string Symbol { get; set; }
    }
}