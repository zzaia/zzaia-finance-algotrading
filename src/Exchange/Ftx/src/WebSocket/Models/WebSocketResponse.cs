using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MarketIntelligency.Exchange.Ftx.WebSocket.Models
{
    public class WebSocketResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("channel")]
        public string Channel { get; set; }
        [JsonPropertyName("market")]
        public string Market { get; set; }
        //[JsonPropertyName("data")]
        //public Dictionary<string, string> Data { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("msg")]
        public string Message { get; set; }

        public static class Types
        {
            public const string Subscribed = "subscribed";
            public const string Unsubscribed = "unsubscribed";
            public const string Update = "update";
            public const string Partial = "partial";
            public const string Info = "info";
            public const string Error = "error";
            public const string Pong = "pong";
        }
    }
}
