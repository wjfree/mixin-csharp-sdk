using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RSA {
	/// <summary>
	/// RSA PEM格式秘钥对的解析和导出
	/// </summary>
	public class RSA_PEM {
		/// <summary>
		/// 用PEM格式密钥对创建RSA，支持PKCS#1、PKCS#8格式的PEM
		/// </summary>
		public static RSACryptoServiceProvider FromPEM(string pem) {

			var rsa = new RSACryptoServiceProvider();

			var param = new RSAParameters();

			var base64 = _PEMCode.Replace(pem, "");
			var data = RSA_Unit.Base64DecodeBytes(base64);
			if (data == null) {
				throw new Exception("PEM内容无效");
			}
			var idx = 0;

			//读取长度
			Func<byte, int> readLen = (first) => {
				if (data[idx] == first) {
					idx++;
					if (data[idx] == 0x81) {
						idx++;
						return data[idx++];
					} else if (data[idx] == 0x82) {
						idx++;
						return (((int)data[idx++]) << 8) + data[idx++];
					} else if (data[idx] < 0x80) {
						return data[idx++];
					}
				}
				throw new Exception("PEM未能提取到数据");
			};
			//读取块数据
			Func<byte[]> readBlock = () => {
				var len = readLen(0x02);
				if (data[idx] == 0x00) {
					idx++;
					len--;
				}
				var val = data.sub(idx, len);
				idx += len;
				return val;
			};
			//比较data从idx位置开始是否是byts内容
			Func<byte[], bool> eq = (byts) => {
				for (var i = 0; i < byts.Length; i++, idx++) {
					if (idx >= data.Length) {
						return false;
					}
					if (byts[i] != data[idx]) {
						return false;
					}
				}
				return true;
			};




			if (pem.Contains("PUBLIC KEY")) {
				/****使用公钥****/
				//读取数据总长度
				readLen(0x30);
				if (!eq(_SeqOID)) {
					throw new Exception("PEM未知格式");
				}
				//读取1长度
				readLen(0x03);
				idx++;//跳过0x00
				//读取2长度
				readLen(0x30);

				//Modulus
				param.Modulus = readBlock();

				//Exponent
				param.Exponent = readBlock();
			} else if (pem.Contains("PRIVATE KEY")) {
				/****使用私钥****/
				//读取数据总长度
				readLen(0x30);

				//读取版本号
				if (!eq(_Ver)) {
					throw new Exception("PEM未知版本");
				}

				//检测PKCS8
				var idx2 = idx;
				if (eq(_SeqOID)) {
					//读取1长度
					readLen(0x04);
					//读取2长度
					readLen(0x30);

					//读取版本号
					if (!eq(_Ver)) {
						throw new Exception("PEM版本无效");
					}
				} else {
					idx = idx2;
				}

				//读取数据
				param.Modulus = readBlock();
				param.Exponent = readBlock();
				param.D = readBlock();
				param.P = readBlock();
				param.Q = readBlock();
				param.DP = readBlock();
				param.DQ = readBlock();
				param.InverseQ = readBlock();
			} else {
				throw new Exception("pem需要BEGIN END标头");
			}

			rsa.ImportParameters(param);
			return rsa;
		}
		static private Regex _PEMCode = new Regex(@"--+.+?--+|\s+");
		static private byte[] _SeqOID = new byte[] { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
		static private byte[] _Ver = new byte[] { 0x02, 0x01, 0x00 };

	}
}
