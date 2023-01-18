using Zzaia.Finance.WebSocket.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Zzaia.Finance.WebSocket
{
    public interface IWebSocketClient
    {
        Task CloseGracefullyAsync(CancellationToken cToken);
        Task ConnectAsync(CancellationToken cToken);
        Task<WebSocketClientResponse> ReceiveAsync(CancellationToken cToken);
        Task ReconnectAsync(CancellationToken cToken);
        Task SendTextAsync(string message, CancellationToken cToken);
        void SetBaseAddress(string address);
    }
}
