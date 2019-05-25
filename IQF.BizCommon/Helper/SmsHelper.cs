using IQF.BizCommon.User;
using IQF.Framework;
using IQF.Framework.Serialization;
using IQF.Framework.Util;
using System;

namespace IQF.BizCommon.Helper
{
	/// <summary>
	/// 短信通道Api
	/// </summary>
	public static class SmsHelper
	{
		private static string BaseUrl = ConfigManager.GetAppSetting("fundationApiDomain", null);

		/// <summary>
		/// 获取验证码
		/// </summary>
		/// <param name="mobile">手机号</param>
		/// <param name="verifyCodeType">验证码类型</param>
		/// <returns></returns>
		public static ResultInfo<string> GetVerifyCode(string mobile, VerifyCodeType verifyCodeType)
		{
			if (string.IsNullOrEmpty(BaseUrl))
			{
				throw new ApplicationException("fundationApiDomain配置不能为空");
			}
			if (string.IsNullOrWhiteSpace(mobile))
			{
				return new ResultInfo<string>(-1, "手机号不能为空");
			}

			var url = string.Format("http://{0}/fundationapi/sendsms/GetVerifyCode?mobile={1}&verifyCodeType={2}", BaseUrl, mobile, (int)verifyCodeType);
			var resp = HttpWebResponseUtility.HttpGet(url);

			var jsonString = new JsonString(resp);

			var result = new ResultInfo<string>();
			result.Error_no = jsonString.GetInt("error_no");
			result.Error_info = jsonString.Get("error_info").SafeToString();
			result.Data = jsonString.Get("data").SafeToString();

			return result;
		}

		/// <summary>
		/// 发送验证码
		/// 根据不同的包类型使用不同的短信签名
		/// </summary>
		/// <param name="verifyCodeType"></param>
		/// <param name="mobile"></param>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static ResultInfo SendVerifyCode(VerifyCodeType verifyCodeType, string mobile, int packType, string ip)
		{
			if (string.IsNullOrEmpty(BaseUrl))
			{
				throw new ApplicationException("fundationApiDomain配置不能为空");
			}
			if (string.IsNullOrWhiteSpace(mobile))
			{
				return new ResultInfo(-1, "手机号不能为空");
			}

			var js = new JsonString();
			js.Set("verifyCodeType", (int)verifyCodeType);
			js.Set("mobile", mobile);
			js.Set("packType", packType);
			js.Set("ip", ip);

			var url = string.Format("http://{0}/fundationapi/sendsms/sendverifycode", BaseUrl);
			var resp = HttpWebResponseUtility.HttpPost(url, js.ToString());

			var jsonString = new JsonString(resp);

			var result = new ResultInfo<string>();
			result.Error_no = jsonString.GetInt("error_no");
			result.Error_info = jsonString.Get("error_info").SafeToString();
			result.Data = jsonString.Get("data").SafeToString();

			return result;
		}

		/// <summary>
		/// 七指禅策略短信通知
		/// </summary>
		/// <param name="room"></param>
		/// <param name="time"></param>
		/// <param name="mobiles">一次最多50个，逗号分隔，尾部没有逗号</param>
		/// <returns></returns>
		public static ResultInfo SendQzcStrategySms(string room, string time, string mobiles)
		{
			if (string.IsNullOrEmpty(BaseUrl))
			{
				throw new ApplicationException("fundationApiDomain配置不能为空");
			}
			if (string.IsNullOrWhiteSpace(mobiles))
			{
				return new ResultInfo(-1, "手机号不能为空");
			}

			var js = new JsonString();
			js.Set("room", room);
			js.Set("time", time);
			js.Set("mobiles", mobiles);

			var url = string.Format("http://{0}/fundationapi/sendsms/sendqzcstrategysms", BaseUrl);
			var resp = HttpWebResponseUtility.HttpPost(url, js.ToString());

			var jsonString = new JsonString(resp);

			var result = new ResultInfo<string>();
			result.Error_no = jsonString.GetInt("error_no");
			result.Error_info = jsonString.Get("error_info").SafeToString();

			return result;
		}
	}
}
