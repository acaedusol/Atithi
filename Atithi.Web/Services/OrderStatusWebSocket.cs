using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace Atithi.Web.Services
{
    public class OrderStatusWebSocket
    {
        private static ConcurrentBag<WebSocket> _webSockets = new ConcurrentBag<WebSocket>();

        public static async Task HandleWebSocket(WebSocket webSocket)
        {
            _webSockets.Add(webSocket);

            var buffer = new byte[1024 * 4];

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                        _webSockets.TryTake(out _); // Remove from the list when closed
                    }
                    else if (result.MessageType == WebSocketMessageType.Text)
                    {
                        // Handle incoming messages from the client (if needed)
                        Console.WriteLine($"Received message: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}");
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Error", CancellationToken.None);
                }
                _webSockets.TryTake(out _); // Ensure WebSocket is removed in case of error
            }
        }

        public static async Task SendOrderStatusUpdate(Guid orderId, bool status, int roomId)
        {
            var message = Encoding.UTF8.GetBytes($"{{\"OrderId\":\"{orderId}\", \"Status\":\"{status}\", \"RoomId\":\"{roomId}\"}}");

            var tasks = new List<Task>();

            foreach (var webSocket in _webSockets)
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    tasks.Add(webSocket.SendAsync(
                        new ArraySegment<byte>(message),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None));
                }
            }

            await Task.WhenAll(tasks); // Ensure all sends are completed
        }
    }
}
