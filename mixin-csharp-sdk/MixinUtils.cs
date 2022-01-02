using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using NeoSmart.Utils;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Rebex.Security.Cryptography;
using Newtonsoft.Json;


namespace MixinSdk
{
    public static class MixinUtils
    {
        private static Random random = new Random();


        private static string BCD2ASC(byte[] bBcd)
        {
            string ret = "";
            foreach (byte b in bBcd)
            {
                ret += b.ToString("x2");
            }
            return ret;
        }


        public static long ToUnixTime(DateTime datetime)
        {
            System.DateTime startTime = new System.DateTime(1970, 1, 1); // 当地时区
            long timeStamp = (long)(datetime - startTime).TotalSeconds; // 相差秒数
            return timeStamp;
        }

        public static string GenJwtAuthCode(string method, string uri, string body, string clientId, string sessionId, string priKey)
        {
            byte[] bsha256 = null;

            using (SHA256 mySHA256 = SHA256.Create())
            {
                bsha256 = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(method + uri + body));
            }

            var bPriToken = UrlBase64.Decode(priKey);

            Ed25519 edkey = (Ed25519)Ed25519.Create("ed25519-sha512");
            edkey.FromPrivateKey(bPriToken);

            var payload = new Dictionary<string, object>()
                        {
                            {"uid", clientId },
                            {"sid", sessionId },
                            {"iat", ToUnixTime(DateTime.UtcNow)},
                            {"exp", ToUnixTime(DateTime.UtcNow) + 3600*24}, //过期时间暂定一天
                            {"jti", Guid.NewGuid().ToString()},
                            {"sig", BCD2ASC(bsha256)},
                            {"scp", "FULL" }
                        };

            var header = new Dictionary<string, object>()
                        {
                            { "alg", "EdDSA"},
                            { "typ", "JWT"},
                            { "kty", "OKP"},
                            { "crv", "Ed25519"},
                            { "x", UrlBase64.Encode(edkey.GetPublicKey()) }
                        };

            var szHeader = JsonConvert.SerializeObject(header);
            var szPayload = JsonConvert.SerializeObject(payload);

            var signData = UrlBase64.Encode(Encoding.ASCII.GetBytes(szHeader)) +"."
                         + UrlBase64.Encode(Encoding.ASCII.GetBytes(szPayload));


            string sign = UrlBase64.Encode(edkey.SignMessage(Encoding.ASCII.GetBytes(signData)));

            return signData+"."+ sign;
        }


        private static byte[] PrivateKeyToCurve25519(byte[] edPrivateKey)
        {
            byte[] bsha512 = null;
            using (SHA512 mySHA512 = SHA512.Create())
            {
                bsha512 = mySHA512.ComputeHash(edPrivateKey);
            }

            bsha512[0] &= 248;

            bsha512[31] &= 127;

            bsha512[31] |= 64;

            byte[] rz = new byte[32];

            Array.Copy(bsha512, rz, 32);
            return rz;
        }


        public static string GenEncrypedPin(string pin, string pinToken, string sessionId, string priKey, ulong it)
        {
            ulong time = (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var bPinToken = UrlBase64.Decode(pinToken);
            var bPriToken = UrlBase64.Decode(priKey);
            byte[] bPriToken2 = new byte[32];
            Array.Copy(bPriToken, bPriToken2, 32);

            var bPriToken3 = PrivateKeyToCurve25519(bPriToken2);

            Curve25519 pkey = Curve25519.Create(Curve25519.Curve25519Sha256);
            pkey.FromPrivateKey(bPriToken3);

            byte[] sk = pkey.GetSharedSecret(bPinToken);

            var bPin = Encoding.ASCII.GetBytes(pin);
            var btime = BitConverter.GetBytes(time);
            var biterator = BitConverter.GetBytes(it);


            int len = bPin.Length + btime.Length + biterator.Length;

            IBlockCipher cipherAes = new AesEngine();
            int bsize = cipherAes.GetBlockSize();
            KeyParameter keyParameter = new KeyParameter(sk);


            int nPadding = bsize - len % bsize;
            var bPadding = new byte[nPadding];
            len += (len % bsize == 0 ? 0 : nPadding);


            var blocks = new byte[len];
            Array.Copy(bPin, blocks, bPin.Length);
            Array.Copy(btime, 0, blocks, bPin.Length, btime.Length);
            Array.Copy(biterator, 0, blocks, bPin.Length + btime.Length, biterator.Length);
            Array.Copy(bPadding, 0, blocks, bPin.Length + btime.Length + biterator.Length, nPadding);

            var iv = new byte[bsize];
            random.NextBytes(iv);

            CbcBlockCipher cbcBc = new CbcBlockCipher(cipherAes);
            ParametersWithIV parametersWithIV = new ParametersWithIV(keyParameter, iv);

            BufferedBlockCipher bc = new BufferedBlockCipher(cbcBc);

            bc.Init(true, parametersWithIV);
            var bOut = bc.ProcessBytes(blocks);
            var rz = new byte[bOut.Length + bsize];
            Array.Copy(iv, rz, iv.Length);
            Array.Copy(bOut, 0, rz, iv.Length, bOut.Length);

            return UrlBase64.Encode(rz);
        }

    }
}
