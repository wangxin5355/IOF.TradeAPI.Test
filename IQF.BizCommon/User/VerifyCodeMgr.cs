using IQF.BizCommon.User.Entity;
using IQF.Utilities;
using IQF.Utilities.Encrypt;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace IQF.BizCommon.User
{
	/// <summary>
	/// 验证码管理
	/// </summary>
	public static class VerifyCodeMgr
	{
		/// <summary>
		/// 获取有效验证码（最近五分钟）
		/// </summary>
		/// <param name="mobile">手机号</param>
		/// <returns></returns>
		public static VerifyCodeInfoEntity GetValid(string mobile, VerifyCodeType codeType)
		{
			if (string.IsNullOrWhiteSpace(mobile))
			{
				return null;
			}
			var sql = "select top 1 * from dbo.VerifyCodeInfo t where t.mobile = @mobile and t.VerifyCodeType = @codeType and t.[status] = 1 and t.AddTime between @startTime and @endTime order by t.AddTime DESC;";
			using (var conn = new SqlConnection(ConnectionString.DB_IQFUser))
			{
				var enMobile = Cryptogram.EncryptPassword(mobile);
				var startTime = DateTime.Now.AddSeconds(-300);
				var entity = conn.Query<VerifyCodeInfoEntity>(sql, new { mobile = enMobile, codeType = codeType, startTime = startTime, endTime = DateTime.Now }).FirstOrDefault();
				return entity;
			}
		}

		/// <summary>
		/// 保存验证码
		/// </summary>
		public static bool Insert(VerifyCodeType verifyCodeType, string mobile, string verifyCode, int packType, int status, string comment)
		{
			if (string.IsNullOrWhiteSpace(mobile) || string.IsNullOrWhiteSpace(verifyCode))
			{
				return false;
			}
			var sql = "INSERT INTO [dbo].[VerifyCodeInfo]([Mobile],[VerifyCode],[VerifyCodeType],[Status],[Comment],[PackType]) VALUES(@Mobile,@VerifyCode,@VerifyCodeType,@Status,@Comment,@PackType);";
			using (var conn = new SqlConnection(ConnectionString.DB_IQFUser))
			{
				var count = conn.Execute(sql, new { verifyCodeType = verifyCodeType, mobile = Cryptogram.EncryptPassword(mobile), verifyCode = verifyCode, packType = packType, status = status, comment });
				return count > 0;
			}
		}
	}

	/// <summary>
	/// 验证码类型
	/// </summary>
	public enum VerifyCodeType
	{
		/// <summary>
		/// 登录发送验证码
		/// </summary>
		Login = 1,

		/// <summary>
		/// 重设密码发送验证码
		/// </summary>
		ResetPwd = 2,

		/// <summary>
		/// 补签开户协议 
		/// </summary>
		PatchOpenAcctAgreement = 3,

		/// <summary>
		/// 注册
		/// </summary>
		Register = 4
	}
}
