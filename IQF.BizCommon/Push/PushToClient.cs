using IQF.Framework;
using IQF.Framework.Serialization;
using IQF.Framework.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Push
{
	public class PushToClient
	{
		private static string BaseUrl = ConfigManager.GetAppSetting("fundationApiDomain", null);

		#region 内推 SOCKET推送
		/// <summary>
		/// 内推 SOCKET推送
		/// </summary>
		/// <param name="pushinfo"></param>
		/// <param name="invalidTime"></param>
		public static void InternalPush(InternalPushInfo pushinfo, DateTime invalidTime)
		{
			if (pushinfo == null)
			{
				return;
			}
			var pushInfos = new List<InternalPushInfo>();
			pushInfos.Add(pushinfo);
			InternalPushBatch(pushInfos, invalidTime);

		}
		/// <summary>
		/// 内推 Socket批量推送
		/// </summary>
		/// <param name="infos"></param>
		/// <param name="invalidTime"></param>
		public static void InternalPushBatch(IEnumerable<InternalPushInfo> infos, DateTime invalidTime)
		{
			if (infos == null || infos.Count() <= 0)
			{
				return;
			}
			if (string.IsNullOrEmpty(BaseUrl))
			{
				throw new ApplicationException("fundationApiDomain配置不能为空");
			}
			var js = new JsonString();
			js.Set("pushInfos", infos);
			js.Set("invalidTime", invalidTime);
			var url = string.Format("http://{0}/fundationapi/pushmsg/internalpush", BaseUrl);
			HttpWebResponseUtility.HttpPost(url, js.ToString());
		}
		#endregion

		#region 外推
		/// <summary>
		/// 外部推送带过期时间
		/// </summary>
		/// <param name="info"></param>
		/// <param name="invalidTime"></param>
		public static void ExternalPush(PushInfo pushInfo, DateTime invalidTime)
		{
			if (pushInfo == null)
			{
				return;
			}
			var pushInofs = new List<PushInfo>();
			pushInofs.Add(pushInfo);
			ExternalPushBatch(pushInofs, invalidTime);
		}

		/// <summary>
		/// 批量外部推送带过期时间
		/// </summary>
		/// <param name="info"></param>
		/// <param name="invalidTime"></param>
		public static void ExternalPushBatch(IEnumerable<PushInfo> infos, DateTime invalidTime)
		{
			if (infos == null || infos.Count() <= 0)
			{
				return;
			}
			if (string.IsNullOrEmpty(BaseUrl))
			{
				throw new ApplicationException("fundationApiDomain配置不能为空");
			}
			JsonString js = new JsonString();
			js.Set("pushInfos", infos);
			js.Set("invalidTime", invalidTime);
			var url = string.Format("http://{0}/fundationapi/pushmsg/externalpush", BaseUrl);
			HttpWebResponseUtility.HttpPost(url, js.ToString());
		}
		#endregion

		/// <summary>
		/// 获取外推KEY
		/// </summary>
		/// <returns></returns>
		public static string GetExternalPushKey()
		{
			return "iqf:push_list";
		}

		/// <summary>
		/// 获取内推KEY
		/// </summary>
		/// <param name="pushServerNo">push服务器编号</param>
		/// <returns></returns>
		public static string GetInternalPushKey(int pushServerNo)
		{
			return $"iqf:socket:{pushServerNo}:";
		}

		/// <summary>
		/// socket推送服务器KEY
		/// </summary>
		/// <returns></returns>
		public static string GetPushServerKey()
		{
			return $"iqf:pushserver";
		}
	}
}
