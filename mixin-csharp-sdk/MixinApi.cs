using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using RestSharp;

namespace MixinSdk
{
    public class MixinApi
    {
        protected MixinUserConfig userConfig = new MixinUserConfig();
        protected RSACryptoServiceProvider priKey;
        protected RsaPrivateCrtKeyParameters rsaParameters;
        protected bool isInited = false;
        protected RestClient client = new RestClient(Config.MIXIN_API_URL);

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
    }
}
