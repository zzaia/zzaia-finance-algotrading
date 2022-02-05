using System.Text.Json.Serialization;

namespace MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class OrderInformationDTO
    {
        [JsonPropertyName("order")]
        public OrderDTO Order { get; set; }

    }
}
