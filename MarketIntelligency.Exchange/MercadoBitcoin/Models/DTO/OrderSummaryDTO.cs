using System.Text.Json.Serialization;

namespace MarketIntelligency.Exchange.MercadoBitcoin.Models.DTO
{
    public class OrderSummaryDTO
    {
        [JsonPropertyName("order_id")]
        public int Id { get; set; }

        [JsonPropertyName("is_owner")]
        public bool IsOwner { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("limit_price")]
        public string PriceLimit { get; set; }
    }
}
