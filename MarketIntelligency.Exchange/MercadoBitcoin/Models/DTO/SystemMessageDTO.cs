using System.Text.Json.Serialization;

namespace MarketIntelligency.Exchange.MercadoBitcoin.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class SystemMessageDTO
    {
        [JsonPropertyName("msg_date")]
        public string Date { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; }

        [JsonPropertyName("event_code")]
        public string Code { get; set; }

        [JsonPropertyName("msg_content")]
        public string Content { get; set; }

    }
}
