using System.Text.Json.Serialization;

namespace MarketIntelligency.Exchange.MercadoBitcoin.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class TickerDTO
    {
        [JsonPropertyName("high")]
        public string High { get; set; }

        [JsonPropertyName("low")]
        public string Low { get; set; }

        [JsonPropertyName("vol")]
        public string Volume { get; set; }

        [JsonPropertyName("last")]
        public string Last { get; set; }

        [JsonPropertyName("buy")]
        public string Buy { get; set; }

        [JsonPropertyName("sell")]
        public string Sell { get; set; }

        [JsonPropertyName("date")]
        public int TimeStamp { get; set; }

    }

    public class TickerDataDTO
    {
        [JsonPropertyName("ticker")]
        public TickerDTO Ticker { get; set; }

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }

    }
}
