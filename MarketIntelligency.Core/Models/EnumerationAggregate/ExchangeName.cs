namespace MarketIntelligency.Core.Models.EnumerationAggregate
{
    /*========================================================================================================
        Supported Exchanges by Name
      ======================================================================================================= */
    public class ExchangeName : Enumeration
    {
        public static readonly ExchangeName MercadoBitcoin = new ExchangeName(1, "Mercado Bitcoin");
        public ExchangeName() { }

        private ExchangeName(int value, string displayName) : base(value, displayName) { }
    }
}