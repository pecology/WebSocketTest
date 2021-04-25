using System;
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

            Console.Write("Type send text: ");
            var sendText = Console.ReadLine();

            var sendBuffer = Encoding.UTF8.GetBytes(sendText);
            await socket.SendAsync(sendBuffer.AsMemory(), WebSocketMessageType.Text, true, CancellationToken.None);

            var recvBuffer = new byte[8];
            await socket.ReceiveAsync(recvBuffer.AsMemory(), CancellationToken.None);
            var recvText = Encoding.UTF8.GetString(recvBuffer);
            Console.WriteLine("recv > " + recvText); 

            //while(true)
            //{
            //    var segment = new ArraySegment<byte>(buffer);
            //    var result = await socket.ReceiveAsync(segment, CancellationToken.None);

            //    if (result.MessageType == WebSocketMessageType.Close)
            //    {
            //        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "OK", CancellationToken.None);
            //        return;
            //    }

            //    if (result.MessageType == WebSocketMessageType.Binary)
            //    {
            //        await socket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "binary not suport", CancellationToken.None);
            //        return;
            //    }

            //    var count = result.Count;
            //    while (!result.EndOfMessage)
            //    {
            //        if (count >= buffer.Length)
            //        {
            //            await socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "too big", CancellationToken.None);

            //            return;
            //        }

            //        segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
            //        result = await socket.ReceiveAsync(segment, CancellationToken.None);
            //        count += result.Count;
            //    }

            //    var message = Encoding.UTF8.GetString(buffer, 0, count);
            //    Console.WriteLine("> " + message);
            //}
        }
    }
}
