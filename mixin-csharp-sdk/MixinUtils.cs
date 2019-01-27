using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Jose;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace MixinSdk
{
    public static class MixinUtils
    {
        private static Random random = new Random();

        private static UInt64 iterator = (UInt64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        private static UInt64 getIterator()
        {
            iterator++;
            return iterator;
        }

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

        public static string GenJwtAuthCode(string method, string uri, string body, string clientId, string sessionId, RSACryptoServiceProvider privateKey)
        {
            byte[] bsha256 = null;


            using (SHA256 mySHA256 = SHA256.Create())
            {
                var signData = method + uri + body;
                bsha256 = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(signData));
            }

            var payload = new Dictionary<string, object>()
                        {
                            {"uid", clientId },
                            {"sid", sessionId },
                            {"iat", ToUnixTime(DateTime.UtcNow)},
                            {"exp", ToUnixTime(DateTime.UtcNow) + 3600}, //过期时间暂定一小时
                            {"jti", System.Guid.NewGuid().ToString()},
                            {"sig", BCD2ASC(bsha256)}
                        };

            string token = Jose.JWT.Encode(payload, privateKey, JwsAlgorithm.RS512);

            return token;
        }

        public static BigInteger makeBigInt(byte[] bytes)
        {
            if ((sbyte)bytes[0] < 0)
            {
                // prepend a zero byte to make it positive.
                var bytes1 = new byte[bytes.Length + 1];
                bytes1[0] = 0;
                bytes.CopyTo(bytes1, 1);
                bytes = bytes1;
            }

            return new BigInteger(bytes);
        }

        public static string GenEncrypedPin(string pin, string pinToken, string sessionId, RsaPrivateCrtKeyParameters rsa)
        {
            UInt64 time = (UInt64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            UInt64 it = getIterator();

            var bPinToken = System.Convert.FromBase64String(pinToken);


            IAsymmetricBlockCipher cipherRsa = new RsaBlindedEngine();
            cipherRsa = new OaepEncoding(cipherRsa, new Sha256Digest(), new Sha256Digest(), Encoding.ASCII.GetBytes(sessionId));
            BufferedAsymmetricBlockCipher cipher = new BufferedAsymmetricBlockCipher(cipherRsa);
            cipher.Init(false, rsa);
            cipher.ProcessBytes(bPinToken, 0, bPinToken.Length);
            var key = cipher.DoFinal();


            var bPin = Encoding.ASCII.GetBytes(pin);
            var btime = BitConverter.GetBytes(time);
            var biterator = BitConverter.GetBytes(it);


            int len = bPin.Length + btime.Length + biterator.Length;

            IBlockCipher cipherAes = new AesEngine();
            int bsize = cipherAes.GetBlockSize();
            KeyParameter keyParameter = new KeyParameter(key);


            int nPadding = bsize - len % bsize;
            var bPadding = new byte[nPadding];
            len += (len % bsize == 0 ? 0 : nPadding);


            var blocks = new byte[len];
            System.Array.Copy(bPin, blocks, bPin.Length);
            System.Array.Copy(btime, 0, blocks, bPin.Length, btime.Length);
            System.Array.Copy(biterator, 0, blocks, bPin.Length + btime.Length, biterator.Length);
            System.Array.Copy(bPadding, 0, blocks, bPin.Length + btime.Length + biterator.Length, nPadding);

            var iv = new byte[bsize];
            random.NextBytes(iv);

            CbcBlockCipher cbcBc = new CbcBlockCipher(cipherAes);
            ParametersWithIV parametersWithIV = new ParametersWithIV(keyParameter, iv);

            BufferedBlockCipher bc = new BufferedBlockCipher(cbcBc);

            bc.Init(true, parametersWithIV);
            var bOut = bc.ProcessBytes(blocks);
            var rz = new byte[bOut.Length + bsize];
            System.Array.Copy(iv, rz, iv.Length);
            System.Array.Copy(bOut, 0, rz, iv.Length, bOut.Length);

            return System.Convert.ToBase64String(rz);
        }

    }
}
