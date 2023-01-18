using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Zzaia.Finance.Exchange.Ftx.WebSocket.Models
{
    public class OrderbookResponse
    {
        [JsonPropertyName("data")]
        public OrderbookData Data { get; set; }
    }
    public class OrderbookData
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }
        [JsonPropertyName("checksum")]
        public Int64 Checksum { get; set; }
        [JsonPropertyName("time")]
        public double Time { get; set; }
        [JsonPropertyName("bids")]
        public IEnumerable<decimal[]> Bids { get; set; }
        [JsonPropertyName("asks")]
        public IEnumerable<decimal[]> Asks { get; set; }
    }
}
