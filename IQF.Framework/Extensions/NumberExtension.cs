using System;

namespace IQF.Framework
{
	public static class NumberExtension
	{
		private const double MDOT = 0.00001F;

		/// <summary>
		/// a <= b
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool LessEqual(this double a, double b, double precision = MDOT)
		{
			return a - b < precision;
		}

		/// <summary>
		/// a >= b
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool GreaterEqual(this double a, double b, double precision = MDOT)
		{
			return b - a < precision;
		}

		/// <summary>
		/// return if (a &lt; b)
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool Less(this double a, double b, double precision = MDOT)
		{
			return (b - a > precision);
		}

		/// <summary>
		/// return if (a > b)
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool Greater(this double a, double b, double precision = MDOT)
		{
			return (a - b > precision);
		}

		/// <summary>
		/// 安全的除法
		/// </summary>
		/// <param name="dividend"></param>
		/// <param name="divisor"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float Divide(this float dividend, float divisor, float defaultValue = 0)
		{
			if (divisor == 0)
			{
				return defaultValue;
			}
			return dividend / divisor;
		}

		/// <summary>
		/// 安全的除法
		/// </summary>
		/// <param name="dividend"></param>
		/// <param name="divisor"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double Divide(this double dividend, double divisor, double defaultValue = 0)
		{
			if (divisor == 0)
			{
				return defaultValue;
			}
			return dividend / divisor;
		}

		/// <summary>
		/// 安全的除法
		/// </summary>
		/// <param name="dividend"></param>
		/// <param name="divisor"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int Divide(this int dividend, int divisor, int defaultValue = 0)
		{
			if (divisor == 0)
			{
				return defaultValue;
			}
			return dividend / divisor;
		}

		/// <summary>
		/// 安全的除法
		/// </summary>
		/// <param name="dividend"></param>
		/// <param name="divisor"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Int64 Divide(this Int64 dividend, Int64 divisor, int defaultValue = 0)
		{
			if (divisor == 0)
			{
				return defaultValue;
			}
			return dividend / divisor;
		}

		/// <summary>
		/// 取余  如果除数是0，返回0
		/// </summary>
		/// <param name="dividend"></param>
		/// <param name="divisor"></param>
		/// <returns></returns>
		public static int ComplementBy(this int dividend, int divisor)
		{
			if (divisor == 0)
			{
				return 0;
			}
			return dividend % divisor;
		}

		/// <summary>
		/// 格式化
		/// 比较两个数值，如果当前值大于另一个数值，则新增加号，再格式化
		/// </summary>
		/// <param name="value"></param>
		/// <param name="compare"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static string Format(this float value, float compare, string format)
		{
			var head = string.Empty;
			if (value > compare) head = "+";
			if (value < compare && value >= 0) head = "-";

			return head + value.ToString(format);
		}

		/// <summary>
		/// 格式化
		/// 比较两个数值，如果当前值大于另一个数值，则新增加号，再格式化
		/// </summary>
		/// <param name="value"></param>
		/// <param name="compare"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static string Format(this decimal value, decimal compare, string format)
		{
			var head = string.Empty;
			if (value > compare) head = "+";
			if (value < compare && value >= 0) head = "-";

			return head + value.ToString(format);
		}

		/// <summary>
		/// 格式化
		/// 比较两个数值，如果当前值大于另一个数值，则新增加号，再格式化
		/// </summary>
		/// <param name="value"></param>
		/// <param name="compare"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static string Format(this double value, double compare, string format)
		{
			var head = string.Empty;
			if (value > compare) head = "+";
			if (value < compare && value >= 0) head = "-";
			return head + value.ToString(format);
		}

		/// <summary>
		/// 格式化
		/// 比较两个数值，如果当前值大于另一个数值，则新增加号
		/// </summary>
		/// <param name="value"></param>
		/// <param name="compare"></param>
		/// <returns></returns>
		public static string Format(this int value, double compare)
		{
			var head = string.Empty;
			if (value > compare)
			{
				head = "+";
			}
			return head + value;
		}

		/// <summary>
		/// 格式化
		/// 比较两个数值，如果当前值大于另一个数值，则新增加号
		/// </summary>
		/// <param name="value"></param>
		/// <param name="compare"></param>
		/// <returns></returns>
		public static string Format(this long value, double compare)
		{
			var head = string.Empty;
			if (value > compare)
			{
				head = "+";
			}
			return head + value;
		}

		/// <summary>
		/// 格式化成百分比的形式
		/// 0.98 = 98.00%
		/// </summary>
		/// <param name="value"></param>
		/// <param name="formart"></param>
		/// <returns></returns>
		public static string FormatPercent(this double value, string formart = "0.00")
		{
			string str = (value * 100).ToString(formart);
			return str + "%";
		}

		/// <summary>
		/// 格式化成百分比的形式
		/// 0.98 = 98.00%
		/// </summary>
		/// <param name="value"></param>
		/// <param name="formart"></param>
		/// <returns></returns>
		public static string FormatPercent(this float value, string formart = "0.00")
		{
			string str = (value * 100).ToString(formart);
			return str + "%";
		}

		/// <summary>
		/// 格式化
		/// </summary>
		/// <param name="value"></param>
		/// <param name="formart">格式化字符串</param>
		/// <param name="specialValue">需要特殊处理的数字</param>
		/// <param name="specialStr">特殊处理数字返回的字符串</param>
		/// <returns></returns>
		public static string Format(this double value, string formart = "0.00#", double specialValue = 0, string specialStr = "--")
		{
			if (value == specialValue)
			{
				return specialStr;
			}
			return value.ToString(formart);
		}

		/// <summary>
		/// 根据精度获取格式化字符串
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetFormart(this double value)
		{
			if (value >= 1)
			{
				return "0";
			}
			else if (value >= 0.1)
			{
				return "0.0";
			}
			else if (value >= 0.01)
			{
				return "0.00";
			}
			else if (value >= 0.001)
			{
				return "0.000";
			}
			else
			{
				return "0.00";
			}
		}

		/// <summary>
		/// 获取小数位整数化 次方数
		/// </summary>
		/// <param name="precision"></param>
		/// <returns></returns>
		public static int GetPlaces(this double precision)
		{
			if (precision >= 1)
			{
				return 0;
			}
			else if (precision >= 0.1)
			{
				return 1;
			}
			else if (precision >= 0.01)
			{
				return 2;
			}
			else if (precision >= 0.001)
			{
				return 3;
			}
			else
			{
				return 4;
			}
		}

		/// <summary>
		/// 获取小数位整数化 次方数
		/// </summary>
		/// <param name="precision"></param>
		/// <returns></returns>
		public static int GetPlaces(this decimal precision)
		{
			return GetPlaces((double)precision);
		}

		/// <summary>
		/// 根据精度获取格式化字符串
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetFormart(this decimal value)
		{
			return GetFormart((double)value);
		}

		/// <summary>
		/// 根据精度获取格式化字符串
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetFormart(this float value)
		{
			return GetFormart((double)value);
		}

		/// <summary>
		/// 获取将浮点数转化为整数所需的乘数
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int GetToIntMultiplier(this double value)
		{
			if (value >= 1)
			{
				return 1;
			}
			else if (value >= 0.1)
			{
				return 10;
			}
			else if (value >= 0.01)
			{
				return 100;
			}
			else if (value >= 0.001)
			{
				return 1000;
			}
			else
			{
				return 10000;
			}
		}

		/// <summary>
		/// 获取将浮点数转化为整数所需的乘数
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int GetToIntMultiplier(this decimal value)
		{
			return GetToIntMultiplier((double)value);
		}

		/// <summary>
		/// 默认保留两位小数 XX万
		/// </summary>
		/// <param name="money"></param>
		/// <returns></returns>
		public static string ToMoneyFormat(this double val)
		{
			string totalStockNum = "0";
			double money = Math.Abs(val);

			if (money / 10000 < 1)
			{
				totalStockNum = Math.Round(money, 3) + "";
			}
			else if (money / (10000.0 * 1000) < 1)
			{
				totalStockNum = Math.Round(money / 10000.0, 3).ToString("F2") + "万";
			}
			else if (money / (10000.0 * 10000) < 1)
			{
				totalStockNum = Math.Round(money / 10000.0, 3).ToString("F0") + "万";
			}
			else if (money / (10000.0 * 10000 * 1000) < 1)
			{
				totalStockNum = Math.Round(money / (10000 * 10000.0), 3).ToString("F2") + "亿";
			}
			else if (money / (10000.0 * 10000 * 10000) < 1)
			{
				totalStockNum = Math.Round(money / (10000 * 10000.0), 3).ToString("F0") + "亿";
			}
			else
				totalStockNum = Math.Round(money / (10000 * 10000.0 * 10000), 3).ToString("F2") + "万亿";

			if (val < 0)
			{
				totalStockNum = "-" + totalStockNum;
			}
			return totalStockNum;
		}
	}
}
