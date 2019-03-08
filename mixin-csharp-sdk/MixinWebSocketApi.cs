using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MixinSdk.Bean;
using Newtonsoft.Json;

namespace MixinSdk
{
    public partial class MixinApi
    {
        public ClientWebSocket clientWebSocket { get; private set; }

        public delegate void OnOpened(object sender, EventArgs args);
        public delegate void OnRecivedMessage(object sender, EventArgs args, string message);
        public delegate void OnClosed(object sender, EventArgs args);

        public OnRecivedMessage onRecivedMessage;
        public OnOpened onOpened;
        public OnClosed onClosed;


        /// <summary>
        /// Webs the socket connect.
        /// </summary>
        /// <returns>The socket connect.</returns>
        public async Task WebSocketConnect(OnRecivedMessage onRecivedMessage = null, OnOpened onOpened = null, 
                                        OnClosed onClosed = null)
        {
            this.onRecivedMessage = onRecivedMessage;
            this.onOpened = onOpened;
            this.onClosed = onClosed;

            if (null != clientWebSocket)
            {
                await clientWebSocket.CloseAsync(WebSocketCloseStatus.Empty, "close", CancellationToken.None);
            }

            clientWebSocket = new ClientWebSocket();
            clientWebSocket.Options.AddSubProtocol("Mixin-Blaze-1");

            string token = GenGetJwtToken("/", "");

            clientWebSocket.Options.SetRequestHeader("Authorization", "Bearer " + token);
            using (var cts = new CancellationTokenSource(ReadTimeout))
            {
                Task taskConnect = clientWebSocket.ConnectAsync(new Uri(MIXIN_WEBSOCKET_URL), cts.Token);

                await taskConnect;
            }

            if (clientWebSocket.State == WebSocketState.Open)
            {
                onOpened?.Invoke(this, null);
            }
            else
            {
                Console.WriteLine("Connetced fails: " + clientWebSocket.State);
            }
        }

        public async Task SendMessage(WebSocketMessage msg)
        {
            string szMsg = msg.ToString();

            var bMsg = Encoding.UTF8.GetBytes(szMsg);

            var compressedMsg = GZipHelper.Compress(bMsg);

            using (var cts = new CancellationTokenSource(ReadTimeout))
            {
                await clientWebSocket.SendAsync(new ArraySegment<byte>(compressedMsg), WebSocketMessageType.Binary, true, cts.Token);
            }
        }


        public async Task SendTextMessage(string conversationId, string text)
        {
            WebSocketMessage msg = new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "CREATE_MESSAGE",
                @params = new Params {
                    conversation_id = conversationId,
                    category = "PLAIN_TEXT",
                    status = "SENT",
                    message_id = Guid.NewGuid().ToString(),
                    data = Convert.ToBase64String(Encoding.UTF8.GetBytes(text))
                }
            };

            await SendMessage(msg);
        }

        public async Task SendStickerMessage(string conversationId, string name, string albumId)
        {
            WebSocketMessage msg = new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "CREATE_MESSAGE",
                @params = new Params
                {
                    conversation_id = conversationId,
                    category = "PLAIN_STICKER",
                    status = "SENT",
                    message_id = Guid.NewGuid().ToString(),
                    data = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"name\": \"" + name + "\", \"album_id\": \"" + albumId + "\"}"))
                }
            };

            await SendMessage(msg);
        }

        public async Task SendContactMessage(string conversationId, string userId)
        {
            WebSocketMessage msg = new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "CREATE_MESSAGE",
                @params = new Params
                {
                    conversation_id = conversationId,
                    category = "PLAIN_CONTACT",
                    status = "SENT",
                    message_id = Guid.NewGuid().ToString(),
                    data = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"user_id\":\""+ userId + "\"}"))
                }
            };

            await SendMessage(msg);
        }

        public async Task SendAppButtonGroupMessage(string conversationId, List<AppButton> appButtons )
        {
            WebSocketMessage msg = new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "CREATE_MESSAGE",
                @params = new Params
                {
                    conversation_id = conversationId,
                    category = "APP_BUTTON_GROUP",
                    status = "SENT",
                    message_id = Guid.NewGuid().ToString(),
                    data = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(appButtons)))
                }
            };

            await SendMessage(msg);
        }

        public async Task SendAppCardMessage(string conversationId, AppCard appCard)
        {
            WebSocketMessage msg = new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "CREATE_MESSAGE",
                @params = new Params
                {
                    conversation_id = conversationId,
                    category = "APP_CARD",
                    status = "SENT",
                    message_id = Guid.NewGuid().ToString(),
                    data = Convert.ToBase64String(Encoding.UTF8.GetBytes(appCard.ToString()))
                }
            };

            await SendMessage(msg);
        }


        public async Task SendImageMessage(ClientWebSocket clientWebSocket, Attachment attachment, string conversationId, string imageUri,
                                            string mimeType, int width, int height, long size, string thumbnail)
        {

            WebSocketMessage msg = new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "CREATE_MESSAGE",
                @params = new Params
                {
                    conversation_id = conversationId,
                    category = "PLAIN_IMAGE",
                    status = "SENT",
                    message_id = Guid.NewGuid().ToString(),

                    data = Convert.ToBase64String(Encoding.UTF8.GetBytes(new MsgAttachment
                    {
                        attachment_id = attachment.attachment_id,
                        mime_type = mimeType,
                        width = width,
                        height = height,
                        size = size,
                        thumbnail = thumbnail
                    }.ToString()))
                }
            };

            await SendMessage(msg);
        }

        public async Task SendDataMessage(ClientWebSocket clientWebSocket, Attachment attachment, string conversationId, string imageUri,
                                            string mimeType, long size, string name)
        {

            WebSocketMessage msg = new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "CREATE_MESSAGE",
                @params = new Params
                {
                    conversation_id = conversationId,
                    category = "PLAIN_DATA",
                    status = "SENT",
                    message_id = Guid.NewGuid().ToString(),

                    data = Convert.ToBase64String(Encoding.UTF8.GetBytes(new MsgAttachment
                    {
                        attachment_id = attachment.attachment_id,
                        mime_type = mimeType,
                        size = size,
                        name = name
                    }.ToString()))
                }
            };

            await SendMessage(msg);
        }

        public async Task SendVideoMessage(ClientWebSocket clientWebSocket, Attachment attachment, string conversationId, string imageUri,
                                            string mimeType, int width, int height, long duration, long size, string thumbnail)
        {

            WebSocketMessage msg = new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "CREATE_MESSAGE",
                @params = new Params
                {
                    conversation_id = conversationId,
                    category = "PLAIN_VIDEO",
                    status = "SENT",
                    message_id = Guid.NewGuid().ToString(),

                    data = Convert.ToBase64String(Encoding.UTF8.GetBytes(new MsgAttachment
                    {
                        attachment_id = attachment.attachment_id,
                        mime_type = mimeType,
                        width = width,
                        height = height,
                        duration = duration,
                        size = size,
                        thumbnail = thumbnail
                    }.ToString()))
                }
            };

            await SendMessage(msg);
        }


        public async Task SendListPendingMessage()
        {
            await SendMessage(new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "LIST_PENDING_MESSAGES"
            });
        }


        public async Task SendMessageResponse(string msgid)
        {
            await SendMessage(new WebSocketMessage
            {
                id = Guid.NewGuid().ToString(),
                action = "ACKNOWLEDGE_MESSAGE_RECEIPT",
                @params = new Params
                {
                    message_id = msgid,
                    status = "READ",
                }
            });
        }
        public async Task StartRecive()
        {
            var buffer = new byte[4096 * 2];
            int startIndex = 0;

            if(clientWebSocket.State != WebSocketState.Open)
            {
                return;
            }

            await SendListPendingMessage();

            while (clientWebSocket.State == WebSocketState.Open)
            {
                var rcvBuffer = new ArraySegment<byte>(buffer, startIndex, buffer.Length - startIndex);
                var response = await clientWebSocket.ReceiveAsync(rcvBuffer, CancellationToken.None);

                if (response.MessageType == WebSocketMessageType.Close)
                {
                    await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close response received",
                            CancellationToken.None);
                    onClosed?.Invoke(this, null);
                    return;
                }
                if (response.MessageType == WebSocketMessageType.Binary)
                {
                    if (response.EndOfMessage)
                    {
                        var js = GZipHelper.Decompress(buffer);
                        var sjs = Encoding.ASCII.GetString(js);
                        Console.WriteLine("收到的报文为：" + sjs);
                        onRecivedMessage?.Invoke(this, null, sjs);

                        startIndex = 0;
                    }
                    else
                    {
                        startIndex += response.Count;
                    }
                }
            }
        }

        public async Task CloseAsync()
        {
            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "CLose Normally.", CancellationToken.None);
            onClosed?.Invoke(this, null);
        }
    }

}
