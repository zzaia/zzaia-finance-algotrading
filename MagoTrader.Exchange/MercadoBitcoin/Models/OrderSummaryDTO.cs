using System.Text.Json.Serialization;

namespace MagoTrader.Exchange.MercadoBitcoin.Models
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
