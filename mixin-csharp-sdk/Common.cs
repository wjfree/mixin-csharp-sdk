using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixinSdk
{
    class Common
    {
        public static byte[] ASC2BCD(string szAsc)
        {
            byte[] ret = new byte[szAsc.Length / 2];
            for (int i = 0; i < szAsc.Length; i += 2)
            {
                string tmp = szAsc.Substring(i, 2);
                ret[i / 2] = Convert.ToByte(tmp, 16);
            }
            return ret;
        }



        public static string BCD2ASC(byte[] bBcd)
        {
            string ret = "";
            foreach (byte b in bBcd)
            {
                ret += b.ToString("x2");
            }
            return ret;
        }

        public static byte[] xor(byte[] a, byte[] b, int Len)
        {
            if (Len > a.Length || Len > b.Length)
            {
                throw new Exception("异或数据长度错误");
            }
            byte[] ret = new byte[Len];

            for (int i = 0; i < Len; i++)
            {
                ret[i] = (byte)(a[i] ^ b[i]);
            }

            return ret;
        }

        public enum PaddingMode
        {
            Pad80Len8,
            Pad80Len8Force,
            Pad00Len8,
            Pad80Len16,
            Pad00Len16,
            Pad80Len16Force
        }
        public static byte[] padding(byte[] inData, PaddingMode mode)
        {
            byte[] ret = null;
            int len = inData.Length;
            int offset = 0;
            byte[] pad80 = { 0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] pad00 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] pad = null;
            switch (mode)
            {
                case PaddingMode.Pad80Len8:
                    len = (len / 8) * 8 + (len % 8 == 0 ? 0 : 8);
                    pad = pad80;
                    break;
                case PaddingMode.Pad80Len8Force:
                    len = (len / 8) * 8 + 8;
                    pad = pad80;
                    break;
                case PaddingMode.Pad80Len16:
                    len = (len / 16) * 16 + (len % 16 == 0 ? 0 : 16);
                    pad = pad80;
                    break;
                case PaddingMode.Pad80Len16Force:
                    len = (len / 16) * 16 + 16;
                    pad = pad80;
                    break;
                case PaddingMode.Pad00Len8:
                    len = (len / 8) * 8 + (len % 8 == 0 ? 0 : 8);
                    pad = pad00;
                    break;
                case PaddingMode.Pad00Len16:
                    len = (len / 16) * 16 + (len % 16 == 0 ? 0 : 16);
                    pad = pad00;
                    break;
            }
            ret = new byte[len];
            System.Array.Copy(inData, 0, ret, offset, inData.Length);
            offset += inData.Length;
            System.Array.Copy(pad, 0, ret, offset, len - offset);

            return ret;
        }

        public static long ToUnixTime(DateTime datetime)
        {
            System.DateTime startTime = new System.DateTime(1970, 1, 1); // 当地时区
            long timeStamp = (long)(datetime - startTime).TotalSeconds; // 相差秒数
            return timeStamp;
        }
    }
}
