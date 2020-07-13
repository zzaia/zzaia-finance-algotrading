using System.Text.Json.Serialization;

namespace MagoTrader.Exchange.MercadoBitcoin.Models
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class OrderbookInformationDTO
    {
        [JsonPropertyName("orderbook")]
        public CompleteOrderbookDTO Orderbook { get; set; }
    }
}
