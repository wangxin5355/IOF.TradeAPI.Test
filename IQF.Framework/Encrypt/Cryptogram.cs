using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IQF.Framework.Encrypt
{
	/// <summary>
	/// 加密类。
	/// </summary>
	[Obsolete("使用TripleDESCryptogram代替", false)]
	public static class Cryptogram
	{
		private readonly static TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

		static Cryptogram()
		{
			des.Mode = System.Security.Cryptography.CipherMode.CBC;
			des.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
		}

		/// <summary>
		/// 字符串MD5加密，返回大写字母的结果。
		/// </summary>
		/// <param name="str">输入值</param>
		/// <returns>返回大写字母MD5加密串</returns>
		public static string GetMD5(string str)
		{
			MD5 md5 = MD5.Create();//实例化一个md5对像
								   // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
			byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
			// 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < s.Length; i++)
			{
				// 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
				sb.Append(s[i].ToString("X2"));//ToString("x");
			}
			return sb.ToString();
		}

		public static string GetSHA1(string str)
		{
			SHA1 sha1 = SHA1.Create();
			byte[] s = sha1.ComputeHash(Encoding.UTF8.GetBytes(str));

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < s.Length; i++)
			{
				sb.Append(s[i].ToString("X2"));
			}
			return sb.ToString();
		}

		public static string EncryptPhotoid(string photoid)
		{
			if (true == string.IsNullOrEmpty(photoid)) return string.Empty;
			try
			{
				byte[] buf;

				if (Encrypt(KEY.passKEY, KEY.passIV, ConvertStringToByteArray(photoid), out buf))
				{
					StringBuilder sb = new StringBuilder();
					for (int i = 0; i < buf.Length; i++)
					{
						sb.Append(buf[i].ToString("X").Length == 2 ? buf[i].ToString("X") : "0" + buf[i].ToString("X"));
					}
					return sb.ToString();
				}
				else
					return string.Empty;
			}
			catch
			{
			}
			return string.Empty;
		}
		/// <summary>
		/// 加密密码
		/// </summary>
		/// <param name="Password">待加密的密码明文</param>
		/// <returns></returns>
		public static string EncryptPassword(string Password)
		{
			if (true == string.IsNullOrEmpty(Password)) return string.Empty;
			try
			{
				byte[] Encrypted;

				if (Encrypt(KEY.passKEY, KEY.passIV, ConvertStringToByteArray(Password), out Encrypted))
				{
					return ToBase64String(Encrypted);
				}
				else
					return string.Empty;
			}
			catch (Exception ex)
			{
				string a = ex.ToString();
			}
			return string.Empty;
		}
		/// <summary>
		/// 解密密码
		/// </summary>
		/// <param name="Password">待解密的密码密文</param>
		/// <returns></returns>
		public static string DecryptPassword(string Password)
		{
			if (true == string.IsNullOrEmpty(Password)) return string.Empty;
			try
			{
				byte[] Decrypted;

				if (Decrypt(KEY.passKEY, KEY.passIV, FromBase64String(Password), out Decrypted))
				{
					return ConvertByteArrayToString(Decrypted);
				}
				else
					return string.Empty;
			}
			catch
			{
			}
			return string.Empty;
		}
		/// <summary>
		/// 加密UserToken
		/// </summary>
		/// <param name="UserToken">待加密的UserToken明文</param>
		/// <returns></returns>
		public static string EncryptUserToken(string UserToken)
		{
			if (true == string.IsNullOrEmpty(UserToken)) return string.Empty;
			try
			{
				byte[] Encrypted;
				if (Encrypt(KEY.cookieKEY, KEY.cookieIV, ConvertStringToByteArray(UserToken), out Encrypted))
				{
					return ToBase64String(Encrypted);
				}
				else
					return string.Empty;
			}
			catch
			{
			}
			return string.Empty;
		}
		/// <summary>
		/// 解密UserToken
		/// </summary>
		/// <param name="UserToken">待解密的UserToken密文</param>
		/// <returns></returns>
		public static string DecryptUserToken(string UserToken)
		{
			if (true == string.IsNullOrEmpty(UserToken)) return string.Empty;
			try
			{
				byte[] Decrypted;
				if (Decrypt(KEY.cookieKEY, KEY.cookieIV, FromBase64String(UserToken), out Decrypted))
				{
					return ConvertByteArrayToString(Decrypted);
				}
				else
				{
					return string.Empty;
				}
			}
			catch
			{
			}
			return string.Empty;
		}
		/// <summary>
		/// 生成摘要
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string ComputeHashString(string s)
		{
			return ToBase64String(ComputeHash(ConvertStringToByteArray(s)));
		}
		private static byte[] ComputeHash(byte[] buf)
		{
			return ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(buf);
		}
		private static bool Encrypt(byte[] KEY, byte[] IV, byte[] TobeEncrypted, out byte[] Encrypted)
		{
			Encrypted = null;
			try
			{
				byte[] tmpiv = { 0, 1, 2, 3, 4, 5, 6, 7 };
				for (int ii = 0; ii < 8; ii++)
				{
					tmpiv[ii] = IV[ii];
				}
				byte[] tmpkey = { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 };
				for (int ii = 0; ii < 24; ii++)
				{
					tmpkey[ii] = KEY[ii];
				}
				ICryptoTransform tridesencrypt = des.CreateEncryptor(tmpkey, tmpiv);
				Encrypted = tridesencrypt.TransformFinalBlock(TobeEncrypted, 0, TobeEncrypted.Length);
				des.Clear();
				return true;
			}
			catch
			{
			}
			return false;
		}
		private static bool Decrypt(byte[] KEY, byte[] IV, byte[] TobeDecrypted, out byte[] Decrypted)
		{
			Decrypted = null;
			try
			{
				byte[] tmpiv = { 0, 1, 2, 3, 4, 5, 6, 7 };
				for (int ii = 0; ii < 8; ii++)
				{
					tmpiv[ii] = IV[ii];
				}
				byte[] tmpkey = { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7 };
				for (int ii = 0; ii < 24; ii++)
				{
					tmpkey[ii] = KEY[ii];
				}
				ICryptoTransform tridesdecrypt = des.CreateDecryptor(tmpkey, tmpiv);
				Decrypted = tridesdecrypt.TransformFinalBlock(TobeDecrypted, 0, TobeDecrypted.Length);
				des.Clear();
			}
			catch
			{
			}
			return true;
		}

		private static string ToBase64String(byte[] buf)
		{
			return System.Convert.ToBase64String(buf);
		}

		private static byte[] FromBase64String(string s)
		{
			return System.Convert.FromBase64String(s);
		}

		private static byte[] ConvertStringToByteArray(String s)
		{
			return System.Text.Encoding.GetEncoding("utf-8").GetBytes(s);
		}

		private static string ConvertByteArrayToString(byte[] buf)
		{
			return System.Text.Encoding.GetEncoding("utf-8").GetString(buf);
		}

		public static string StringToUnicode(string text)
		{
			string result = string.Empty;
			if (false == string.IsNullOrEmpty(text))
			{
				for (int i = 0; i < text.Length; i++)
				{
					//将中文字符转为10进制整数，然后转为16进制unicode字符
					result += "\\u" + ((int)(text[i])).ToString("x");
				}
			}
			return result;
		}
		public static string UnicodeToString(string unicodeStr)
		{
			string result = string.Empty;
			if (false == string.IsNullOrEmpty(unicodeStr))
			{
				string[] strlist = unicodeStr.Replace("\\", "").Split('u');
				try
				{
					for (int i = 1; i < strlist.Length; i++)
					{
						//将unicode字符转为10进制整数，然后转为char中文字符
						result += (char)(int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber));
					}
				}
				catch (FormatException ex)
				{
					result = ex.Message;
				}
			}
			return result;
		}


		/// <summary>
		/// 获取密钥
		/// </summary>
		public static string aesKey = string.Empty;

		/// <summary>
		/// 获取向量
		/// </summary>
		public static string aesIV = string.Empty;

		/// <summary>
		/// AES加密
		/// </summary>
		/// <param name="plainStr">明文字符串</param>
		/// <returns>密文</returns>
		public static string AESEncrypt(string plainStr)
		{
			byte[] bKey = Encoding.UTF8.GetBytes(aesKey);
			byte[] bIV = Encoding.UTF8.GetBytes(aesIV);
			byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

			string encrypt = null;
			Rijndael aes = Rijndael.Create();
			try
			{
				using (MemoryStream mStream = new MemoryStream())
				{
					using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
					{
						cStream.Write(byteArray, 0, byteArray.Length);
						cStream.FlushFinalBlock();
						encrypt = Convert.ToBase64String(mStream.ToArray());
					}
				}
			}
			catch { }
			aes.Clear();

			return encrypt;
		}

		/// <summary>
		/// AES解密
		/// </summary>
		/// <param name="encryptStr">密文字符串</param>
		/// <returns>明文</returns>
		public static string AESDecrypt(string encryptStr)
		{
			byte[] bKey = Encoding.UTF8.GetBytes(aesKey);
			byte[] bIV = Encoding.UTF8.GetBytes(aesIV);
			byte[] byteArray = Convert.FromBase64String(encryptStr);

			string decrypt = null;
			Rijndael aes = Rijndael.Create();
			try
			{
				using (MemoryStream mStream = new MemoryStream())
				{
					using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
					{
						cStream.Write(byteArray, 0, byteArray.Length);
						cStream.FlushFinalBlock();
						decrypt = Encoding.UTF8.GetString(mStream.ToArray());
					}
				}
			}
			catch { }
			aes.Clear();

			return decrypt;
		}
	}

	internal static class KEY
	{
		public readonly static byte[] passKEY =
			new byte[]
			{
				0xda, 0xef, 0xe3, 0x16, 0x1f, 0x35, 120, 0xe2,
				0xdf, 0xdf, 0xab, 210, 180, 0x9e, 0x43, 0x56,
				0x7a, 0x27, 0xee, 0x5f, 0x62, 0x8a, 0x42, 0x9f
			};

		public readonly static byte[] passIV =
			new byte[]
			{
				1, 2, 3, 4, 5, 6, 7, 8
			};

		public readonly static byte[] cookieKEY =
			new byte[]
			{
				0x7a, 0xef, 0xe3, 0x16, 0x1f, 0x35, 120, 0xd8,
				0xdf, 0xdf, 0xab, 210, 160, 0x9e, 0x3a, 0x56,
				0x7a, 0x27, 0xee, 0x5f, 12, 0x8a, 0x42, 0x9b
			};

		public readonly static byte[] cookieIV =
			new byte[]
			{
				1, 2, 3, 4, 5, 6, 7, 8
			};
	}
}
