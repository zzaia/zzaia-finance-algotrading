using System.Text.Json.Serialization;

namespace MarketIntelligency.Exchange.MercadoBitcoin.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class TradeDTO
    {
        [JsonPropertyName("date")]
        public int TimeStamp { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("tid")]
        public int Tid { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
