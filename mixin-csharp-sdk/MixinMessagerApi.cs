using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using RestSharp;
using MixinSdk.Bean;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MixinSdk
{
    public class MixinMessagerApi : MixinApi
    {
        public UserInfo ReadProfile()
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/me";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = mixin_utils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<UserInfo>(response.Data.data);

            return rz;
        }

        public UserInfo UpdatePerference(string receiveMessageSource, string acceptConversationSource)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/me/preferences";

            var request = new RestRequest(req, Method.POST);

            UserPerference p = new UserPerference
            {
                receive_message_source = receiveMessageSource,
                accept_conversation_source = acceptConversationSource
            };


            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = mixin_utils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<UserInfo>(response.Data.data);

            return rz;
        }

        public UserInfo UpdateProfile(string fullName, string avatarBase64)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/me";

            var request = new RestRequest(req, Method.POST);

            UserProfile p = new UserProfile
            {
                full_name = fullName,
                avatar_base64 = avatarBase64
            };


            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = mixin_utils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<UserInfo>(response.Data.data);

            return rz;
        }

        public List<UserInfo> ReadUsers(List<String> userUuids)
        {

            if (!isInited)
            {
                return null;
            }

            const string req = "/users/fetch";

            var request = new RestRequest(req, Method.POST);


            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(userUuids);

            string token = mixin_utils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(userUuids), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<UserInfo>>(response.Data.data);

            return rz;
        }

        public UserInfo ReadUser(string userUuid)
        {

            if (!isInited)
            {
                return null;
            }

            string req = "/users/" + userUuid;

            var request = new RestRequest(req, Method.GET);


            request.AddHeader("Content-Type", "application/json");

            string token = mixin_utils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<UserInfo>(response.Data.data);

            return rz;
        }

        public UserInfo SearchUser(string mixinIdOrPhoneNo)
        {
            if (!isInited)
            {
                return null;
            }

            string req = "/search/" + mixinIdOrPhoneNo;

            var request = new RestRequest(req, Method.GET);


            request.AddHeader("Content-Type", "application/json");

            string token = mixin_utils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<UserInfo>(response.Data.data);

            return rz;
        }

        public UserInfo RotateUserQR()
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/me/code";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = mixin_utils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<UserInfo>(response.Data.data);

            return rz;

        }

        public List<UserInfo> Friends()
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/friends";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = mixin_utils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<UserInfo>>(response.Data.data);

            return rz;
        }

        public Attachment CreateAttachment()
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/attachments";

            var request = new RestRequest(req, Method.POST);

            request.AddHeader("Content-Type", "application/json");

            string token = mixin_utils.GenJwtAuthCode("POST", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Attachment>(response.Data.data);

            return rz;
        }

        private string UniqueConversationId(string userId, string recipientId)
        {

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

            var uuid = new Guid(sum);
            return uuid.ToString();
        }


        public Conversation CreateConversation(string category, List<ParticipantAction> participants)
        {
            if (!isInited)
            {
                return null;
            }

            if(!"GROUP".Equals(category) && !"CONTACT".Equals(category))
            {
                return null;
            }

            string conversationId = Guid.NewGuid().ToString();
            if ("GROUP".Equals(category))
            {
                // todo : conversationId = UniqueConversationId();
            }


            const string req = "/conversations";

            var request = new RestRequest(req, Method.POST);
            var p = new CreateConversationReq
            {
                category = category,
                conversation_id = conversationId,
                participants = participants
            };


            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = mixin_utils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Conversation>(response.Data.data);

            return rz;
        }

        public object ReadConversation(string conversationId)
        {
            if (!isInited)
            {
                return null;
            }

            string req = "/conversations/" + conversationId;

            var request = new RestRequest(req, Method.GET);


            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = mixin_utils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Conversation>(response.Data.data);

            return rz;
        }
    }
}
