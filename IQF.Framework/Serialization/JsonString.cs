using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace IQF.Framework.Serialization
{
	/// <summary>
	/// 方便易用的json封装
	/// </summary>
	public class JsonString
	{
		private readonly Dictionary<string, object> resultDic = new Dictionary<string, object>();

		public JsonString()
		{
		}

		/// <summary>
		/// 失败可能抛异常。TryParse方法可以避免抛异常。
		/// </summary>
		/// <param name="json"></param>
		public JsonString(string json)
		{
			var ay = this.ParseJson(json);
			foreach (var item in ay)
			{
				this.Set(item.Key, item.Value);
			}
		}

		public JsonString(IDictionary<string, object> ay)
		{
			foreach (var item in ay)
			{
				this.Set(item.Key, item.Value);
			}
		}

		/// <summary>
		/// key存在就更新，不存在就新增
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(string key, object value)
		{
			if (resultDic.ContainsKey(key))
			{
				resultDic[key] = value;
			}
			else
			{
				resultDic.Add(key, value);
			}
		}

		/// <summary>
		/// key存在就更新，不存在就新增（符合一般逻辑的支持JsonString类型的value）
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set(string key, JsonString value)
		{
			if (resultDic.ContainsKey(key))
			{
				resultDic[key] = value.ToDic();
			}
			else
			{
				resultDic.Add(key, value.ToDic());
			}
		}

		/// <summary>
		/// 给名为arrayName的数组，新增一个json格式的条目
		/// </summary>
		/// <param name="arrayName"></param>
		/// <param name="itemKey"></param>
		/// <param name="itemValue"></param>
		public void AddArrayItem(string arrayName, JsonString item)
		{
			var dic = item.ToDic();
			if (resultDic.ContainsKey(arrayName))
			{
				List<Dictionary<string, object>> list = (List<Dictionary<string, object>>)resultDic[arrayName];
				list.Add(dic);
			}
			else
			{
				var list = new List<Dictionary<string, object>>();
				list.Add(dic);
				resultDic.Add(arrayName, list);
			}
		}

		/// <summary>
		/// 给名为itemName的数组，新增一个json格式的条目
		/// </summary>
		public void AddItem(string itemName, JsonString item)
		{
			var dic = item.ToDic();
			if (resultDic.ContainsKey(itemName))
			{
				resultDic[itemName] = dic;

			}
			else
			{
				resultDic.Add(itemName, dic);

			}
		}

		/// <summary>
		/// 给名为arrayName的数组，新增一个object的条目
		/// </summary>
		/// <param name="arrayName"></param>
		/// <param name="item"></param>
		public void AddArrayItem(string arrayName, object item)
		{
			if (resultDic.ContainsKey(arrayName))
			{
				List<object> list = (List<object>)resultDic[arrayName];
				list.Add(item);
			}
			else
			{
				var list = new List<object>();
				list.Add(item);
				resultDic.Add(arrayName, list);
			}
		}

		/// <summary>
		/// 尝试加一个空数组
		/// </summary>
		/// <param name="arrayName"></param>
		public void AddArray(string arrayName)
		{
			if (!resultDic.ContainsKey(arrayName))
			{
				var list = new List<object>();
				resultDic.Add(arrayName, list);
			}
		}

		public bool ContainsKey(string key)
		{
			return resultDic.ContainsKey(key);
		}

		/// <summary>
		/// 如果存在则移除，如果不存在则不做任何事情
		/// </summary>
		/// <param name="key"></param>
		public void RemoveKey(string key)
		{
			if (resultDic.ContainsKey(key))
			{
				resultDic.Remove(key);
			}
		}

		/// <summary>
		/// 根据key获取value，如果key不存在则返回null
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string Get(string key)
		{
			if (!resultDic.ContainsKey(key))
			{
				return null;
			}
			if (resultDic[key] == null)
			{
				return string.Empty;
			}
			return resultDic[key].ToString();
		}

		/// <summary>
		/// 根据key获取value
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public int GetInt(string key, int defaultValue = 0)
		{
			return Get(key).ToInt(defaultValue);
		}

		/// <summary>
		/// 根据key获取value
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public long GetLong(string key, long defaultValue = 0)
		{
			return Get(key).ToLong(defaultValue);
		}

		/// <summary>
		/// 根据key获取value
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public double GetDouble(string key, double defaultValue = 0)
		{
			return Get(key).ToDouble(defaultValue);
		}

		/// <summary>
		/// 根据key获取value
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public decimal GetDecimal(string key, decimal defaultValue = 0)
		{
			return Get(key).ToDecimal(defaultValue);
		}

		/// <summary>
		/// 根据key获取value
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public float GetFloat(string key, float defaultValue = 0)
		{
			return Get(key).ToFloat(defaultValue);
		}

		/// <summary>
		/// 根据key获取value，失败抛异常
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public DateTime GetDatetime(string key, string formartor = "yyyy-MM-dd HH:mm:ss", DateTime defaultTime = new DateTime())
		{
			if (!resultDic.ContainsKey(key) || resultDic[key] == null)
			{
				return defaultTime;
			}
			if (resultDic[key] is DateTime)
			{
				return (DateTime)resultDic[key];
			}
			return Get(key).ToDateTime(defaultTime, formartor);
		}

		/// <summary>
		/// 获取对象
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object GetObject(string key)
		{
			if (!resultDic.ContainsKey(key))
			{
				return null;
			}
			return resultDic[key];
		}

		/// <summary>
		/// 对应AddArrayItem()。key不存在返回长度为零的array
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public JsonString[] GetArray(string key)
		{
			var items = GetObject(key) as JArray;
			if (items == null)
			{
				return new JsonString[0];
			}
			if (items.Count == 0)
			{
				return new JsonString[0];
			}
			var ret = new List<JsonString>();
			foreach (var item in items)
			{
				var dic = item.ToObject<Dictionary<string, object>>();
				ret.Add(new JsonString(dic));
			}
			return ret.ToArray();
		}

		public Dictionary<string, object> ToDic()
		{
			return this.resultDic;
		}

		public override string ToString()
		{
			return JsonHelper.Serialize(this.resultDic);
		}

		private Dictionary<string, object> ParseJson(string input)
		{
			var dict = JsonHelper.Deserialize<Dictionary<string, object>>(input);
			return dict;
		}
	}
}
