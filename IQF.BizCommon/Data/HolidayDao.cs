using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework;
using IQF.Framework.Cache;
using System;
using System.Collections.Generic;

namespace IQF.BizCommon.Data
{
	/// <summary>
	/// 假期
	/// </summary>
	public static class HolidayDao
	{
		private readonly static ICacheInterceptor holidayCache = CacheInterceptorFactory.Create(GetHolidayDict, 24 * 60 * 60);

		/// <summary>
		/// 获取假期名称
		/// 非假期返回NULL
		/// </summary>
		/// <param name="date"></param>
		/// <param name="isNight">是否夜盘  true：夜盘  false：白天</param>
		/// <returns></returns>
		public static string GetHolidayName(DateTime date, bool isNight = false)
		{
			var holidays = GetCacheHolidays();
			if (holidays == null)
			{
				return null;
			}
			var type = isNight ? 1 : 0;

			if (!holidays.ContainsKey(date.ToDate()))
			{
				return null;
			}
			var item = holidays[date.ToDate()];
			if (item.Type != type)
			{
				return null;
			}
			return item.HolidayName;
		}

		/// <summary>
		/// 是否节假日
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static bool IsHoliday(Date date)
		{
			var holidays = GetCacheHolidays();
			if (holidays == null)
			{
				return false;
			}
			if (holidays.ContainsKey(date) && holidays[date].Type == 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 是否由于节假日导致的晚上21点不开盘
		/// </summary>
		/// <param name="natureDate">自然日，比如9月30日是没有夜盘交易的</param>
		/// <returns></returns>
		public static bool IsHolidayNight(Date natureDate)
		{
			var holidays = GetCacheHolidays();
			if (holidays == null)
			{
				return false;
			}

			if (holidays.ContainsKey(natureDate) && holidays[natureDate].Type == 1)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// 获取法定节假日，以及法定节假日引起的上个交易日夜盘休市
		/// </summary>
		/// <returns></returns>
		private static Dictionary<Date, HolidaysDateEntity> GetCacheHolidays()
		{
			return holidayCache.Execute(new Dictionary<Date, HolidaysDateEntity>());
		}

		private static Dictionary<Date, HolidaysDateEntity> GetHolidayDict()
		{
			var holidays = GetDbHolidays();
			if (holidays == null || holidays.Count <= 0)
			{
				return null;
			}

			var ret = new Dictionary<Date, HolidaysDateEntity>();
			foreach (var item in holidays)
			{
				ret.Add(item.Date.ToDate(), item);
			}

			return ret;
		}

		/// <summary>
		/// 获取完整的Holiday 数据
		/// </summary>
		/// <returns></returns>
		public static List<HolidaysDate> GetHolidaysDate()
		{
			var result = GetCacheHolidays();
			var list = new List<HolidaysDate>();
			foreach (var item in result.Values)
			{
				list.Add(new HolidaysDate { Date = item.Date, HolidayName = item.HolidayName, Type = item.Type });
			}
			return list;
		}

		private static List<HolidaysDateEntity> GetDbHolidays()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<HolidaysDateEntity>("select * from HolidaysDate").AsList();
			}
		}
	}

	public class HolidaysDate
	{
		/// <summary>
		/// 
		/// </summary>
		public DateTime Date { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string HolidayName { get; set; }
		/// <summary>
		/// 0:正常节假日，1:夜盘休市
		/// </summary>
		public int Type { get; set; }
	}
}
