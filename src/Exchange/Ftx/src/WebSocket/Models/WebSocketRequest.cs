namespace MarketIntelligency.Exchange.Ftx.WebSockets.Models
{
    public class WebSocketRequest
    {
        public string channel { get; set; }
        public string market { get; set; }
        public string op { get; set; }

        public WebSocketRequest(string channel, string market, string op)
        {
            this.channel = channel;
            this.market = market;
            this.op = op;
        }
    }
}