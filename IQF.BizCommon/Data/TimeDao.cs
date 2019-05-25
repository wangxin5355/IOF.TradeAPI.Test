using IQF.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Data
{
	public class TimeDao
	{
		#region 整个盘口时间相关方法
		/// <summary>
		/// 收盘时间
		/// </summary>
		public static Dictionary<TimeSpan, TimeSpan> CloseTime = new Dictionary<TimeSpan, TimeSpan>();
		private const int NightBeginTime = 210000;
		static TimeDao()
		{
			//开盘时间
			//09:00:00 - 11:30:00
			//13:00:00 - 15:15:00
			//21:00:00 - 02:30:00
			CloseTime.Add(new TimeSpan(2, 30, 0), new TimeSpan(9, 0, 0));
			CloseTime.Add(new TimeSpan(11, 30, 0), new TimeSpan(13, 0, 0));
			CloseTime.Add(new TimeSpan(15, 15, 0), new TimeSpan(21, 0, 0));
		}
		/// <summary>
		/// 是否是开盘时间
		/// </summary>
		/// <returns></returns>
		public static bool IsOpenTime()
		{
			return IsOpenTime(DateTime.Now);
		}
		/// <summary>
		/// 获取下次开盘时间
		/// </summary>
		/// <returns></returns>
		public static DateTime GetNextOpen()
		{
			return GetNextOpen(DateTime.Now);
		}
		/// <summary>
		/// 获取下次收盘时间
		/// </summary>
		/// <returns></returns>
		public static DateTime GetNextClose()
		{
			return GetNextClose(DateTime.Now);
		}
		/// <summary>
		/// 是否是开盘时间
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static bool IsOpenTime(DateTime dt)
		{
			//周六凌晨
			var hhmm = dt.GetHHMM();
			if (dt.DayOfWeek == DayOfWeek.Saturday)
			{
				if (hhmm > 230)
				{
					return false;
				}
				else
				{
					var ret = HolidayDao.IsHolidayNight(dt.AddDays(-1).ToDate());
					return !ret;
				}
			}
			if (!IsTradeDay(dt.ToDate()))
			{
				return false;
			}
			if (dt.DayOfWeek == DayOfWeek.Monday && hhmm < 900)
			{
				return false;
			}
			if (hhmm >= 2100)
			{
				var ret = HolidayDao.IsHolidayNight(dt.ToDate());
				return !ret;
			}
			var t = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
			foreach (KeyValuePair<TimeSpan, TimeSpan> item in CloseTime)
			{
				if (t > item.Key && t < item.Value)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 下次开盘时间
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DateTime GetNextOpen(DateTime dt)
		{
			DateTime result = dt;
			List<TimeSpan> tData = CloseTime.Values.ToList();
			tData.Sort();
			TimeSpan t = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
			foreach (TimeSpan item in tData)
			{
				result = result.Date + item;
				if (result > dt)
				{
					return result;
				}
			}
			result = GetNextTradeDate(dt.ToDate());

			return result.Date + tData.Min();
		}
		/// <summary>
		/// 下次收盘时间
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DateTime GetNextClose(DateTime dt)
		{
			DateTime result = dt;
			List<TimeSpan> tData = CloseTime.Keys.ToList();
			tData.Sort();
			TimeSpan t = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
			foreach (TimeSpan item in tData)
			{
				result = result.Date + item;
				if (result > dt)
				{
					return result;
				}
			}
			result = GetNextTradeDate(dt.ToDate());
			if (false == IsTradeDay(result.ToDate().AddDays(-1)))
			{
				tData.Remove(tData.Min());
			}
			return result.Date + tData.Min();
		}
		#endregion

		/// <summary>
		/// 获取dateTime对应的交易日。返回日期date
		/// 默认用2100作为每个交易日的起始时间
		/// </summary>
		/// <param name="dateTime">时间</param>
		/// <param name="tradeStartTime">交易开始时间，默认已21点做分割</param>
		/// <returns></returns>
		public static Date GetTradeDate(DateTime dateTime, int tradeStartTime = 2100)
		{
			if (!IsTradeDay(dateTime.ToDate()))
			{
				return GetTradeDate(dateTime.ToDate());
			}
			var hhmm = dateTime.GetHHMM();
			if (hhmm >= tradeStartTime)
			{
				dateTime = dateTime.AddDays(1);
				return GetTradeDate(dateTime.ToDate());
			}
			return dateTime.ToDate();
		}

		private static Date GetTradeDate(Date date)
		{
			for (int i = 0; i < 365; i++)
			{
				if (IsTradeDay(date))
				{
					return date;
				}
				date = date.AddDays(1);
			}
			return date;
		}

		/// <summary>
		/// 判断交易日期，有日盘就被看作是交易日。注意周六不是trading day，但周六凌晨是有交易的
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static bool IsTradeDay(Date date)
		{
			if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
			{
				return false;
			}
			return !HolidayDao.IsHoliday(date);
		}
		/// <summary>
		/// 根据合约和当前时间 判断下个间隔时间
		/// </summary>
		/// <param name="time">当前时间</param>
		/// <param name="contractId">合约ID</param>
		/// <param name="minute">间隔分钟</param>
		/// <returns></returns>
		public static DateTime GetNextTime(DateTime time, long contractId, int minute)
		{
			var contract = AllContractDao.Get(contractId);
			if (contract == null) return DateTime.MinValue;
			if (!AllContractDao.IsIndustryIndexContract(contractId))
			{
				return VarietyDao.GetNextTime(time, contract.VarietyID, minute);
			}
			var tradeTimeRange = IndexOpenTimeDao.GetTimeRanges(contractId);
			return VarietyDao.GetNextTimeByContractId(time, tradeTimeRange, contractId, minute);
		}

		/// <summary>
		/// 是否交易时间
		/// </summary>
		/// <param name="time"></param>
		/// <param name="contractID"></param>
		/// <param name="defaultValue">出错时的默认值</param>
		/// <returns></returns>
		public static bool IsTradingTimeBy(DateTime time, long contractID, bool defaultValue = true)
		{
			var contract = ContractDao.Get(contractID);
			if (contract == null)
			{
				return defaultValue;
			}
			return IsTradingTime(time, contract.VarietyID, defaultValue);
		}

		/// <summary>
		/// 是否交易时间
		/// </summary>
		/// <param name="time"></param>
		/// <param name="symbol"></param>
		/// <param name="defaultValue">出错时的默认值</param>
		/// <returns></returns>
		public static bool IsTradingTime(DateTime time, string symbol, bool defaultValue = true)
		{
			var contractID = ContractDao.GetContractID(symbol);
			if (contractID == -1)
			{
				return defaultValue;
			}
			return IsTradingTimeBy(time, contractID, defaultValue);
		}

		/// <summary>
		/// 是否交易时间
		/// </summary>
		/// <param name="time"></param>
		/// <param name="vid">品种编号</param>
		/// <param name="defaultValue">出错时的默认值</param>
		/// <returns></returns>
		public static bool IsTradingTime(DateTime time, long vid, bool defaultValue = true)
		{
			var vari = VarietyDao.Get(vid);
			if (vari == null)
			{
				return defaultValue;
			}
			return IsTradingTime(time, vari.TradeTimeList, defaultValue);
		}

		/// <summary>
		///  根据合约ID 判断是否交易时间
		/// </summary>
		/// <param name="time"></param>
		/// <param name="contractId"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool IsTradingTimeByContractId(DateTime time, long contractId, bool defaultValue = true)
		{
			var contract = AllContractDao.Get(contractId);
			if (contract == null) return defaultValue;
			if (!AllContractDao.IsIndustryIndexContract(contractId))
			{
				return IsTradingTime(time, contract.VarietyID, defaultValue);
			}
			var tradeTimeRange = IndexOpenTimeDao.GetTimeRanges(contractId);
			return IsTradingTime(time, tradeTimeRange, defaultValue);
		}

		public static bool IsNight(long contractId)
		{
			if (!AllContractDao.IsIndustryIndexContract(contractId))
			{
				var variety = VarietyDao.Get(contractId);
				if (variety == null) return false;

				return variety.IsNight();
			}

			return IndexOpenTimeDao.IsNight(contractId);
		}

		/// <summary>
		/// 根据开收盘时间判断是否交易时间 
		/// 特别说明：已经在行情里使用，谨慎修改
		/// 作者:jincheng
		/// 修改时间:2017年8月11日 14:19:15
		/// </summary>
		/// <param name="time"></param>
		/// <param name="symbol"></param>
		/// <param name="defaultValue">出错时的默认值</param>
		/// <returns></returns>
		private static bool IsTradingTime(DateTime time, List<TradeTimeRange> timeRangeList, bool defaultValue = true)
		{
			var nowHhmmss = time.GetHHMMSS();
			var nightTradeTimeRange = timeRangeList.FirstOrDefault(p => p.BeginTime >= NightBeginTime);
			if (time.DayOfWeek == DayOfWeek.Saturday)
			{
				if (nightTradeTimeRange == null) return false;
				if (nowHhmmss > nightTradeTimeRange.EndTime) return false;
			}

			if (time.DayOfWeek == DayOfWeek.Sunday)
			{
				return false;
			}
			if (HolidayDao.IsHoliday(time.ToDate()))
			{
				return false;
			}
			if (nowHhmmss >= 210000)
			{
				var isHolidayNight = HolidayDao.IsHolidayNight(time.ToDate());
				if (isHolidayNight)
				{
					return false;
				}
			}

			if (nowHhmmss >= 0 && nowHhmmss <= 30000) //节假日前一夜及第二天凌晨 无交易。节假日前夜，且第二天非法定节假日如：2017.09.30
			{
				var isHolidayNight = HolidayDao.IsHolidayNight(time.AddDays(-1).ToDate());
				if (isHolidayNight)
				{
					return false;
				}
			}
			if (timeRangeList == null || timeRangeList.Count <= 0)
			{
				return defaultValue;
			}

			var hhmmss = time.GetHHMMSS();
			//星期6需要特殊处理
			if (time.DayOfWeek == DayOfWeek.Saturday)
			{
				if (hhmmss >= 210000) return false;

				return timeRangeList.Exists(
					e =>
						e.BeginTime >= NightBeginTime &&
						e.EndTime < e.BeginTime && (hhmmss <= e.EndTime || hhmmss >= e.BeginTime));

			}
			if (time.DayOfWeek == DayOfWeek.Monday)
			{
				var startTime = timeRangeList.Where(e => e.BeginTime < NightBeginTime).OrderBy(p => p.BeginTime).FirstOrDefault();
				if (startTime != null)
				{
					if (hhmmss < startTime.BeginTime) return false;
				}
			}

			var isDay = timeRangeList.Exists(e => e.BeginTime < NightBeginTime && (e.EndTime > e.BeginTime && hhmmss >= e.BeginTime && hhmmss <= e.EndTime)); //白天
			if (isDay)
			{
				return true;
			}
			var isNight = timeRangeList.Exists(e => e.BeginTime >= NightBeginTime && ((e.EndTime < e.BeginTime && (hhmmss <= e.EndTime || hhmmss >= e.BeginTime))
				|| (e.EndTime > e.BeginTime && hhmmss >= e.BeginTime && hhmmss <= e.EndTime)));//夜盘
			if (isNight)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 开盘时间
		/// </summary>
		/// <param name="time"></param>
		/// <param name="contractId"></param>
		/// <returns></returns>
		public static DateTime GetTradingBeginTimeByContractId(DateTime time, long contractId)
		{
			var contract = AllContractDao.Get(contractId);

			if (!AllContractDao.IsIndustryIndexContract(contractId))
			{
				return GetTradingBeginTime(time, contract.VarietyID);
			}

			var indexOpenTime = IndexOpenTimeDao.GetOpenTime(contractId);
			bool isHasNight = IndexOpenTimeDao.IsNight(contractId);
			return GetBeginTime(time, indexOpenTime, isHasNight);
		}

		/// <summary>
		/// 获取交易开始时间
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static DateTime GetTradingBeginTime(DateTime time, long vid)
		{
			var vari = VarietyDao.Get(vid);
			if (vari == null)
			{
				return time.Date.AddHours(-9);
			}
			bool isHasNight = vari.IsNight();
			var openTime = vari.GetOpenTime();
			return GetBeginTime(time, openTime, isHasNight);
		}

		private static DateTime GetBeginTime(DateTime time, int openTime, bool isHasNight)
		{
			time = time.AddDays(1);
			for (int i = 0; i < 365; i++)
			{
				time = time.AddDays(-1);
				if (time.DayOfWeek == DayOfWeek.Saturday
					|| time.DayOfWeek == DayOfWeek.Sunday
				) continue; // 周末 跳过
				if (isHasNight)
				{
					if (i == 0 && time.GetHHMMSS() < openTime)
					{
						continue;
					}
					else
					{
						time = time.Date.AddHours(openTime / 10000);
						time = time.AddMinutes(openTime / 100 % 100);
						time = time.AddSeconds(openTime % 100);
						return time;
					}
				}
				else
				{
					time = time.Date.AddHours(openTime / 10000);
					time = time.AddMinutes(openTime / 100 % 100);
					time = time.AddSeconds(openTime % 100);
					return time;

				}
			}
			return time.Date.AddHours(9);
		}

		/// <summary>
		/// 获取dt的交易时间段
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="datas"></param>
		/// <returns></returns>
		private static Dictionary<DateTime, DateTime> getTradeTime(DateTime dt, Dictionary<string, string> datas)
		{
			Dictionary<DateTime, DateTime> result = null;
			if (datas != null && datas.Count > 0)
			{
				result = new Dictionary<DateTime, DateTime>();
				foreach (KeyValuePair<string, string> item in datas)
				{
					DateTime begin = string.Format("{0} {1}", dt.ToString("yyyy-MM-dd"), item.Key.ToString()).ToDateTime(DateTime.MinValue, "yyyy-MM-dd HH:mm:ss");
					DateTime end = string.Format("{0} {1}", dt.ToString("yyyy-MM-dd"), item.Value.ToString()).ToDateTime(DateTime.MinValue, "yyyy-MM-dd HH:mm:ss");
					if (begin > end)
					{
						end = end.AddDays(1);
					}
					if (false == result.ContainsKey(begin))
					{
						result.Add(begin, end);
					}
					else
					{
						result[begin] = end;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 获取某日期 + nextDays个交易日的日期
		/// </summary>
		/// <param name="date">某日期</param>
		/// <param name="nextDays">交易日天数</param>
		/// <returns>日期时间</returns>
		public static Date GetNextTradeDate(Date date, int nextDays = 1)
		{
			var result = date;
			int index = 0;
			while (index < nextDays)
			{
				result = result.AddDays(1);
				if (true == IsTradeDay(result))
				{
					index++;
				}
			}
			return result;
		}

		/// <summary>
		/// 获取某日期 - preDays个交易日的日期时间
		/// </summary>
		/// <param name="today">某日期</param>
		/// <param name="preDays">交易日天数</param>
		/// <returns>日期时间</returns>
		public static Date GetPreTradeDate(Date today, int preDays = 1)
		{
			var result = today;
			int index = 0;
			while (index < preDays)
			{
				result = result.AddDays(-1);
				if (true == IsTradeDay(result))
				{
					index++;
				}
			}
			return result;
		}

		/// <summary>
		/// 获取dt时间（dt是非交易日则取下一个交易日）的交易失效时间
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="vID"></param>
		/// <returns></returns>
		public static DateTime GetValidTime(DateTime dt, long vID)
		{
			DateTime result = DateTime.MinValue;
			if (false == IsTradeDay(dt.ToDate()))
			{
				dt = GetNextTradeDate(dt.ToDate());
			}
			Dictionary<string, string> datas = VarietyOpenTimeDao.GetBy(vID);
			Dictionary<DateTime, DateTime> timeDatas = getTradeTime(dt, datas);
			timeDatas = timeDatas.Where(i => i.Value.Hour == 15).ToDictionary(d => d.Key, v => v.Value);
			if (timeDatas != null && timeDatas.Count > 0)
			{
				result = timeDatas.Values.ToList().Max();
				if (result < dt)
				{
					result = string.Format("{0} {1}", GetNextTradeDate(result.ToDate()).ToString("yyyy-MM-dd"), result.ToString("HH:mm:ss")).ToDateTime(DateTime.MinValue, "yyyy-MM-dd HH:mm:ss"); ;
				}
			}
			return result;
		}

		/// <summary>
		/// 获得某品种下一次开盘时间。
		/// </summary>
		/// <returns></returns>
		public static DateTime GetNextTradeOpenDateTime(long vID)
		{
			DateTime result = DateTime.Now;
			return GetNextTradeOpenDateTime(result, vID);
		}
		/// <summary>
		/// 获得某品种下一次开盘时间。
		/// </summary>
		/// <param name="dt">时间</param>
		/// <param name="vID">品种编号</param>
		/// <returns></returns>
		public static DateTime GetNextTradeOpenDateTime(DateTime dt, long vID)
		{
			bool isTradeDay = IsTradeDay(dt.ToDate());
			if (false == isTradeDay)
			{
				dt = GetNextTradeDate(dt.ToDate());
			}
			var datas = VarietyOpenTimeDao.GetBy(vID);
			var timeDatas = getTradeTime(dt, datas);
			List<DateTime> beginDatas = timeDatas.Keys.ToList();
			return beginDatas.FindAll(i => i >= dt).Min();
		}

	}//End class TimeDao
}//End namespace IQF.BizCommon.Data
