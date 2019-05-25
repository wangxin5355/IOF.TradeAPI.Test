using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework;
using IQF.Framework.Cache;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Data
{
	/// <summary>
	/// 交易时间
	/// </summary>
	public class VarietyOpenTimeDao
	{
		private static readonly ICacheInterceptor VariOpenTimesCache = CacheInterceptorFactory.Create(GetDbVarietyOpenTimes);

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns>
		/// key:品种编号
		/// value：字典[beginTime, endTime]
		/// </returns>
		public static Dictionary<string, string> GetBy(long variID)
		{
			var all = GetCacheVariOpenTimes();
			if (all == null || all.Count <= 0 || !all.ContainsKey(variID))
			{
				return new Dictionary<string, string>();
			}
			return all[variID].ToDictionary(k => k.BeginTime, v => v.EndTime);
		}

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns>
		/// key:品种编码
		/// value：字典[beginTime, endTime]
		/// </returns>
		public static Dictionary<string, string> GetBy(string variCode)
		{
			var variID = VarietyDao.GetVarietyID(variCode);
			if (variID <= 0)
			{
				return new Dictionary<string, string>();
			}
			return GetBy(variID);
		}

		/// <summary>
		/// 从缓存中获取开盘时间
		/// </summary>
		/// <returns></returns>
		private static Dictionary<long, List<VarietyOpenTimeEntity>> GetCacheVariOpenTimes()
		{
			return VariOpenTimesCache.Execute<Dictionary<long, List<VarietyOpenTimeEntity>>>();
		}

		/// <summary>
		/// 获取所有品种信息 开盘时间
		/// </summary>
		/// <returns></returns>
		private static Dictionary<long, List<VarietyOpenTimeEntity>> GetDbVarietyOpenTimes()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				var all = conn.Query<VarietyOpenTimeEntity>("select * from VarietyOpenTime;").AsList();

				return all.GroupBy(g => g.VID).ToDictionary(k => k.Key, v => v.ToList());
			}
		}
	}
}
