using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MixinSdk.Bean;
using Newtonsoft.Json;
using RestSharp;
using NeoSmart.Utils;
using Rebex.Security.Cryptography;

namespace MixinSdk
{
    public partial class MixinApi
    {
        public const string MIXIN_API_URL = "https://api.mixin.one";
        public const string MIXIN_WEBSOCKET_URL = "wss://blaze.mixin.one/";

        /// <summary>
        /// Gets or sets the read timeout.
        /// </summary>
        /// <value>Read timeout</value>
        public int ReadTimeout { get; set; } = 10000;

        private MixinUserConfig userConfig = new MixinUserConfig();
        private bool isInited = false;
        private RestClient client = new RestClient(MIXIN_API_URL);

        private void CheckAuth()
        {
            if (!isInited)
            {
                throw new NeedAuthException();
            }
        }

        /// <summary>
        /// Init MixinApi use specified ClientId, ClientSecret, SessionId, PinToken and PrivateKey.
        /// </summary>
        /// <param name="ClientId">Client identifier.</param>
        /// <param name="ClientSecret">Client secret.</param>
        /// <param name="SessionId">Session identifier.</param>
        /// <param name="PinToken">Pin token.</param>
        /// <param name="PrivateKey">Private key.</param>
        public void Init(string ClientId, string ClientSecret, string SessionId, string PinToken, string PrivateKey)
        {
            userConfig.ClientId = ClientId;
            userConfig.ClientSecret = ClientSecret;
            userConfig.SessionId = SessionId;
            userConfig.PinToken = PinToken;
            userConfig.PrivateKey = PrivateKey;

            isInited = true;
        }

        private UInt64 iterator = (UInt64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        private UInt64 getIterator()
        {
            iterator++;
            return iterator;
        }

        private string GenGetJwtToken(string uri, string body)
        {
            return MixinUtils.GenJwtAuthCode("GET", uri, body, userConfig.ClientId, userConfig.SessionId, userConfig.PrivateKey);
        }

        private string GenPostJwtToken(string uri, string body)
        {
            return MixinUtils.GenJwtAuthCode("POST", uri, body, userConfig.ClientId, userConfig.SessionId, userConfig.PrivateKey);
        }

        private string GenEncrypedPin(string pin)
        {
            return MixinUtils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, userConfig.PrivateKey, getIterator());
        }

        private async Task<string> doPostRequestAsync(string uri, object o, bool isNeedAuth, string token = null)
        {
            var request = new RestRequest(uri, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(o);

            if (isNeedAuth)
            {
                if (string.IsNullOrEmpty(token))
                {
                    CheckAuth();
                    token = GenPostJwtToken(uri, JsonConvert.SerializeObject(o));
                }
                var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
                jwtAuth.Authenticate(client, request);
            }

            var cts = new CancellationTokenSource(ReadTimeout);
            var response = await client.ExecuteTaskAsync<Data>(request, cts.Token);

            if (null == response.Data.data)
            {
                if (response.Content.Equals("{}"))
                {
                    return response.Content;
                }
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            return response.Data.data;
        }

        private async Task<string> doGetRequestAsync(string uri, bool isNeedAuth, string token = null)
        {
            var request = new RestRequest(uri, Method.GET);
            request.AddHeader("Content-Type", "application/json");

            if (isNeedAuth)
            {
                if (string.IsNullOrEmpty(token))
                {
                    CheckAuth();
                    token = GenGetJwtToken(uri, "");
                }
                var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
                jwtAuth.Authenticate(client, request);
            }

            var cts = new CancellationTokenSource(ReadTimeout);
            var response = await client.ExecuteTaskAsync<Data>(request, cts.Token);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            return response.Data.data;
        }
    }
}
