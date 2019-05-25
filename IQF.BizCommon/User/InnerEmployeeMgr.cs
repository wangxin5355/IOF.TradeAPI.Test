using IQF.Framework;
using IQF.Framework.Cache;
using IQF.Framework.Serialization;
using IQF.Framework.Util;
using System;
using System.Collections.Generic;

namespace IQF.BizCommon.User
{
	public static class InnerEmployeeMgr
	{
		private readonly static ICacheInterceptor InnerEmployeeCache = CacheInterceptorFactory.Create(GetAll, 10 * 60);

		/// <summary>
		/// 是否公司内部员工
		/// </summary>
		/// <param name="mobile"></param>
		/// <returns></returns>
		public static bool IsInnerEmployee(string mobile)
		{
			if (string.IsNullOrWhiteSpace(mobile))
			{
				return false;
			}

			try
			{
				var all = InnerEmployeeCache.Execute<List<string>>();
				if (all == null)
				{
					return false;
				}

				return all.Contains(mobile);
			}
			catch (Exception ex)
			{
				LogRecord.writeLogsingle("InnerEmployeeError.log", ex.ToString());
				return false;
			}
		}

		private static List<string> GetAll()
		{
			var resp = HttpWebResponseUtility.HttpGet("http://inapi.inquant.cn/appmanager/InnerUser/getall");
			var ret = JsonHelper.Deserialize<ResultInfo<List<string>>>(resp);
			if (ret.IsError())
			{
				return null;
			}
			return ret.Data;
		}
	}
}
