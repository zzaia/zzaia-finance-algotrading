using System.Text.Json.Serialization;

namespace MagoTrader.Exchange.MercadoBitcoin.Models
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class WithdrawLimitsDTO
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
    }
}
