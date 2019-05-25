using System;
using System.Security.Cryptography;
using System.Text;

namespace IQF.Framework.Encrypt
{
	/// <summary>
	/// 加密类。
	/// </summary>
	public class TripleDESCryptogram
	{
		/// <summary>
		/// 默认秘钥
		/// </summary>
		private readonly static string DefaultSecretKey = "niugu123niugu456";

		/// <summary>
		/// 使用默认秘钥加密
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public static string Encrypt(string plainText)
		{
			string val = Encrypt(plainText, DefaultSecretKey);
			return val;
		}

		/// <summary>
		/// 根据明文，做DES加密，并返回Hex字符串
		/// </summary>
		/// <param name="plainText">明文</param>
		/// <param name="desKey">注意3DES要求key为8*3=24字节。C#会自动补全，其他语言可能需要手动补全</param>
		/// <returns></returns>
		public static string Encrypt(string plainText, string desKey)
		{
			byte[] data = Encoding.UTF8.GetBytes(plainText);
			using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
			{
				tdes.Mode = CipherMode.CBC;
				tdes.Padding = PaddingMode.PKCS7;

				tdes.Key = Encoding.UTF8.GetBytes(desKey);
				tdes.IV = Encoding.UTF8.GetBytes("12312300");
				ICryptoTransform cTransform = tdes.CreateEncryptor();
				byte[] resultArray = cTransform.TransformFinalBlock(data, 0, data.Length);
				string ret = BytesToHexString(resultArray);
				return ret;
			}
		}

		/// <summary>
		/// 使用默认秘钥解密
		/// </summary>
		/// <param name="hexString"></param>
		/// <returns></returns>
		public static string Decrypt(string hexString)
		{
			string val = Decrypt(hexString, DefaultSecretKey);
			return val;
		}

		/// <summary>
		/// 根据Hex字符串，DES解密出json格式的参数
		/// </summary>
		/// <param name="hexString"></param>
		/// <param name="desKey"></param>
		/// <returns></returns>
		public static string Decrypt(string hexString, string desKey)
		{
			byte[] data = HexStringToBytes(hexString);

			using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
			{
				tdes.Key = Encoding.UTF8.GetBytes(desKey);
				tdes.IV = Encoding.UTF8.GetBytes("12312300");
				ICryptoTransform cTransform = tdes.CreateDecryptor();
				byte[] resultArray = cTransform.TransformFinalBlock(data, 0, data.Length);
				string ret = UTF8Encoding.UTF8.GetString(resultArray);
				return ret;
			}
		}

		private static byte[] HexStringToBytes(string hex)
		{
			int NumberChars = hex.Length;
			byte[] bytes = new byte[NumberChars / 2];
			for (int i = 0; i < NumberChars; i += 2)
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			return bytes;
		}

		public static string BytesToHexString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
				hex.AppendFormat("{0:x2}", b);
			return hex.ToString();
		}
	}
}
