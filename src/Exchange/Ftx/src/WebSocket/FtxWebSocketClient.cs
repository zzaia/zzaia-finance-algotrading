using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Exchange.Ftx.WebSockets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.Ftx.WebSockets
{
    public class FtxWebSocketClient : WebSocketClient, IFtxWebSocketClient
    {
        public WebSocketState GetState() => _state;
        public bool IsOpen() => _state == WebSocketState.Open;
        public FtxWebSocketClient(Uri url) : base(url.AbsoluteUri) { }

        public async Task SubscribeOrderbook(string pair, CancellationToken stoppingToken)
        {
            var subscribeRequest = new WebSocketRequest("orderbook", pair, "subscribe");
            var subscribeRequestMessage = JsonSerializer.Serialize(subscribeRequest);
            await this.SendTextAsync(subscribeRequestMessage, stoppingToken);
        }

        public async Task UnsubscribeOrderbook(string pair, CancellationToken stoppingToken)
        {
            var unsubscribeRequest = new WebSocketRequest("orderbook", pair, "unsubscribe");
            var unsubscribeRequestMessage = JsonSerializer.Serialize(unsubscribeRequest);
            await this.SendTextAsync(unsubscribeRequestMessage, stoppingToken);
        }

        public async Task ManageSubscribeTicker(List<string> oldList, List<string> newList, CancellationToken cancellationToken)
        {
            var oldExceptNew = oldList.Except(newList);
            var newExceptOld = newList.Except(oldList);
            foreach (var item in oldExceptNew) await this.UnsubscribeOrderbook(item, cancellationToken);
            foreach (var item in newExceptOld) await this.SubscribeOrderbook(item, cancellationToken);
        }

        public Task AuthenticateSubscription(ClientCredential clientCredentials)
        {
            throw new NotImplementedException();
        }
    }
}
