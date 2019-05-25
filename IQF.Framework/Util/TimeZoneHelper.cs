using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IQF.Framework.Util
{
	/// <summary>
	/// 时区相关的
	/// </summary>
	public static class TimeZoneHelper
	{
		/// <summary>
		/// 线程本地缓存的时间数据
		/// </summary>
		[ThreadStatic]
		private static Dictionary<int, DateTime> usTimes;

		/// <summary>
		/// 线程本地缓存的时间数据
		/// </summary>
		[ThreadStatic]
		private static Dictionary<int, DateTime> beijingTimes;

		static TimeZoneHelper()
		{
			var isWin = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
			if (isWin)
			{
				BeijingTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
				USEastTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
			}
			else
			{
				//Iana格式
				BeijingTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai");
				USEastTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
			}
		}

		/// <summary>
		/// 北京时间
		/// </summary>
		public readonly static TimeZoneInfo BeijingTimeZone = null;

		/// <summary>
		/// 美东时间
		/// </summary>
		public readonly static TimeZoneInfo USEastTimeZone = null;

		/// <summary>
		/// Convert datetime to UTC time, as understood by Facebook.
		/// </summary>
		/// <param name="dateToConvert">The date that we need to pass to the api.</param>
		/// <returns>The number of seconds since Jan 1, 1970.</returns>
		public static uint ConvertDateToLong(DateTime dateToConvert)
		{
			double result;
			DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
			TimeSpan span = dateToConvert - utcDate;
			result = span.TotalSeconds;
			return (uint)result;
		}

		/// <summary>
		/// 北京时间 转时间戳
		/// </summary>
		/// <param name="dateToConvert"></param>
		/// <returns></returns>
		public static int ConvertBeijingTimeToInt(DateTime dateToConvert)
		{
			dateToConvert = dateToConvert.ToUniversalTime();
			DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
			TimeSpan span = dateToConvert - utcDate;
			var result = span.TotalSeconds;
			return (int)result;
		}

		/// <summary>
		/// 把UTC时间转为timestamp时间戳
		/// </summary>
		/// <param name="dateToConvert"></param>
		/// <returns></returns>
		public static int ConvertDateToInt(DateTime dateToConvert)
		{
			double result;
			DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
			TimeSpan span = dateToConvert - utcDate;
			result = span.TotalSeconds;
			return (int)result;
		}

		/// <summary>
		/// 获取当前的timestamp时间戳
		/// </summary>
		/// <returns></returns>
		public static uint ConvertDateToLong()
		{
			DateTime dateToConvert = System.DateTime.Now;
			double result;
			DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
			TimeSpan span = dateToConvert - utcDate;
			result = span.TotalSeconds;
			return (uint)result;
		}

		/// <summary>
		/// 把北京时间转换为timestamp时间戳
		/// </summary>
		/// <param name="dateToConvert"></param>
		/// <returns></returns>
		public static double ConvertDateToDouble(DateTime dateToConvert)
		{
			double result;
			DateTime utcDate = new DateTime(1970, 1, 1, 8, 0, 0);
			TimeSpan span = dateToConvert - utcDate;
			result = span.TotalSeconds;
			return result;
		}

		/// <summary>
		/// 把timestamp时间戳转换为北京时间
		/// </summary>
		/// <param name="secondsSinceEpoch"></param>
		/// <returns></returns>
		public static DateTime ConvertDoubleToDate(long secondsSinceEpoch)
		{
			//TimeZone localZone = TimeZone.CurrentTimeZone;
			DateTime utcdate = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds((double)secondsSinceEpoch);
			//DateTime localTime = localZone.ToLocalTime(utcdate);

			return utcdate;
		}

		/// <summary>
		/// 根据时间戳获取北京时间（带缓存性能很好）
		/// </summary>
		/// <returns></returns>
		public static DateTime GetTimeBeijing(int timestamp)
		{
			if (beijingTimes == null)
			{
				beijingTimes = new Dictionary<int, DateTime>();
			}

			if (beijingTimes.Count > 10000)
			{
				beijingTimes.Clear();
			}

			if (!beijingTimes.ContainsKey(timestamp))
			{
				DateTime utc = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timestamp);
				DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(utc, BeijingTimeZone);  //这个步骤会有点耗时

				beijingTimes.Add(timestamp, dt);
			}

			var ret = beijingTimes[timestamp];
			return ret;
		}

		/// <summary>
		/// 根据时间戳获取美国东部时间（带缓存性能很好）
		/// </summary>
		/// <returns></returns>
		public static DateTime GetTimeUS(int timestamp)
		{
			if (usTimes == null)
			{
				usTimes = new Dictionary<int, DateTime>();
			}

			if (usTimes.Count > 10000)
			{
				usTimes.Clear();
			}

			if (!usTimes.ContainsKey(timestamp))
			{
				DateTime utc = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timestamp);
				DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(utc, USEastTimeZone);   //这个步骤会有点耗时

				usTimes.Add(timestamp, dt);
			}

			DateTime ret = usTimes[timestamp];
			return ret;
		}

		/// <summary>
		/// 根据utc时间获取美国东部时间
		/// </summary>
		/// <returns></returns>
		public static DateTime GetTimeUS(DateTime utcTime)
		{
			int ts = GetTimestampUTC(utcTime);

			DateTime dt = GetTimeUS(ts);

			return dt;
		}

		/// <summary>
		/// 根据utc timestamp 获取utc时间
		/// </summary>
		/// <param name="utctimestamp"></param>
		/// <returns></returns>
		public static DateTime GetTimeUTC(int utctimestamp)
		{
			DateTime utc = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(utctimestamp);
			return utc;
		}

		/// <summary>
		/// 根据UTC时间获取时间戳
		/// </summary>
		/// <param name="utc"></param>
		/// <returns></returns>
		public static int GetTimestampUTC(DateTime utc)
		{
			DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0);
			TimeSpan span = utc - epoch;
			int result = (int)span.TotalSeconds;
			return result;
		}

		/// <summary>
		/// 根据UTC时间获取时间戳
		/// </summary>
		/// <param name="utc"></param>
		/// <returns></returns>
		public static double GetDoubleTimestampUTC(DateTime utc)
		{
			DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0);
			TimeSpan span = utc - epoch;
			double result = span.TotalSeconds;
			return result;
		}

		/// <summary>
		/// 获取北京时间的时间戳
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static int GetTimeStampBeijing(DateTime dt)
		{
			DateTime utc = TimeZoneInfo.ConvertTimeToUtc(dt, BeijingTimeZone);

			return GetTimestampUTC(utc);
		}

		/// <summary>
		/// 获取本地时间的时间戳
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static int GetTimeStampLocal(DateTime dt)
		{
			DateTime utc = TimeZoneInfo.ConvertTimeToUtc(dt, TimeZoneInfo.Local);

			return GetTimestampUTC(utc);
		}

		/// <summary>
		/// 获取当前的美国时间
		/// </summary>
		/// <returns></returns>
		public static DateTime GetNowUS()
		{
			int ts = GetTimestampNow();

			DateTime dt = GetTimeUS(ts);

			return dt;
		}

		/// <summary>
		/// 获取当前北京时间
		/// </summary>
		/// <returns></returns>
		public static DateTime GetNowBeigjing()
		{
			int ts = GetTimestampNow();

			DateTime dt = GetTimeBeijing(ts);

			return dt;
		}

		/// <summary>
		/// 获取美国时间的时间戳
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static int GetTimeStampUS(DateTime dt)
		{
			DateTime utc = TimeZoneInfo.ConvertTimeToUtc(dt, USEastTimeZone);

			return GetTimestampUTC(utc);
		}

		/// <summary>
		/// 返回当前的时间戳。timestamp本身是不分时区的，只是以各个时区的形式显示。
		/// </summary>
		/// <returns></returns>
		public static int GetTimestampNow()
		{
			return GetTimestampUTC(DateTime.UtcNow);
		}
	}
}
