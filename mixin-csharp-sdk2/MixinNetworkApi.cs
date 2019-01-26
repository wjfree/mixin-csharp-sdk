using System.Security.Cryptography;
using MixinSdk.Bean;
using Org.BouncyCastle.Crypto.Parameters;
using RestSharp;
using Newtonsoft.Json;

namespace MixinSdk
{
    public class MixinNetworkApi
    {
        private MixinUserConfig userConfig = new MixinUserConfig();
        private RSACryptoServiceProvider priKey;
        private RsaPrivateCrtKeyParameters rsaParameters;
        private bool isInited = false;

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

            var client = new RestClient(Config.MIXIN_API_URL);
            var request = new RestRequest(req, Method.POST);

            var oldPinBlock = mixin_utils.GenEncrypedPin(oldPin, userConfig.PinToken, userConfig.SessionId, rsaParameters);
            var newPinBlock = mixin_utils.GenEncrypedPin(newPin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            var p = new CreatePinReq();
            p.old_pin = oldPinBlock;
            p.pin = newPinBlock;

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = mixin_utils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(token);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<PinInfo>(response.Data.data);

            return rz;
        }


        public PinInfo VerifyPIN(string pin)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/pin/verify";

            var client = new RestClient(Config.MIXIN_API_URL);
            var request = new RestRequest(req, Method.POST);

            var pinBlock = mixin_utils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            VerifyPinReq p = new VerifyPinReq();
            p.pin = pinBlock;

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = mixin_utils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(token);

            var response = client.Execute<Data>(request);

            if(null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<PinInfo>(response.Data.data);

            return rz;
        }

        public Asset Deposit(string assetID)
        {
            if (!isInited)
            {
                return null;
            }

            const string req = "/assets/";

            var client = new RestClient(Config.MIXIN_API_URL);
            var request = new RestRequest(req + assetID, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = mixin_utils.GenJwtAuthCode("GET", req + assetID, "", userConfig.ClientId, userConfig.SessionId, priKey);

            client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(token);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Asset>(response.Data.data);

            return rz;
        }

        public object Withdrawal()
        {
            return null;
        }

        public object CreateAddress()
        {
            return null;
        }

        public object DeleteAddress()
        {
            return null;
        }

        public object ReadAddress()
        {
            return null;
        }

        public object WithdrawalAddresses()
        {
            return null;
        }

        public object ReadAsset(string asset)
        {
            return Deposit(asset);
        }

        public object ReadAssets()
        {
            return null;
        }

        public object VerifyPayment()
        {
            return null;
        }

        public object Transfer()
        {
            return null;
        }

        public object ReadTransfer()
        {
            return null;
        }

        public object TopAssets()
        {
            return null;
        }

        public object NetworkAsset()
        {
            return null;
        }

        public object NetworkSnapshots()
        {
            return null;
        }

        public object NetworkSnapshot()
        {
            return null;
        }

        public object ExternalTransactions()
        {
            return null;
        }

        public object SearchAssets()
        {
            return null;
        }

        public object APPUser()
        {
            return null;
        }
    }
}
