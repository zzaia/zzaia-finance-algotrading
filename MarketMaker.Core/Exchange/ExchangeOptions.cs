namespace MarketMaker.Core.Exchange
{
    public class ExchangeOptions
    {
        public string PrivateClientCredentialReference { get; set; }
        public string TradeClientCredentialReference { get; set; }
        public bool HasAlreadyAuthenticatedSuccessfully { get; set; }
    }
}
