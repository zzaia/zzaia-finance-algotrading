using MarketIntelligency.Exchange.Ftx.WebSockets.Models;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.Ftx.WebSockets
{
    public class WebSocketClient : IDisposable, IWebSocketClient
    {
        public readonly Uri _address;
        private ClientWebSocket _internalWs { get; set; }
        public long? _lastMessageReceivedTime { get; set; } // In milliseconds.
        public WebSocketState _state { get; private set; }

        public WebSocketClient(string address)
        {
            _internalWs = new ClientWebSocket();
            _address = new Uri(address);
            _state = WebSocketState.None;
        }

        public async Task ConnectAsync(CancellationToken cToken)
        {
            if (_state != WebSocketState.None)
                throw new InvalidOperationException("Websocket has already been used (state is not None). Please use reconnect instead.");

            _state = WebSocketState.Connecting;
            await _internalWs.ConnectAsync(_address, cToken);
            _state = WebSocketState.Open;
        }

        public async Task ReconnectAsync(CancellationToken cToken)
        {
            if (_internalWs.State == WebSocketState.Open)
                await CloseGracefullyAsync(cToken);

            _internalWs.Dispose();
            _internalWs = new ClientWebSocket();

            _state = WebSocketState.None;
            await ConnectAsync(cToken);
        }


        public async Task<WebSocketClientResponse> ReceiveAsync(CancellationToken cToken)
        {
            _lastMessageReceivedTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            byte[] finalResultBytes = Array.Empty<byte>();
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
                        _state = WebSocketState.Closed;
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
            _state = WebSocketState.CloseSent;
            await _internalWs.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed gracefully", cToken);
            _state = WebSocketState.Closed;
            return;
        }

        private static byte[] AddResultBytes(byte[] first, byte[] second, WebSocketReceiveResult result)
        {
            byte[] ret = new byte[first.Length + result.Count];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, result.Count);
            return ret;
        }

        public void Dispose()
        {
            _internalWs.Abort();
            _internalWs.Dispose();
        }
    }
}
