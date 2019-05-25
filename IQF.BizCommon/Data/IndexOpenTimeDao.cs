using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Data
{
	public class IndexOpenTimeDao
	{
		private static readonly ICacheInterceptor IndexOpenTimeCache = CacheInterceptorFactory.Create(GetDbIndexOpenTime, 2 * 60 * 60);

		private const int NightBeginTime = 210000;

		public static List<TradeTimeRange> GetTimeRanges(long contractId)
		{
			var openTimes = Get(contractId);
			var allTradeTimeRange = new List<TradeTimeRange>();
			foreach (var item in openTimes)
			{
				var range = new TradeTimeRange();
				range.BeginTime = Convert.ToInt32(item.BeginTime.Replace(":", ""));
				range.EndTime = Convert.ToInt32(item.EndTime.Replace(":", ""));
				allTradeTimeRange.Add(range);
			}
			var result = new List<TradeTimeRange>();
			var list = allTradeTimeRange.Where(w => w.BeginTime >= 210000).OrderBy(o => o.BeginTime);
			result.AddRange(list);
			list = allTradeTimeRange.Where(w => w.BeginTime < 210000).OrderBy(o => o.BeginTime);
			result.AddRange(list);
			return allTradeTimeRange;
		}

		public static bool IsNight(long contractId)
		{
			var timeRanges = GetTimeRanges(contractId);
			if (timeRanges == null)
			{
				return false;
			}
			return timeRanges.Any(p => p.BeginTime >= NightBeginTime);
		}

		public static int GetOpenTime(long contractId)
		{
			var timeRanges = GetTimeRanges(contractId);
			if (timeRanges == null)
			{
				return 90000;
			}
			if (IsNight(contractId))
			{
				return timeRanges.Where(w => w.BeginTime >= NightBeginTime).OrderBy(o => o.BeginTime).Select(s => s.BeginTime).First();
			}
			return timeRanges.Where(w => w.BeginTime < NightBeginTime).OrderBy(o => o.BeginTime).Select(s => s.BeginTime).First();
		}

		public int GetEndTime(long contractId)
		{
			var timeRanges = GetTimeRanges(contractId);
			if (timeRanges == null)
			{
				return 150000;
			}

			var tradeTimeRange = timeRanges.LastOrDefault();
			if (tradeTimeRange != null) return tradeTimeRange.EndTime;
			return 150000;
		}

		private static List<IndexOpenTimeEntity> Get(long contractId)
		{
			var allData = GetAll();
			if (allData == null) return null;
			var result = allData.Where(p => p.ContractID == contractId).ToList();
			return result;
		}

		private static List<IndexOpenTimeEntity> GetAll()
		{
			return IndexOpenTimeCache.Execute<List<IndexOpenTimeEntity>>();
		}

		private static List<IndexOpenTimeEntity> GetDbIndexOpenTime()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<IndexOpenTimeEntity>("select * from IndexOpenTime").ToList();

			}
		}
	}
}