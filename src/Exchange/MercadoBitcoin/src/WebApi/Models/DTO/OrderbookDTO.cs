using System.Text.Json.Serialization;

namespace Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class OrderBookDTO
    {
        [JsonPropertyName("asks")]
        public decimal[][] Asks { get; set; }

        [JsonPropertyName("bids")]
        public decimal[][] Bids { get; set; }

        [JsonPropertyName("ticker")]
        public string MainTicker { get; set; }

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }

    }
}
