using System.Linq;

namespace MarketIntelligency.Core.Models.EnumerationAggregate
{
    /*========================================================================================================
        Supported Exchanges by Name
      ======================================================================================================= */
    public class ExchangeName : Enumeration
    {
        public static readonly ExchangeName MercadoBitcoin = new(1, "Mercado Bitcoin");
        public static readonly ExchangeName Binance = new(2, "Binance");
        public static readonly ExchangeName Coinbase = new(3, "Coinbase");
        public static readonly ExchangeName Ftx = new(3, "Ftx");
        public ExchangeName() { }

        private ExchangeName(int value, string displayName) : base(value, displayName) { }

        public static bool IsValid(string displayName)
        {
            var listOfallCurrencyNames = GetAll<ExchangeName>().Select(a => a.DisplayName).ToList();
            return listOfallCurrencyNames.Contains(displayName);
        }

        public static bool IsValid(int value)
        {
            var listOfallCurrencyNames = GetAll<ExchangeName>().Select(a => a.Value).ToList();
            return listOfallCurrencyNames.Contains(value);
        }
    }
}