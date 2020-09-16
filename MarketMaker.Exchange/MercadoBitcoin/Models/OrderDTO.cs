using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MarketMaker.Exchange.MercadoBitcoin.Models
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    /// 
    public class OrderDTO
    {
        [JsonPropertyName("order_id")]
        public int Id { get; set; }

        [JsonPropertyName("coin_pair")]
        public string PairTicker { get; set; }

        [JsonPropertyName("order_type")]
        public int OrderType { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("has_fills")]
        public bool HasFills { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("cost")]
        public string Cost { get; set; }

        [JsonPropertyName("limit_price")]
        public string PriceLimit { get; set; }

        [JsonPropertyName("executed_quantity")]
        public string ExecutedQuantity { get; set; }

        [JsonPropertyName("executed_price_avg")]
        public string ExecutedPriceAverage { get; set; }

        [JsonPropertyName("fee")]
        public string Fee { get; set; }

        [JsonPropertyName("created_timestamp")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("updated_timestamp")]
        public string UpdatedAt { get; set; }

        [JsonPropertyName("operations")]
        public IEnumerable<OperationsDTO> Operations { get; set; }

    }
}
