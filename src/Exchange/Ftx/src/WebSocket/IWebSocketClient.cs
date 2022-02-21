using MarketIntelligency.Exchange.Ftx.WebSockets.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.Ftx.WebSockets
{
    public interface IWebSocketClient
    {
        Task CloseGracefullyAsync(CancellationToken cToken);
        Task ConnectAsync(CancellationToken cToken);
        Task<WebSocketClientResponse> ReceiveAsync(CancellationToken cToken);
        Task ReconnectAsync(CancellationToken cToken);
        Task SendTextAsync(string message, CancellationToken cToken);
    }
}
