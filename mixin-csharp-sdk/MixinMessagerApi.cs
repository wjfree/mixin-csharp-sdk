using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MixinSdk.Bean;
using Newtonsoft.Json;

namespace MixinSdk
{
    public partial class MixinApi
    {

        /// <summary>
        /// Gets the OAuth Url.
        /// </summary>
        /// <returns>The OAuth Url.</returns>
        /// <param name="scope">Scope.</param>
        public string GetOAuthString(string scope)
        {
            return "https://mixin.one/oauth/authorize?client_id=" + userConfig.ClientId + "&scope=" + scope + "&response_type=code";
        }

        /// <summary>
        /// Gets the client auth token.
        /// </summary>
        /// <returns>The client auth token.</returns>
        /// <param name="code">Code.</param>
        public string GetClientAuthToken(string code)
        {
            return GetClientAuthTokenAsync(code).Result;
        }

        /// <summary>
        /// Gets the client auth token async.
        /// </summary>
        /// <returns>The client auth token async.</returns>
        /// <param name="code">Code.</param>
        public async Task<string> GetClientAuthTokenAsync(string code)
        {
            const string req = "/oauth/token";

            var p = new AuthTokenReq
            {
                code = code,
                client_id = userConfig.ClientId,
                client_secret = userConfig.ClientSecret
            };

            var rz = await doPostRequestAsync(req, p, false);
            var rsp = JsonConvert.DeserializeObject<AuthTokenRsp>(rz);

            return rsp.access_token;
        }

        /// <summary>
        /// Reads the profile.
        /// </summary>
        /// <returns>The profile.</returns>
        public UserInfo ReadProfile(string token = null)
        {
            return ReadProfileAsync(token).Result;
        }

        /// <summary>
        /// Reads the profile async.
        /// </summary>
        /// <returns>The profile async.</returns>
        /// <param name="token">Token.</param>
        public async Task<UserInfo> ReadProfileAsync(string token = null)
        {
            const string req = "/me";
            var rz = await doGetRequestAsync(req, true, token);
            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Updates the perference.
        /// </summary>
        /// <returns>User info.</returns>
        /// <param name="receiveMessageSource">Receive message source.</param>
        /// <param name="acceptConversationSource">Accept conversation source.</param>
        public UserInfo UpdatePerference(string receiveMessageSource, string acceptConversationSource)
        {
            return UpdatePerferenceAsync(receiveMessageSource, acceptConversationSource).Result;
        }

        /// <summary>
        /// Updates the perference async.
        /// </summary>
        /// <returns>The perference async.</returns>
        /// <param name="receiveMessageSource">Receive message source.</param>
        /// <param name="acceptConversationSource">Accept conversation source.</param>
        public async Task<UserInfo> UpdatePerferenceAsync(string receiveMessageSource, string acceptConversationSource)
        {
            const string req = "/me/preferences";

            UserPerference p = new UserPerference
            {
                receive_message_source = receiveMessageSource,
                accept_conversation_source = acceptConversationSource
            };

            var rz = await doPostRequestAsync(req, p, true);

            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <returns>User info</returns>
        /// <param name="fullName">Full name.</param>
        /// <param name="avatarBase64">Avatar base64.</param>
        public UserInfo UpdateProfile(string fullName, string avatarBase64)
        {
            return UpdateProfileAsync(fullName, avatarBase64).Result;
        }

        /// <summary>
        /// Updates the profile async.
        /// </summary>
        /// <returns>The profile async.</returns>
        /// <param name="fullName">Full name.</param>
        /// <param name="avatarBase64">Avatar base64.</param>
        public async Task<UserInfo> UpdateProfileAsync(string fullName, string avatarBase64)
        {
            const string req = "/me";

            UserProfile p = new UserProfile
            {
                full_name = fullName,
                avatar_base64 = avatarBase64
            };

            var rz = await doPostRequestAsync(req, p, true);

            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Reads the users.
        /// </summary>
        /// <returns>The users.</returns>
        /// <param name="userUuids">User uuids.</param>
        public List<UserInfo> ReadUsers(List<String> userUuids)
        {
            return ReadUsersAsync(userUuids).Result;
        }

        /// <summary>
        /// Reads the users async.
        /// </summary>
        /// <returns>The users async.</returns>
        /// <param name="userUuids">User uuids.</param>
        public async Task<List<UserInfo>> ReadUsersAsync(List<String> userUuids)
        {
            const string req = "/users/fetch";

            var rz = await doPostRequestAsync(req, userUuids, true);

            return JsonConvert.DeserializeObject<List<UserInfo>>(rz);
        }

        /// <summary>
        /// Reads the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="userUuid">User UUID.</param>
        public UserInfo ReadUser(string userUuid)
        {
            return ReadUserAsync(userUuid).Result;
        }

        /// <summary>
        /// Reads the user async.
        /// </summary>
        /// <returns>The user async.</returns>
        /// <param name="userUuid">User UUID.</param>
        public async Task<UserInfo> ReadUserAsync(string userUuid)
        {
            string req = "/users/" + userUuid;

            var rz = await doGetRequestAsync(req, true);
            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Searchs the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="mixinIdOrPhoneNo">Mixin identifier or phone no.</param>
        public UserInfo SearchUser(string mixinIdOrPhoneNo)
        {
            return SearchUserAsync(mixinIdOrPhoneNo).Result;
        }

        /// <summary>
        /// Searchs the user async.
        /// </summary>
        /// <returns>The user async.</returns>
        /// <param name="mixinIdOrPhoneNo">Mixin identifier or phone no.</param>
        public async Task<UserInfo> SearchUserAsync(string mixinIdOrPhoneNo)
        {
            string req = "/search/" + mixinIdOrPhoneNo;

            var rz = await doGetRequestAsync(req, true);
            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Rotates the user qr.
        /// </summary>
        /// <returns>The user qr.</returns>
        public UserInfo RotateUserQR()
        {
            return RotateUserQRAsync().Result;
        }

        /// <summary>
        /// Rotates the user QRA sync.
        /// </summary>
        /// <returns>The user QRA sync.</returns>
        public async Task<UserInfo> RotateUserQRAsync()
        {
            const string req = "/me/code";

            var rz = await doGetRequestAsync(req, true);
            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Friends this instance.
        /// </summary>
        /// <returns>The friends.</returns>
        public List<UserInfo> Friends()
        {
            return FriendsAsync().Result;
        }

        /// <summary>
        /// Friendses the async.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task<List<UserInfo>> FriendsAsync()
        {
            const string req = "/friends";

            var rz = await doGetRequestAsync(req, true);
            return JsonConvert.DeserializeObject<List<UserInfo>>(rz);
        }

        /// <summary>
        /// Creates the attachment.
        /// </summary>
        /// <returns>The attachment.</returns>
        public Attachment CreateAttachment()
        {
            return CreateAttachmentAsync().Result;
        }

        /// <summary>
        /// Creates the attachment async.
        /// </summary>
        /// <returns>The attachment async.</returns>
        public async Task<Attachment> CreateAttachmentAsync()
        {
            const string req = "/attachments";

            var rz = await doPostRequestAsync(req, null, true);
            return JsonConvert.DeserializeObject<Attachment>(rz);
        }

        /// <summary>
        /// Uniques the conversation identifier.
        /// </summary>
        /// <returns>The conversation identifier.</returns>
        /// <param name="userId">User identifier.</param>
        /// <param name="recipientId">Recipient identifier.</param>
        private string UniqueConversationId(string userId, string recipientId)
        {
            System.Console.WriteLine("a = " + userId);
            System.Console.WriteLine("b = "+ recipientId);
            var minId = userId;
            var maxId = recipientId;

            if (string.Compare(userId, recipientId) > 0)
            {
                minId = recipientId;
                maxId = userId;
            }

            var md5 = MD5.Create();
            var sum = md5.ComputeHash(Encoding.ASCII.GetBytes(minId + maxId));
            sum[6] = (byte)((sum[6] & 0x0f) | 0x30);
            sum[8] = (byte)((sum[8] & 0x3f) | 0x80);

            var s = BitConverter.ToString(sum);
            s = s.Replace("-", "");

            s = s.Substring(0, 8) + "-" + s.Substring(8);
            s = s.Substring(0, 13) + "-" + s.Substring(13);
            s = s.Substring(0, 18) + "-" + s.Substring(18);
            s = s.Substring(0, 23) + "-" + s.Substring(23);
            s = s.ToLower();

            return s;
        }

        /// <summary>
        /// Creates the conversation.
        /// </summary>
        /// <returns>The conversation.</returns>
        /// <param name="category">Category.</param>
        /// <param name="participants">Participants.</param>
        public Conversation CreateConversation(string category, List<ParticipantAction> participants)
        {
            return CreateConversationAsync(category, participants).Result;
        }

        /// <summary>
        /// Creates the conversation async.
        /// </summary>
        /// <returns>The conversation async.</returns>
        /// <param name="category">Category.</param>
        /// <param name="participants">Participants.</param>
        public async Task<Conversation> CreateConversationAsync(string category, List<ParticipantAction> participants)
        {
            if (!"GROUP".Equals(category) && !"CONTACT".Equals(category))
            {
                return null;
            }

            string conversationId = Guid.NewGuid().ToString();
            if ("CONTACT".Equals(category))
            {
                conversationId = UniqueConversationId(userConfig.ClientId, participants[0].user_id);
            }

            const string req = "/conversations";
            var p = new CreateConversationReq
            {
                category = category,
                conversation_id = conversationId,
                participants = participants
            };

            var rz = await doPostRequestAsync(req, p, true);
            return JsonConvert.DeserializeObject<Conversation>(rz);
        }

        /// <summary>
        /// Reads the conversation.
        /// </summary>
        /// <returns>The conversation.</returns>
        /// <param name="conversationId">Conversation identifier.</param>
        public Conversation ReadConversation(string conversationId)
        {
            return ReadConversationAsync(conversationId).Result;
        }

        public async Task<Conversation> ReadConversationAsync(string conversationId)
        {
            string req = "/conversations/" + conversationId;

            var rz = await doGetRequestAsync(req, true);
            return JsonConvert.DeserializeObject<Conversation>(rz);
        }
    }
}
