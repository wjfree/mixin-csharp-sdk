using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace MixinSdk
{
    public class GZipHelper
    {
        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] rawData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var inStram = new MemoryStream(rawData))
                {
                    GZip.Compress(inStram, ms, false);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// ZIP解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] zippedData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var inStram = new MemoryStream(zippedData))
                {
                    GZip.Decompress(inStram, ms, false);
                }

                return ms.ToArray();
            }
        }
    }
}
