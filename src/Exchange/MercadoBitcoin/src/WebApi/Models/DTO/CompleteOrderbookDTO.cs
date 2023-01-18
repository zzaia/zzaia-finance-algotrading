using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class CompleteOrderbookDTO
    {
        [JsonPropertyName("bids")]
        public IEnumerable<OrderSummaryDTO> Bids { get; set; }

        [JsonPropertyName("asks")]
        public IEnumerable<OrderSummaryDTO> Asks { get; set; }

        [JsonPropertyName("latest_order_id")]
        public int LatestOrderId { get; set; }

    }
}
