using System.Text.Json.Serialization;

namespace Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class WithdrawalDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("coin")]
        public string CoinTicker { get; set; }

        [JsonPropertyName("fee")]
        public string Fee { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("created_timestamp")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("updated_timestamp")]
        public string UpdatedAt { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("net_quantity")]
        public string NetQuantity { get; set; }
        
        [JsonPropertyName("account")]
        public string BankAccount { get; set; }

        [JsonPropertyName("address")]
        public string WalletAddress { get; set; }

        [JsonPropertyName("tx")]
        public string Transaction { get; set; }

        [JsonPropertyName("destination_tag")]
        public int DestinationTag { get; set; }
    }
}
