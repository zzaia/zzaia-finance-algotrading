using MarketIntelligency.Web.Socket;
using MarketIntelligency.WebSocket.Models;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.WebSocket
{
    public class WebSocketClient : IDisposable, IWebSocketClient
    {

        public readonly Uri Address;

        private ClientWebSocket _internalWs { get; set; }
        public WebSocketState State { get; private set; }
        public long? LastMessageReceivedTime { get; set; } // In milliseconds.

        public WebSocketClient(string address)
        {
            _internalWs = new ClientWebSocket();
            Address = new Uri(address);
            State = WebSocketState.None;
        }

        public async Task ConnectAsync(CancellationToken cToken)
        {
            if (State != WebSocketState.None)
                throw new InvalidOperationException("Websocket has already been used (state is not None). Please use reconnect instead.");

            State = WebSocketState.Connecting;
            await _internalWs.ConnectAsync(Address, cToken);
            State = WebSocketState.Open;
        }

        public async Task ReconnectAsync(CancellationToken cToken)
        {
            if (_internalWs.State == WebSocketState.Open)
                await CloseGracefullyAsync(cToken);

            _internalWs.Dispose();
            _internalWs = new ClientWebSocket();

            State = WebSocketState.None;
            await ConnectAsync(cToken);
        }


        public async Task<WebSocketClientResponse> ReceiveAsync(CancellationToken cToken)
        {
            LastMessageReceivedTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            byte[] finalResultBytes = new byte[0];
            byte[] buffer = new byte[2048];

            WebSocketReceiveResult receivedData = null;
            bool receivedEndOfMessage = false;
            while (!receivedEndOfMessage)
            {
                if (_internalWs.State == WebSocketState.Open)
                {
                    receivedData = await _internalWs.ReceiveAsync(new ArraySegment<byte>(buffer), cToken);
                    if (receivedData.MessageType == WebSocketMessageType.Text || receivedData.MessageType == WebSocketMessageType.Binary)
                    {
                        finalResultBytes = AddResultBytes(finalResultBytes, buffer, receivedData);
                        if (receivedData.EndOfMessage)
                            receivedEndOfMessage = true;
                    }
                    else if (receivedData.MessageType == WebSocketMessageType.Close)
                    {
                        // Close bubble up.
                        State = WebSocketState.Closed;
                    }
                }
            }

            return new WebSocketClientResponse(receivedData, finalResultBytes);
        }

        public Task SendTextAsync(string message, CancellationToken cToken)
        {
            byte[] msgBytes = Encoding.UTF8.GetBytes(message);
            return _internalWs.SendAsync(new ArraySegment<byte>(msgBytes), WebSocketMessageType.Text, true, cToken);
        }

        public async Task CloseGracefullyAsync(CancellationToken cToken)
        {
            State = WebSocketState.CloseSent;
            await _internalWs.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed gracefully", cToken);
            State = WebSocketState.Closed;
            return;
        }

        private byte[] AddResultBytes(byte[] first, byte[] second, WebSocketReceiveResult result)
        {
            byte[] ret = new byte[first.Length + result.Count];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, result.Count);
            return ret;
        }

        public void Dispose()
        {
            _internalWs.Abort(); // isso ta errado
            _internalWs.Dispose();
        }
    }
}
