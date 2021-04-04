using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class SystemMessagesDTO
    {
        [JsonPropertyName("messages")]
        public IEnumerable<SystemMessageDTO> Messages { get; set; }
    }
}
