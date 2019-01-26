using System.Security.Cryptography;
using MixinSdk.Bean;
using Org.BouncyCastle.Crypto.Parameters;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MixinSdk
{
    public class MixinNetworkApi
    {
        private MixinUserConfig userConfig = new MixinUserConfig();
        private RSACryptoServiceProvider priKey;
        private RsaPrivateCrtKeyParameters rsaParameters;
        private bool isInited = false;
        RestClient client = new RestClient(Config.MIXIN_API_URL);

        public MixinNetworkApi()
        {
        }

        public void Init(string ClientId, string ClientSecret, string SessionId, string PinToken, string PrivateKey)
        {
            userConfig.ClientId = ClientId;
            userConfig.ClientSecret = ClientSecret;
            userConfig.SessionId = SessionId;
            userConfig.PinToken = PinToken;
            userConfig.PrivateKey = PrivateKey;

            priKey = RSA.RSA_PEM.FromPEM(userConfig.PrivateKey);


            var rsaParams = priKey.ExportParameters(true);
            var Modulus = mixin_utils.makeBigInt(rsaParams.Modulus);
            var Exponent = mixin_utils.makeBigInt(rsaParams.Exponent);
            var D = mixin_utils.makeBigInt(rsaParams.D);
            var P = mixin_utils.makeBigInt(rsaParams.P);
            var Q = mixin_utils.makeBigInt(rsaParams.Q);
            var DP = mixin_utils.makeBigInt(rsaParams.DP);
            var DQ = mixin_utils.makeBigInt(rsaParams.DQ);
            var InverseQ = mixin_utils.makeBigInt(rsaParams.InverseQ);
            rsaParameters = new RsaPrivateCrtKeyParameters(Modulus, Exponent, D, P, Q, DP, DQ, InverseQ);

            isInited = true;
        }

        public object CreatePIN(string oldPin, string newPin)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/pin/update";

            var request = new RestRequest(req, Method.POST);

            var oldPinBlock = mixin_utils.GenEncrypedPin(oldPin, userConfig.PinToken, userConfig.SessionId, rsaParameters);
            var newPinBlock = mixin_utils.GenEncrypedPin(newPin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            var p = new CreatePinReq();
            p.old_pin = oldPinBlock;
            p.pin = newPinBlock;

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


        public UserInfo VerifyPIN(string pin)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/pin/verify";

            var request = new RestRequest(req, Method.POST);

            var pinBlock = mixin_utils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            VerifyPinReq p = new VerifyPinReq();
            p.pin = pinBlock;

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = mixin_utils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if(null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<UserInfo>(response.Data.data);

            return rz;
        }

        public Asset Deposit(string assetID)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/assets/";

            var request = new RestRequest(req + assetID, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = mixin_utils.GenJwtAuthCode("GET", req + assetID, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Asset>(response.Data.data);

            return rz;
        }

        public object Withdrawal(string addressId, string amount, string pin, string traceId, string memo)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/withdrawals";

            var request = new RestRequest(req, Method.POST);

            var pinBlock = mixin_utils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            WithDrawalReq p = new WithDrawalReq
            {
                address_id = addressId,
                amount = amount,
                pin = pinBlock,
                trace_id = traceId,
                memo = memo
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

            var rz = JsonConvert.DeserializeObject<WithDrawalRsp>(response.Data.data);

            return rz;
        }

        public Address CreateAddress(string assetId, string publicKey, string label, string accountName, string accountTag, string pin)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/addresses";

            var request = new RestRequest(req, Method.POST);

            var pinBlock = mixin_utils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            CreateAddressReq p = new CreateAddressReq
            {
                asset_id = assetId,
                public_key = publicKey,
                label = label,
                account_name = accountName,
                account_tag = accountTag,
                pin = pinBlock
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

            var rz = JsonConvert.DeserializeObject<Address>(response.Data.data);

            return rz;
        }

        public object DeleteAddress()
        {
            return null;
        }

        public Address ReadAddress(string addressId)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/addresses/";

            var request = new RestRequest(req + addressId, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = mixin_utils.GenJwtAuthCode("GET", req + addressId, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Address>(response.Data.data);

            return rz;

        }

        public object WithdrawalAddresses(string assetId)
        {
            if (!isInited)
            {
                return null;
            }

            string req = "/assets/" + assetId + "/addresses";
            

            var request = new RestRequest(req + assetId, Method.GET);

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

            var rz = JsonConvert.DeserializeObject<List<Address>>(response.Data.data);

            return rz;

        }

        public object ReadAsset(string asset)
        {
            return Deposit(asset);
        }

        public object ReadAssets()
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/assets";

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

            var rz = JsonConvert.DeserializeObject<List<Asset>>(response.Data.data);

            return rz;
        }

        public VerifyPaymentRsp VerifyPayment(string assetId, string opponentId, string amount, string traceId)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/payments";

            var request = new RestRequest(req, Method.POST);

            VerifyPaymentReq p = new VerifyPaymentReq
            {
                asset_id = assetId,
                opponent_id = opponentId,
                amount = amount,
                trace_id = traceId
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

            var rz = JsonConvert.DeserializeObject<VerifyPaymentRsp>(response.Data.data);

            return rz;
        }

        public Transfer Transfer(string assetId, string opponentId, string amount, string pin, string traceId, string memo)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/transfers";

            var pinBlock = mixin_utils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            var request = new RestRequest(req, Method.POST);

            TransferReq p = new TransferReq
            {
                asset_id = assetId,
                opponent_id = opponentId,
                amount = amount,
                trace_id = traceId,
                pin = pinBlock,
                memo = memo
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

            var rz = JsonConvert.DeserializeObject<Transfer>(response.Data.data);

            return rz;
        }

        public Transfer ReadTransfer(string traceId)
        {
            if (!isInited)
            {
                return null;
            }

            string req = "/transfers/trace/"+traceId;

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

            var rz = JsonConvert.DeserializeObject<Transfer>(response.Data.data);

            return rz;

        }

        public List<Asset> TopAssets()
        {
            if (!isInited)
            {
                return null;
            }

            string req = "network/assets/top";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<Asset>>(response.Data.data);

            return rz;
        }

        public NetworkAsset NetworkAsset(string assetId)
        {
            if (!isInited)
            {
                return null;
            }

            string req = "/network/assets/" + assetId;

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

            var rz = JsonConvert.DeserializeObject<NetworkAsset>(response.Data.data);

            return rz;

        }

        public List<Asset> NetworkSnapshots(int limit, string offset, string assetId, string order, bool isAuth)
        {
            if (!isInited)
            {
                isAuth = false;
            }

            string req = "/network/snapshots";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("limit", limit, ParameterType.QueryString);
            request.AddParameter("offset", offset, ParameterType.QueryString);
            if (!string.IsNullOrEmpty(assetId))
            {
                request.AddParameter("asset", assetId);
            }
            if(!string.IsNullOrEmpty(order))
            {
                request.AddParameter("order", order);
            }

            if (isAuth)
            {
                string token = mixin_utils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

                var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
                jwtAuth.Authenticate(client, request);
            }

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<Asset>>(response.Data.data);

            return rz;
        }

        public Asset NetworkSnapshot( string snapshotId, bool isAuth)
        {
            if (!isInited)
            {
                isAuth = false;
            }

            string req = "/network/snapshots" + snapshotId;

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            if (isAuth)
            {
                string token = mixin_utils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

                var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
                jwtAuth.Authenticate(client, request);
            }

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Asset>(response.Data.data);

            return rz;
        }

        public List<ExternalTransation> ExternalTransactions(string assetId, string publicKey, string accountTag, string accountName, int? limit, string offset)
        {

            string req = "/external/transactions";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(assetId))
            {
                request.AddParameter("asset", assetId);
            }

            if (!string.IsNullOrEmpty(publicKey))
            {
                request.AddParameter("public_key", publicKey);
            }

            if (!string.IsNullOrEmpty(accountTag))
            {
                request.AddParameter("account_tag", accountTag);
            }

            if (!string.IsNullOrEmpty(accountName))
            {
                request.AddParameter("account_name", accountName);
            }

            if (null != limit)
            {
                request.AddParameter("limit", limit, ParameterType.QueryString);
            }

            if (!string.IsNullOrEmpty(offset))
            {
                request.AddParameter("offset", offset, ParameterType.QueryString);
            }

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<ExternalTransation>>(response.Data.data);

            return rz;
        }

        public List<Asset> SearchAssets(string assetName)
        {
            string req = "/network/assets/search/" + assetName;

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<Asset>>(response.Data.data);

            return rz;

        }

        public UserInfo APPUser(string fullName, string sessionSecret)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/users";
            
            var request = new RestRequest(req, Method.POST);

            NewUser p = new NewUser
            {
                full_name = fullName,
                session_secret = sessionSecret
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
    }
}
