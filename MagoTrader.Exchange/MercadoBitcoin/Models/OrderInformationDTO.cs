using System.Text.Json.Serialization;

namespace MarketMaker.Exchange.MercadoBitcoin.Models
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
