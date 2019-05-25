using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace IQF.Framework.Serialization
{
	/// <summary>
	/// JSON帮助类
	/// </summary>
	public static class JsonHelper
	{
		/// <summary>
		/// 序列化
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="dateTimeFormat"></param>
		/// <param name="isCamelCase"></param>
		/// <returns></returns>
		public static string Serialize(object obj, JsonSerializerSettings jsonSerializerSettings = null)
		{
			if (obj == null)
			{
				return string.Empty;
			}
			if (jsonSerializerSettings == null)
			{
				jsonSerializerSettings = GetDefaultJsonSetting();
			}
			return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
		}

		/// <summary>
		/// 反序列化
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <param name="dateTimeFormat"></param>
		/// <param name="isCamelCase"></param>
		/// <returns></returns>
		public static T Deserialize<T>(string json, JsonSerializerSettings jsonSerializerSettings = null)
		{
			if (string.IsNullOrWhiteSpace(json))
			{
				return default(T);
			}
			if (jsonSerializerSettings == null)
			{
				jsonSerializerSettings = GetDefaultJsonSetting();
			}

			return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
		}

		/// <summary>
		/// 反序列化
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <param name="dateTimeFormat"></param>
		/// <param name="isCamelCase"></param>
		/// <returns></returns>
		public static object Deserialize(string json, Type type, JsonSerializerSettings jsonSerializerSettings = null)
		{
			if (string.IsNullOrWhiteSpace(json))
			{
				return null;
			}
			if (jsonSerializerSettings == null)
			{
				jsonSerializerSettings = GetDefaultJsonSetting();
			}

			return JsonConvert.DeserializeObject(json, type, jsonSerializerSettings);
		}

		/// <summary>
		/// 反序列化匿名类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="jstr"></param>
		/// <param name="typeInfo"></param>
		/// <returns></returns>
		public static T DeserializeAnonymousType<T>(string json, T typeInfo, JsonSerializerSettings jsonSerializerSettings = null)
		{
			if (string.IsNullOrWhiteSpace(json) || typeInfo == null)
			{
				return default(T);
			}
			if (jsonSerializerSettings == null)
			{
				jsonSerializerSettings = GetDefaultJsonSetting();
			}
			return JsonConvert.DeserializeAnonymousType<T>(json, typeInfo, jsonSerializerSettings);
		}

		/// <summary>
		/// 获取JSON值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static T GetValue<T>(string json, string key)
		{
			if (string.IsNullOrWhiteSpace(json) || string.IsNullOrWhiteSpace(key))
			{
				return default(T);
			}
			var jobject = JObject.Parse(json);
			var jtoken = jobject.GetValue(key);
			if (jtoken == null)
			{
				return default(T);
			}
			if (typeof(T) == typeof(string))
			{
				return (T)(Object)jtoken.ToString();
			}
			return jtoken.ToObject<T>();
		}

		/// <summary>
		/// 获取默认JSON序列化设置
		/// </summary>
		/// <returns></returns>
		public static JsonSerializerSettings GetDefaultJsonSetting()
		{
			var settings = new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new NullToEmptyStringResolver()
				{
					NamingStrategy = new CamelCaseNamingStrategy()
					{
						OverrideSpecifiedNames = false
					}
				},
				DateFormatString = "yyyy-MM-dd HH:mm:ss"
			};
			settings.Converters.Add(new StringEnumConverter());
			return settings;
		}

		/// <summary>
		/// Json序列化改为默认配置
		/// </summary>
		/// <param name="settings"></param>
		public static void ResetDefault(this JsonSerializerSettings settings)
		{
			var jsonSettings = JsonHelper.GetDefaultJsonSetting();
			settings.ReferenceLoopHandling = jsonSettings.ReferenceLoopHandling;
			settings.ContractResolver = jsonSettings.ContractResolver;
			settings.DateFormatString = jsonSettings.DateFormatString;
			settings.Converters = jsonSettings.Converters;
			settings.FloatFormatHandling = jsonSettings.FloatFormatHandling;
			settings.FloatParseHandling = jsonSettings.FloatParseHandling;
		}
	}
}
