using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {

            Console.WriteLine("Hello World!");
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:80/");
            listener.Start();

            var context = await listener.GetContextAsync();
            var webSocketContext = await context.AcceptWebSocketAsync(null);

            var webSocket = webSocketContext.WebSocket;

            for (int i = 0; i < 100; i++)
            {
                var sendData = new byte[1];
                sendData[0] = (byte)i;
                await webSocket.SendAsync(sendData, System.Net.WebSockets.WebSocketMessageType.Binary, true, CancellationToken.None);
            }

        }
    }
}
