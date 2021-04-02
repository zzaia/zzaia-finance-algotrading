using System.Globalization;
using System.Text.Json.Serialization;

namespace MarketIntelligency.Exchange.MercadoBitcoin.Models
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class TAPResponse<T>
    {
        [JsonPropertyName("error_message")]
        public string Message { get; set; }

        [JsonPropertyName("server_unix_timestamp")]
        public string TimeStamp { get; set; }

        [JsonPropertyName("status_code")]
        public int Code { get; set; }

        [JsonPropertyName("response_data")]
        public T Data { get; set; }
        public bool Success => TapStatusCodeEnum.Success == System.Enum.Parse<TapStatusCodeEnum>(Code.ToString(CultureInfo.InvariantCulture));
    }
}