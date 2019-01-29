using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MixinSdk.Bean;

namespace MixinSdk
{
    public class MixinWebSocketApi : MixinApi
    {
        /// <summary>
        /// Webs the socket connect.
        /// </summary>
        /// <returns>The socket connect.</returns>
        public async Task<ClientWebSocket> WebSocketConnect()
        {
            ClientWebSocket clientWebSocket = new ClientWebSocket();
            clientWebSocket.Options.AddSubProtocol("Mixin-Blaze-1");

            string token = MixinUtils.GenJwtAuthCode("GET", "/", "", userConfig.ClientId, userConfig.SessionId, priKey);

            clientWebSocket.Options.SetRequestHeader("Authorization", "Bearer " + token);
            using (var cts = new CancellationTokenSource(10000))
            {
                Task taskConnect = clientWebSocket.ConnectAsync(new Uri(Config.MIXIN_WEBSOCKET_URL), cts.Token);

                await taskConnect;
            }

            if (clientWebSocket.State == WebSocketState.Open)
            {
                System.Console.WriteLine("Connetced !!");
            }
            else
            {
                System.Console.WriteLine("Connetced fails" + clientWebSocket.State);
            }

            return clientWebSocket;
        }

        public async Task SendMessage(ClientWebSocket clientWebSocket, WebSocketMessage msg)
        {
            string szMsg = msg.ToString();
            var bMsg = Encoding.UTF8.GetBytes(szMsg);

            var compressedMsg = GZipHelper.Compress(bMsg);

            using (var cts = new CancellationTokenSource(10000))
            {
                var task = clientWebSocket.SendAsync(new ArraySegment<byte>(compressedMsg), WebSocketMessageType.Binary, true, cts.Token);
                await task;
            }
        }

        public async Task StartRecive(ClientWebSocket clientWebSocket)
        {
            var buffer = new byte[4096 * 2];
            int startIndex = 0;

            if(clientWebSocket.State != WebSocketState.Open)
            {
                return;
            }

            WebSocketMessage webSocketMessage = new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "LIST_PENDING_MESSAGES"
            };

            await SendMessage(clientWebSocket, webSocketMessage);

            while (clientWebSocket.State == WebSocketState.Open)
            {
                var rcvBuffer = new ArraySegment<byte>(buffer, startIndex, buffer.Length - startIndex);
                var response = await clientWebSocket.ReceiveAsync(rcvBuffer, CancellationToken.None);

                if (response.MessageType == WebSocketMessageType.Close)
                {
                    await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close response received",
                            CancellationToken.None);
                    return;
                }
                if (response.MessageType == WebSocketMessageType.Binary)
                {
                    if (response.EndOfMessage)
                    {
                        var js = GZipHelper.Decompress(buffer);
                        var sjs = Encoding.ASCII.GetString(js);
                        System.Console.WriteLine(sjs);
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex += response.Count;
                    }
                }
            }
        }
    }

    public interface IMessageProcesser
    {
        string OnRecive();
    }
}
