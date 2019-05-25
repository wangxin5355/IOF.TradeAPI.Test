using IQF.Framework;
using IQF.Framework.Encrypt;
using System;
using System.Text;

namespace IQF.BizCommon.User
{
	public class UserTokenMgr
	{
		/// <summary>
		/// 分割串儿。
		/// </summary>
		public readonly static char Separator = (char)0x1EEC;

		/// <summary>
		/// 用户对象，生成令牌
		/// </summary>
		/// <param name="user">用户对象</param>
		/// <param name="UserToken">令牌</param>
		/// <returns>true/false</returns>
		public static string GenUserToken(UserTokenModel user)
		{
			if (user == null || user.UserID <= 0)
			{
				return null;
			}
			StringBuilder sb = new StringBuilder();
			sb.Append(user.UserID);
			sb.Append(Separator);

			sb.Append(user.Mobile ?? string.Empty);

			string strUserToken = Cryptogram.EncryptUserToken(sb.ToString());
			strUserToken = strUserToken.Replace("+", "-").Replace("/", "_").Replace("=", "*");
			return System.Web.HttpUtility.UrlEncode(strUserToken, System.Text.Encoding.UTF8);
		}

		/// <summary>
		/// 验证令牌，生成用户
		/// </summary>
		/// <param name="userToken">令牌</param>
		/// <param name="user">返回用户对象</param>
		/// <returns>true/false</returns>
		public static bool ValidateUserToken(ref string userToken, out UserTokenModel user)
		{
			if (string.IsNullOrWhiteSpace(userToken))
			{
				user = null;
				return false;
			}
			try
			{
				userToken = HandleOrginalToken(userToken);
				if (userToken == null)
				{
					user = null;
					return false;
				}
				var arr = userToken.Split(Separator);
				if (arr.Length == 2)
				{
					var userId = arr[0].ToLong();
					string mobile = arr[1] ?? string.Empty;

					user = new UserTokenModel() { UserID = userId, Mobile = mobile };

					return true;
				}
				LogRecord.writeLogsingle("ValidateUserTokenError.log", "Token格式不正确:" + userToken);
			}
			catch (Exception ex)
			{
				LogRecord.writeLogsingle("ValidateUserTokenError.log", ex.ToString());
			}
			user = null;
			return false;
		}

		/// <summary>
		/// 处理原始令牌
		/// </summary>
		/// <param name="orginalToken">原始令牌</param>
		/// <returns>处理过的令牌</returns>
		private static string HandleOrginalToken(string orginalToken)
		{
			if (string.IsNullOrWhiteSpace(orginalToken))
			{
				return null;
			}
			//如果app传过来的值没有编码：1、带“/”会被截断导致出错，2、“+”会urldecode为空格，空格补“+”
			var token = orginalToken.Replace(" ", "+");//补+的问题
													   //处理app传过来的值，有可能被多次encoder的情况 
			for (int index = 0; index < 5; index++)
			{
				if (token.IndexOf("%") < 0)
				{
					break;
				}
				token = System.Web.HttpUtility.UrlDecode(token, System.Text.Encoding.UTF8);
				if (token.IndexOf("%") < 0)
				{
					break;
				}
			}
			token = token.Replace("-", "+").Replace("_", "/").Replace("*", "=");
			return Cryptogram.DecryptUserToken(token);
		}
	}

	/// <summary>
	/// 用户令牌
	/// </summary>
	public class UserTokenModel
	{
		/// <summary>
		/// 用户编号
		/// </summary>
		public long UserID { get; set; }

		/// <summary>
		/// 用户手机号
		/// </summary>
		public string Mobile { get; set; }
	}
}
