using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace IQF.Framework
{
	public static class StringExtension
	{
		/// <summary>
		/// 当字符串为null时就转化为空字符串
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ToEmptyIfNull(this string str)
		{
			if (str == null)
			{
				return string.Empty;
			}
			else
			{
				return str;
			}
		}

		/// <summary>
		/// 为NULL返回empty，否则tostring
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string ToEmptyIfNull(this object obj)
		{
			if (obj == null)
			{
				return string.Empty;
			}
			else
			{
				return obj.ToString();
			}
		}

		/// <summary>
		/// 按utf8获取bytes
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static byte[] GetBytesUtf8(this string s)
		{
			return Encoding.UTF8.GetBytes(s);
		}

		/// <summary>
		/// 不抛异常的转换必须有个默认值。如果想要抛异常可以用int.Parse
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int ToInt(this string s, int defaultValue = 0)
		{
			int ret = 0;
			if (int.TryParse(s, out ret))
			{
				return ret;
			}
			else
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// 不抛异常的转换必须有个默认值。如果想要抛异常可以用int.Parse
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long ToLong(this string s, long defaultValue = 0)
		{
			long ret = 0;
			if (long.TryParse(s, out ret))
			{
				return ret;
			}
			else
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// 是否为纯数字
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static bool IsNumber(this string src)
		{
			if (string.IsNullOrWhiteSpace(src))
			{
				return false;
			}
			int result;
			if (int.TryParse(src, out result))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 不抛异常的转换必须有个默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double ToDouble(this string s, double defaultValue = 0)
		{
			double ret;
			if (double.TryParse(s, out ret))
			{
				return ret;
			}
			else
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// 不抛异常的转换必须有个默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static decimal ToDecimal(this string s, decimal defaultValue = 0)
		{
			decimal ret;
			if (decimal.TryParse(s, out ret))
			{
				return ret;
			}
			else
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// 不抛异常的转换必须有个默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float ToFloat(this string s, float defaultValue = 0F)
		{
			float ret;
			if (float.TryParse(s, out ret))
			{
				return ret;
			}
			else
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// 无法parse则返回零
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double TryParseToDouble(this string s)
		{
			double ret = 0.0;
			if (s != null)
			{
				double.TryParse(s, out ret);
			}
			return ret;
		}

		/// <summary>
		/// 按utf8获取string
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string GetStringUtf8(this byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}

		/// <summary>
		/// 返回字符串左边最多len个字符
		/// </summary>
		/// <param name="value"></param>
		/// <param name="len"></param>
		/// <returns></returns>
		public static string Left(this string value, int len)
		{
			if (string.IsNullOrEmpty(value)) return value;

			return (value.Length <= len
				   ? value
				   : value.Substring(0, len)
				   );
		}

		/// <summary>
		/// 返回字符串右边最多len个字符
		/// </summary>
		/// <param name="value"></param>
		/// <param name="len"></param>
		/// <returns></returns>
		public static string Right(this string value, int len)
		{
			if (string.IsNullOrEmpty(value)) return value;

			return (value.Length <= len
				   ? value
				   : value.Substring(value.Length - len, len)
				   );
		}

		/// <summary>
		/// 获取字典值，否则返回默认值
		/// </summary>
		/// <param name="dict"></param>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
		{
			if (dict == null)
			{
				return defaultValue;
			}
			TValue value;
			if (dict.TryGetValue(key, out value))
			{
				return value;
			}
			return defaultValue;
		}

		/// <summary>
		/// 获取枚举类型的描述信息
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static string Description(this Enum e)
		{
			var enumType = e.GetType();
			var fieldInfo = enumType.GetFields().FirstOrDefault(a => a.Name == Enum.GetName(enumType, e));

			if (fieldInfo == null) return "";
			object[] obj = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
			DescriptionAttribute des = (DescriptionAttribute)obj[0];
			return des.Description;
		}

		/// <summary>
		/// 时间转化
		/// </summary>
		/// <param name="str"></param>
		/// <param name="defaultTime"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(this string str, DateTime defaultTime, string format = "yyyyMMddHHmmss")
		{
			if (string.IsNullOrWhiteSpace(str) || string.IsNullOrWhiteSpace(format))
			{
				return defaultTime;
			}
			DateTime result;
			if (DateTime.TryParseExact(str, format, null, System.Globalization.DateTimeStyles.None, out result))
			{
				return result;
			}
			return defaultTime;
		}

		/// <summary>
		/// 如果源是NULL，则返回默认值
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static string SafeToString(this object src, string defaultValue = "")
		{
			if (src == null)
			{
				return defaultValue;
			}
			return src.ToString();
		}

		/// <summary>
		/// 将一个或多个枚举常数的名称或数字值的字符串表示转换成等效的枚举对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="src"></param>
		/// <returns></returns>
		public static T ToEnum<T>(this string src) where T : struct
		{
			T data = default(T);
			if (src != null)
			{
				Enum.TryParse<T>(src, out data);
			}
			return data;
		}

		/// <summary>
		/// 安全的枚举转换
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="src"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T ToTryEnum<T>(this string src, T defaultValue = default(T)) where T : struct
		{
			T model;
			if (Enum.TryParse(src, out model))
			{
				return model;
			}
			return defaultValue;
		}

		/// <summary>
		/// 安全的枚举转换
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="src"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T ToTryEnum<T>(this int val, T defaultValue = default(T)) where T : struct
		{
			T model;
			if (Enum.TryParse(val.ToString(), out model))
			{
				return model;
			}
			return defaultValue;
		}

		/// <summary>
		/// 判断是否是手机号码
		/// </summary>
		/// <param name="str">测试字符串</param>
		/// <returns>true 是 false 不就</returns>
		public static bool IsMobile(this string str)
		{
			Regex reg = new Regex(@"^((13|15|16|18|17|19)\d{9})|((14[57])\d{8})$");
			return reg.IsMatch(str);
		}

		/// <summary>
		/// 是否中国移动的号码
		/// </summary>
		/// <param name="str">测试字符串</param>
		/// <returns>true 是 false 不就</returns>
		public static bool IsChinaMobile(this string str)
		{
			Regex reg = new Regex(@"^(((13)[4-9]{1})|((15)[0,1,2,7,8,9]{1})|(147)|((18)[2,7,8]{1}))\d{8}$");

			return reg.IsMatch(str);
		}

		/// <summary>
		/// 判断是否包含手机号码
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsContainMobile(this string str)
		{
			Regex reg = new Regex(@"((13|15|18)\d{9})|((14[57]|17[0135678])\d{8})");
			return reg.IsMatch(str);
		}

		/// <summary>
		/// 判断是否是Decimal类型格式变量
		/// </summary>
		/// <param name="str">测试字符串</param>
		/// <returns>true/false</returns>
		public static bool IsDecimal(this string str)
		{
			Regex reg = new Regex(@"^\d{1,10}(\.\d{1,2})?$");
			return reg.IsMatch(str);
		}

		/// <summary>
		/// 判断是否是正确的Email信息。
		/// </summary>
		/// <param name="str">测试字符串。</param>
		/// <returns>true/false</returns>
		public static bool IsEmail(this string str)
		{
			string validationExpression = @"^[A-Z,a-z,0-9]+([-+._][A-Z,a-z,0-9]+)*@[A-Z,a-z,0-9]+([-.][A-Z,a-z,0-9]+)*\.[A-Z,a-z,0-9]+([-.][A-Z,a-z,0-9]+)*$";
			Regex reg1
				= new Regex(validationExpression);
			return reg1.IsMatch(str);
		}

		/// <summary>
		/// 判断是否是IP地址。
		/// </summary>
		/// <param name="ipAdress">IP地址</param>
		/// <returns>false/true</returns>
		public static bool IsIPAdress(this string ipAdress)
		{
			Regex reg1
				= new Regex(@"^(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))$");
			return reg1.IsMatch(ipAdress);
		}

		/// <summary>
		/// 首字母小写
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static string LowerFirst(this string src)
		{
			if (string.IsNullOrEmpty(src))
				return src;
			return Char.ToLowerInvariant(src[0]) + src.Substring(1);
		}
	    public static string[] Split(this string str, string splitStr)
	    {
	        return Regex.Split(str, splitStr, RegexOptions.IgnoreCase);
	    }

	    public static string AppendNewLine(this string str, string newstr)
	    {
	        if (!string.IsNullOrWhiteSpace(str))
	        {
	            return str + newstr + Environment.NewLine;

	        }
	        return newstr + Environment.NewLine;

	    }

	    /// <summary>
	    /// 扩展方法，获得枚举的Description
	    /// </summary>
	    /// <param name="value">枚举值</param>
	    /// <param name="nameInstend">当枚举没有定义DescriptionAttribute,是否用枚举名代替，默认使用</param>
	    /// <returns>枚举的Description</returns>
	    public static string GetDescription(this Enum value, bool nameInstend = true)
	    {
	        Type type = value.GetType();
	        string name = Enum.GetName(type, value);
	        if (name == null)
	        {
	            return null;
	        }
	        FieldInfo field = type.GetField(name);
	        DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
	        if (attribute == null && nameInstend == true)
	        {
	            return name;
	        }
	        return attribute == null ? null : attribute.Description;
	    }
    }
}
