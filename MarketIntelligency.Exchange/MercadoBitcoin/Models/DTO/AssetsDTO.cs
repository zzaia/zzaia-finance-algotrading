using System.Text.Json.Serialization;

namespace MarketIntelligency.Exchange.MercadoBitcoin.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class AssetsDTO
    {
        [JsonPropertyName("bch")]
        public BalanceDTO BCH { get; set; }

        [JsonPropertyName("brl")]
        public BalanceDTO BRL { get; set; }

        [JsonPropertyName("btc")]
        public BalanceDTO BTC { get; set; }

        [JsonPropertyName("eth")]
        public BalanceDTO ETH { get; set; }

        [JsonPropertyName("ltc")]
        public BalanceDTO LTC { get; set; }

        [JsonPropertyName("xrp")]
        public BalanceDTO XRP { get; set; }

        [JsonPropertyName("mbprk01")]
        public BalanceDTO MBPRK01 { get; set; }

        [JsonPropertyName("mbprk02")]
        public BalanceDTO MBPRK02 { get; set; }

        [JsonPropertyName("mbprk03")]
        public BalanceDTO MBPRK03 { get; set; }

        [JsonPropertyName("mbprk04")]
        public BalanceDTO MBPRK04 { get; set; }

        [JsonPropertyName("mbcons01")]
        public BalanceDTO MBCONS01 { get; set; }

        [JsonPropertyName("usdc")]
        public BalanceDTO USDC { get; set; }
    }
}
