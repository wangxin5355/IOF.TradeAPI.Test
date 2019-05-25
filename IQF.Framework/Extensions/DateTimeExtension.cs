using System;

namespace IQF.Framework
{
	public static class DateTimeExtension
	{
		/// <summary>
		/// 获取形如20150702这样格式的日期
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static int GetYYYYMMDD(this DateTime dt)
		{
			int ret = dt.Year * 10000 + dt.Month * 100 + dt.Day;
			return ret;
		}

		/// <summary>
		/// 获取形如93012这样的时间
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static int GetHHMMSS(this DateTime dt)
		{
			int ret = dt.Hour * 10000 + dt.Minute * 100 + dt.Second;
			return ret;
		}

		/// <summary>
		/// 获取形如930这样的时间
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static int GetHHMM(this DateTime dt)
		{
			int ret = dt.Hour * 100 + dt.Minute;
			return ret;
		}

		/// <summary>
		/// 获取形如201809121018这样格式的日期+时间
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static long GetYYYYMMDDHH(this DateTime dt)
		{
			var hhmm = dt.GetHHMM();
			var yyyyMMdd = dt.GetYYYYMMDD();
			var str = yyyyMMdd + hhmm.ToString().PadLeft(4, '0');
			return str.ToLong(0);
		}

		/// <summary>
		/// 获取形如20180912101806这样格式的日期+时间
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static long GetYYYYMMDDHHSS(this DateTime dt)
		{
			var hhmmss = dt.GetHHMMSS();
			var yyyyMMdd = dt.GetYYYYMMDD();
			var str = yyyyMMdd + hhmmss.ToString().PadLeft(6, '0');
			return str.ToLong(0);
		}

		/// <summary>
		/// 返回"yyyy-MM-dd HH:mm:ss.fff"格式的时间，满足sql的要求，并精确到毫秒
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string ToStringSQL(this DateTime dt)
		{
			string ss = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
			return ss;
		}

		/// <summary>
		/// 转换时间为unix时间戳
		/// </summary>
		/// <param name="date">需要传递UTC时间,避免时区误差,例:DataTime.UTCNow</param>
		/// <returns></returns>
		public static long GetUtcTimeStamp(this DateTime time)
		{
			var startTime = new DateTime(1970, 1, 1).ToLocalTime();
			//DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			return (long)(time - startTime).TotalSeconds;
		}

		public static Date ToDate(this DateTime dt)
		{
			return new Date(dt);
		}

		public static DateTime ToDate(this int yyyyMMdd)
		{
			DateTime dt = new DateTime(yyyyMMdd / 10000, (yyyyMMdd % 10000) / 100, yyyyMMdd % 100);
			return dt;
		}

		public static DateTime GetMonday(this DateTime dateTime)
		{
			int weeknow = Convert.ToInt32(dateTime.DayOfWeek);
			weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
			int daydiff = (-1) * weeknow;
			return dateTime.AddDays(daydiff);
		}

		public static DateTime GetSunday(this DateTime dateTime)
		{
			int weeknow = Convert.ToInt32(dateTime.DayOfWeek);
			weeknow = weeknow == 0 ? 0 : 7 - weeknow;
			return dateTime.AddDays(weeknow);
		}

		public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, dateTime.Month, 1);
		}

		public static DateTime GetFirstDayOfYear(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, 1, 1);
		}

		public static DateTime GetEndDayOfMonth(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
		}

		public static DateTime GetEndDayOfYear(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, 12, DateTime.DaysInMonth(dateTime.Year, 12));
		}

		public static string GetTimeFormat(this DateTime dateTime)
		{
			var currTime = DateTime.Now;
			TimeSpan ts = currTime - dateTime;
			if (dateTime.Year < currTime.Year)
			{
				return dateTime.ToString("yyyy年MM月dd日");
			}
			if (ts.TotalDays > 1 && dateTime.Year == currTime.Year)
			{
				return dateTime.ToString("MM月dd日");
			}
			if (ts.TotalDays < 1 && ts.TotalHours > 1)
			{
				return string.Format("{0}小时前", (int)ts.TotalHours);
			}
			if (ts.TotalHours < 1 && ts.TotalMinutes > 1)
			{
				return string.Format("{0}分钟前", (int)ts.TotalMinutes);
			}
			if (ts.Minutes < 1 && ts.Seconds > 1)
			{
				return string.Format("{0}秒前", (int)ts.TotalSeconds);
			}
			return "刚刚";
		}
	}
}
