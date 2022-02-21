using System.Net.WebSockets;
using System.Text;

namespace MarketIntelligency.Exchange.Ftx.WebSockets.Models
{
    public class WebSocketClientResponse
    {
        public int Lenght { get; set; }
        public WebSocketMessageType? MessageType { get; set; }
        public byte[] Message { get; set; }

        public WebSocketClientResponse(WebSocketReceiveResult receiveResult, byte[] receiveBytes)
        {
            MessageType = receiveResult.MessageType;
            if (receiveResult.MessageType != WebSocketMessageType.Close)
            {
                Message = receiveBytes;
                Lenght = receiveBytes.Length;
            }
            else
            {
                string message = $"{(receiveResult == null ? "UKNCODE" : receiveResult.CloseStatus.ToString())} {receiveResult.CloseStatusDescription}";
                Message = Encoding.UTF8.GetBytes(message);
                Lenght = Message.Length;
            }
        }
    }
}
