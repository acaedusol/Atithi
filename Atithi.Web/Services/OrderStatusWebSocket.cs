using System.Net.WebSockets;
using System.Text;

namespace Atithi.Web.Services
{
    public class OrderStatusWebSocket
    {
        private static List<WebSocket> _webSockets = new List<WebSocket>(); // List of connected WebSockets

        public static async Task HandleWebSocket(WebSocket webSocket)
        {
            // Add the new WebSocket to the list of connected clients
            _webSockets.Add(webSocket);

            var buffer = new byte[1024 * 4]; // Buffer for incoming data

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                        _webSockets.Remove(webSocket); // Remove from the list when closed
                    }
                    else if (result.MessageType == WebSocketMessageType.Text)
                    {
                        // Handle incoming messages from the client (if needed)
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}");
                _webSockets.Remove(webSocket); // Ensure the WebSocket is removed in case of error
            }
        }

        public static async Task SendOrderStatusUpdate(Guid orderId, bool status, int roomId)
        {
            // Convert the status update to a JSON string
            var message = Encoding.UTF8.GetBytes($"{{\"OrderId\":\"{orderId}\", \"Status\":\"{status}\",\"RoomId\":\"{roomId}\"}}");

            var tasks = new List<Task>();

            // Send the update to all connected clients
            foreach (var socket in _webSockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    tasks.Add(socket.SendAsync(
                        new ArraySegment<byte>(message),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None));
                }
            }

            await Task.WhenAll(tasks); // Wait for all sends to complete
        }
    }

}
