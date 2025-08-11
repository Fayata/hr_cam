using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Net.WebSockets;

namespace hr_cam
{
    public class WebSocketHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                // Perbaikan: Gunakan lambda expression agar sesuai dengan tipe delegate
                //context.AcceptWebSocketRequest(async (webSocketContext) => await HandleWebSocketCommunication(webSocketContext));
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("This is not a WebSocket request.");
            }
        }

        //private async Task HandleWebSocketCommunication(AspNetWebSocketContext webSocketContext)
        //{
        //    WebSocket webSocket = webSocketContext.WebSocket;
        //    byte[] buffer = new byte[1024];

        //    while (webSocket.State == WebSocketState.Open)
        //    {
        //        try
        //        {
        //            // Terima pesan dari client (Go application)
        //            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //            if (result.MessageType == WebSocketMessageType.Close)
        //            {
        //                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        //            }
        //            else
        //            {
        //                // Konversi pesan ke string
        //                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);

        //                // Kirim kembali pesan yang diterima dari Go ke browser
        //                string responseMessage = "Received from Go: " + receivedMessage;
        //                byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
        //                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error handling WebSocket message: " + ex.Message);
        //        }
        //    }
        //}

        public bool IsReusable
        {
            get { return false; }
        }
    }
}