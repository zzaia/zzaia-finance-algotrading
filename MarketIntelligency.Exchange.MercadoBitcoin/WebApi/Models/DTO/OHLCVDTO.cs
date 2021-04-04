using System.Text.Json.Serialization;

namespace MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class OHLCVDTO
    {

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("opening")]
        public decimal Open { get; set; }

        [JsonPropertyName("closing")]
        public decimal Close { get; set; }

        [JsonPropertyName("lowest")]
        public decimal Low { get; set; }

        [JsonPropertyName("highest")]
        public decimal High { get; set; }

        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }

        [JsonPropertyName("quantity")]
        public decimal TradedQuantity { get; set; }

        [JsonPropertyName("amount")]
        public int NumberOfTrades { get; set; }

        [JsonPropertyName("avg_price")]
        public decimal Average { get; set; }

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }

    }
}
