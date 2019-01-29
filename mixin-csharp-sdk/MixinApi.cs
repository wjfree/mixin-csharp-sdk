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

        protected void CheckAuth()
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

            priKey = RSA.RSA_PEM.FromPEM(userConfig.PrivateKey);


            var rsaParams = priKey.ExportParameters(true);
            var Modulus = MixinUtils.makeBigInt(rsaParams.Modulus);
            var Exponent = MixinUtils.makeBigInt(rsaParams.Exponent);
            var D = MixinUtils.makeBigInt(rsaParams.D);
            var P = MixinUtils.makeBigInt(rsaParams.P);
            var Q = MixinUtils.makeBigInt(rsaParams.Q);
            var DP = MixinUtils.makeBigInt(rsaParams.DP);
            var DQ = MixinUtils.makeBigInt(rsaParams.DQ);
            var InverseQ = MixinUtils.makeBigInt(rsaParams.InverseQ);
            rsaParameters = new RsaPrivateCrtKeyParameters(Modulus, Exponent, D, P, Q, DP, DQ, InverseQ);

            isInited = true;
        }
    }
}
