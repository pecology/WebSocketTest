using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();

        }


        private async static Task MainAsync()
        {
            var socket = new ClientWebSocket();
            var uri = new Uri("ws://localhost:80/");
                await socket.ConnectAsync(uri, CancellationToken.None);

            for (int i = 0; i < 100; i++)
            {
                var data = new byte[1];
                var res = await socket.ReceiveAsync(data, CancellationToken.None);
                Console.WriteLine(data[0]);
            }
        }
    }
}
