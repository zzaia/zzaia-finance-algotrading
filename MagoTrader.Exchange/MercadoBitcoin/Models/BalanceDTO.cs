using System.Text.Json.Serialization;

namespace MarketMaker.Exchange.MercadoBitcoin.Models
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class BalanceDTO
    {
        [JsonPropertyName("available")]
        public string Available { get; set; }

        [JsonPropertyName("total")]
        public string Total { get; set; }

        [JsonPropertyName("amount_open_orders")]
        public int Used { get; set; }

    }
}
