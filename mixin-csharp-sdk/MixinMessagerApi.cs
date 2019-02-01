using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MixinSdk.Bean;
using Newtonsoft.Json;

namespace MixinSdk
{
    public partial class MixinApi
    {
        /// <summary>
        /// Reads the profile.
        /// </summary>
        /// <returns>The profile.</returns>
        public UserInfo ReadProfile()
        {
            const string req = "/me";

            var rz = doGetRequest(req, true);
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
            const string req = "/me/preferences";

            UserPerference p = new UserPerference
            {
                receive_message_source = receiveMessageSource,
                accept_conversation_source = acceptConversationSource
            };

            var rz = doPostRequest(req, p, true);

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
            const string req = "/me";
            
            UserProfile p = new UserProfile
            {
                full_name = fullName,
                avatar_base64 = avatarBase64
            };

            var rz = doPostRequest(req, p, true);

            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Reads the users.
        /// </summary>
        /// <returns>The users.</returns>
        /// <param name="userUuids">User uuids.</param>
        public List<UserInfo> ReadUsers(List<String> userUuids)
        {
            const string req = "/users/fetch";

            var rz = doPostRequest(req, userUuids, true);

            return JsonConvert.DeserializeObject<List<UserInfo>>(rz);
        }

        /// <summary>
        /// Reads the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="userUuid">User UUID.</param>
        public UserInfo ReadUser(string userUuid)
        {
            string req = "/users/" + userUuid;

            var rz = doGetRequest(req, true);
            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Searchs the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="mixinIdOrPhoneNo">Mixin identifier or phone no.</param>
        public UserInfo SearchUser(string mixinIdOrPhoneNo)
        {
            string req = "/search/" + mixinIdOrPhoneNo;

            var rz = doGetRequest(req, true);
            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Rotates the user qr.
        /// </summary>
        /// <returns>The user qr.</returns>
        public UserInfo RotateUserQR()
        {
            const string req = "/me/code";

            var rz = doGetRequest(req, true);
            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Friends this instance.
        /// </summary>
        /// <returns>The friends.</returns>
        public List<UserInfo> Friends()
        {
            const string req = "/friends";

            var rz = doGetRequest(req, true);
            return JsonConvert.DeserializeObject<List<UserInfo>>(rz);
        }

        /// <summary>
        /// Creates the attachment.
        /// </summary>
        /// <returns>The attachment.</returns>
        public Attachment CreateAttachment()
        {
            const string req = "/attachments";

            var rz = doPostRequest(req, null, true);
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

            var rz = doPostRequest(req, p, true);
            return JsonConvert.DeserializeObject<Conversation>(rz);
        }

        /// <summary>
        /// Reads the conversation.
        /// </summary>
        /// <returns>The conversation.</returns>
        /// <param name="conversationId">Conversation identifier.</param>
        public Conversation ReadConversation(string conversationId)
        {
            string req = "/conversations/" + conversationId;

            var rz = doGetRequest(req, true);
            return JsonConvert.DeserializeObject<Conversation>(rz);
        }
    }
}
