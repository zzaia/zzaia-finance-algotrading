using System.Text.Json.Serialization;

namespace Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Models.DTO
{
    public class OperationsDTO
    {
        [JsonPropertyName("operation_id")]
        public int Id { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("fee_rate")]
        public string FeeRate { get; set; }

        [JsonPropertyName("executed_timestamp")]
        public string ExecutedAt { get; set; }

    }
}