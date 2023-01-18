using System.Text.Json.Serialization;

namespace Zzaia.Finance.Exchange.Ftx.WebSockets.Models
{
    public class WebSocketRequest
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; }
        [JsonPropertyName("market")]
        public string Market { get; set; }
        [JsonPropertyName("op")]
        public string Operation { get; set; }

        public WebSocketRequest(string channel, string market, string operation)
        {
            this.Channel = channel;
            this.Market = market;
            this.Operation = operation;
        }
        public static class OperationTypes
        {
            public const string Subscribe = "subscribe";
            public const string Unsubscribe = "unsubscribe";
            public const string Ping = "ping";
        }

        public static class ChannelTypes
        {
            public const string Trades = "trades";
            public const string Orderbook = "orderbook";
            public const string Ticker = "ticker";
        }
    }
}