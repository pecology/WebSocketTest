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
            var uri = new Uri("ws://localhost:60304/webSocketTest");
            await socket.ConnectAsync(uri, CancellationToken.None);

            var messageQueue = new ConcurrentQueue<byte[]>();

            // キューにあるデータを送信し続けるタスク
            var sendTask = Task.Run(async () =>
            {
                while (true)
                {
                    if (messageQueue.TryDequeue(out var message))
                    {
                        // キューにデータがあれば送信
                        await socket.SendAsync(message, WebSocketMessageType.Binary, true, CancellationToken.None);
                    }
                    else
                    {
                        // キューにデータがなければ0.1秒待つ
                        await Task.Delay(100);
                    }
                }
            });

            // 一秒毎にキューにあるデータの数をログ出力
            var logTask = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    await Console.Out.WriteLineAsync("queue data count: " + messageQueue.Count);
                }
            });

            // 0.1秒に一回1MBのデータをキューに入れる。
            var sendData = new byte[1024 * 1024];
            while(true)
            {
                await Task.Delay(100);
                messageQueue.Enqueue(sendData);
            }
        }
    }
}
