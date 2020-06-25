using System.Text.Json.Serialization;

namespace MagoTrader.Exchange.MercadoBitcoin.Private
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    class BalanceDTO
    {
        [JsonPropertyName("available")]
        public string Available { get; set; }

        [JsonPropertyName("total")]
        public string Total { get; set; }

        [JsonPropertyName("amount_open_orders")]
        public int Used { get; set; }

    }
}
