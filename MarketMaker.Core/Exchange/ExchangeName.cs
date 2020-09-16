namespace MarketMaker.Core.Exchange
{
    /*========================================================================================================
        Supported Exchanges
      ======================================================================================================= */
    public class ExchangeName : Enumeration
    {
        public static readonly ExchangeName MercadoBitcoin = new ExchangeName(1, "Mercado Bitcoin");
        public ExchangeName() { }

        private ExchangeName(int value, string displayName) : base(value, displayName) { }
    }
}