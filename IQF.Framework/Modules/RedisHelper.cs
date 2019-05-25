using IQF.Framework.Cache;
using IQF.Framework.Serialization;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IQF.Framework
{
	/// <summary>
	/// redis数据访问
	/// </summary>
	public class RedisHelper : IDistributedCache
	{
		/// <summary>
		/// 连接串
		/// </summary>
		private readonly string connectString = null;

		/// <summary>
		/// 连接池
		/// </summary>
		private readonly List<ConnectionMultiplexer> connectionPool = new List<ConnectionMultiplexer>();

		/// <summary>
		/// 默认连接数
		/// </summary>
		private readonly int poolSize = 5;

		/// <summary>
		/// 锁对象
		/// </summary>
		private readonly object objLock = new object();

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="connectString">
		/// 连接字符串
		/// "redis0:6380,redis1:6380,defaultDatabase=0,poolSize=10"
		/// </param>
		public RedisHelper(string connectString)
		{
			this.connectString = connectString;

			if (!string.IsNullOrWhiteSpace(connectString))
			{
				var val = Regex.Match(connectString, @"poolSize=\d+").Value;
				var poolSize = val.Replace("poolSize=", "").ToInt(this.poolSize);
				this.poolSize = Math.Max(poolSize, 1);
			}
		}

		/// <summary>
		/// 添加数据
		/// </summary>
		public bool Set<T>(string key, T val)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return false;
			}
			var ss = this.Serialize(val);
			return this.GetDatabase().StringSet(key, ss);
		}

		public bool Set<T>(string key, T val, DateTime expiresAt)
		{
			return this.Set(key, val, expiresAt - DateTime.Now);
		}

		public bool Set<T>(string key, T val, TimeSpan expiresIn)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return false;
			}
			var ss = this.Serialize(val);
			return this.GetDatabase().StringSet(key, ss, expiresIn);
		}

		/// <summary>
		/// 设置到期
		/// </summary>
		/// <param name="key"></param>
		/// <param name="expiresAt"></param>
		/// <returns></returns>
		public void KeyExpire(IEnumerable<string> keys, DateTime expiresAt)
		{
			var db = GetDatabase();
			var batch = db.CreateBatch();
			foreach (var key in keys)
			{
				batch.KeyExpireAsync(key, expiresAt, CommandFlags.FireAndForget);
			}
			batch.Execute();
		}

		/// <summary>
		/// 批量更新
		/// </summary>
		/// <param name="actions"></param>
		/// <returns></returns>
		public void BatchSet<T>(IDictionary<string, T> dict)
		{
			if (dict == null || dict.Count <= 0)
			{
				return;
			}

			var pairs = new Dictionary<RedisKey, RedisValue>();
			foreach (var item in dict)
			{
				var ss = this.Serialize(item.Value);
				pairs.Add(item.Key, ss);
			}
			this.GetDatabase().StringSet(pairs.ToArray(), flags: CommandFlags.FireAndForget);
		}

		/// <summary>
		/// 读取数据
		/// </summary>
		public T Get<T>(string key)
		{
			var ss = this.GetDatabase().StringGet(key, CommandFlags.PreferSlave);
			return this.Deserialize<T>(ss);
		}

		public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
		{
			if (keys == null || keys.Count() <= 0)
			{
				return new Dictionary<string, T>();
			}
			var rKeys = keys.Select(key => (RedisKey)key).ToArray();
			var rVals = this.GetDatabase().StringGet(rKeys, CommandFlags.PreferSlave);

			var result = new Dictionary<string, T>();
			for (int i = 0; i < rVals.Length; i++)
			{
				if (rVals[i].IsNull)
				{
					continue;
				}
				var val = this.Deserialize<T>(rVals[i]);
				result.Add(rKeys[i].ToString(), val);
			}
			return result;
		}

		/// <summary>
		/// 删除数据
		/// </summary>
		public bool Remove(string key)
		{
			return this.GetDatabase().KeyDelete(key);
		}

		/// <summary>
		/// 批量删除数据
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public long RemoveAll(IEnumerable<string> keys)
		{
			var rKeys = keys.Select(key => (RedisKey)key).ToArray();
			return this.GetDatabase().KeyDelete(rKeys);
		}

		/// <summary> 
		/// 链表尾部元素(RPop)
		/// </summary> 
		/// <param name="listId">主键</param> 
		/// <returns>栈头部项</returns> 
		public T PopItemFromList<T>(string listId)
		{
			var val = this.GetDatabase().ListRightPop(listId);
			return this.Deserialize<T>(val);
		}

		/// <summary> 
		/// 插入链表尾部(RPush)
		/// </summary> 
		/// <param name="listId"></param> 
		/// <param name="item"></param> 
		public void PushItemToList<T>(string listId, T item)
		{
			if (string.IsNullOrWhiteSpace(listId))
			{
				return;
			}
			var ss = this.Serialize(item);
			this.GetDatabase().ListRightPush(listId, ss);
		}

		/// <summary> 
		/// 插入链表头部(lpush)
		/// </summary> 
		/// <param name="listId">主键</param> 
		/// <param name="item">项</param> 
		public void EnqueueItemOnList<T>(string listId, T item)
		{
			if (string.IsNullOrWhiteSpace(listId))
			{
				return;
			}
			var ss = this.Serialize(item);
			this.GetDatabase().ListLeftPush(listId, ss);
		}

		/// <summary>
		/// 插入链表头部(lpush)
		/// </summary>
		/// <param name="listId">对列名</param>
		/// <param name="item">值</param>
		/// <param name="invalidTime">过期时间</param>
		public void EnqueueItemOnList<T>(string listId, T item, DateTime invalidTime)
		{
			if (string.IsNullOrWhiteSpace(listId))
			{
				return;
			}
			var db = this.GetDatabase();
			var ss = this.Serialize(item);
			var count = db.ListLeftPush(listId, ss);
			db.KeyExpire(listId, invalidTime);
		}

		/// <summary>
		/// 批量插入链表头部(lpush)
		/// </summary>
		/// <param name="actions"></param>
		/// <returns></returns>
		public void BatchEnqueue<T>(string listId, IEnumerable<T> list, DateTime invalidTime)
		{
			if (string.IsNullOrWhiteSpace(listId) || list == null || list.Count() <= 0)
			{
				return;
			}
			var dict = new Dictionary<string, List<T>>();
			dict.Add(listId, list.ToList());
			BatchEnqueue(dict, invalidTime);
		}

		/// <summary>
		/// 批量插入链表头部(lpush)
		/// </summary>
		/// <param name="actions"></param>
		/// <returns></returns>
		public void BatchEnqueue<T>(IDictionary<string, List<T>> dict, DateTime invalidTime)
		{
			if (dict == null || dict.Count <= 0)
			{
				return;
			}

			var temp = new Dictionary<string, List<RedisValue>>();
			foreach (var listId in dict.Keys)
			{
				var values = new List<RedisValue>();
				foreach (var item in dict[listId])
				{
					var ss = this.Serialize(item);
					values.Add(ss);
				}
				values.Reverse();//反转链表顺序，防止顺讯错乱
				temp.Add(listId, values);
			}

			var db = this.GetDatabase();
			var batch = db.CreateBatch();
			foreach (var listId in temp.Keys)
			{
				batch.ListLeftPushAsync(listId, temp[listId].ToArray(), CommandFlags.FireAndForget);
				batch.KeyExpireAsync(listId, invalidTime, CommandFlags.FireAndForget);
			}
			batch.Execute();
		}

		/// <summary> 
		/// 从链表尾部取出值(rpop)
		/// </summary> 
		/// <param name="listId">主键</param> 
		/// <returns></returns> 
		public T DequeueItemFromList<T>(string listId)
		{
			var val = this.GetDatabase().ListRightPop(listId);
			return this.Deserialize<T>(val);
		}

		/// <summary>
		/// 从列表中删除所有
		/// </summary>
		/// <param name="listId"></param>
		public bool RemoveAllFromList(string listId)
		{
			if (string.IsNullOrWhiteSpace(listId))
			{
				return false;
			}
			return this.GetDatabase().KeyDelete(listId);
		}

		/// <summary>
		/// 删除列表中的项
		/// </summary>
		/// <param name="listId"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public long RemoveItemFromList(string listId, string value)
		{
			return this.GetDatabase().ListRemove(listId, value);
		}
		/// <summary>
		/// 删除列表中的项
		/// </summary>
		/// <param name="listId"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public long RemoveItemFromList<T>(string listId, T value)
		{
			if (string.IsNullOrWhiteSpace(listId))
			{
				return 0;
			}
			var ss = this.Serialize(value);
			return this.GetDatabase().ListRemove(listId, ss);
		}


		/// <summary>
		/// 获取列表中的所有项
		/// </summary>
		/// <param name="listId"></param>
		/// <returns></returns>
		public List<T> GetAllItemsFromList<T>(string listId)
		{
			return GetItemsFromList<T>(listId);
		}

		/// <summary>
		/// 获取列表中的项
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listId"></param>
		/// <param name="start">起始编号 从0开始</param>
		/// <param name="stop">结束编号 从0开始  -1代表最后一个 -2代表倒数第二个 以此来推</param>
		/// <returns></returns>
		public List<T> GetItemsFromList<T>(string listId, long start = 0, long stop = -1)
		{
			if (string.IsNullOrWhiteSpace(listId))
			{
				return new List<T>();
			}
			var values = this.GetDatabase().ListRange(listId, start, stop, CommandFlags.PreferSlave);
			var ret = new List<T>();
			foreach (var item in values)
			{
				var val = this.Deserialize<T>(item);
				ret.Add(val);
			}
			return ret;
		}

		/// <summary> 
		/// 链表中数据容量
		/// </summary> 
		/// <param name="listId">主键</param> 
		/// <returns></returns>
		public long GetListCount(string listId)
		{
			if (string.IsNullOrWhiteSpace(listId))
			{
				return 0;
			}
			return this.GetDatabase().ListLength(listId, CommandFlags.PreferSlave);
		}

		/// <summary>
		/// 整理List，保留部分元素
		/// </summary>
		/// <param name="listIds"></param>
		/// <param name="start"></param>
		/// <param name="stop"></param>
		public void BatchListTrim(IEnumerable<string> listIds, long start, long stop)
		{
			var db = this.GetDatabase();
			var batch = db.CreateBatch();
			foreach (var listId in listIds)
			{
				batch.ListTrimAsync(listId, start, stop, CommandFlags.FireAndForget);
			}
			batch.Execute();
		}

		/// <summary>
		/// 增加计数
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public long StringIncrement(string key, long value = 1)
		{
			var db = GetDatabase();
			return db.StringIncrement(key, value);
		}

		public void BatchIncrement(IEnumerable<string> keys, long value = 1)
		{
			var db = GetDatabase();
			var batch = db.CreateBatch();
			foreach (var key in keys)
			{
				batch.StringIncrementAsync(key, value, CommandFlags.FireAndForget);
			}
			batch.Execute();
		}

		/// <summary>
		/// 减少计数
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public long StringDecrement(string key, long value = 1)
		{
			var db = GetDatabase();
			return db.StringDecrement(key, value);
		}
		/// <summary>
		/// 检查key是否存在
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Exists(string key)
		{
			var db = GetDatabase();
			return db.KeyExists(key, CommandFlags.PreferSlave);
		}

		public bool SetContains(string key, string value)
		{
			var db = GetDatabase();
			return db.SetContains(key, value, CommandFlags.PreferSlave);
		}

		/// <summary>
		/// 哈希表批量增加计数
		/// </summary>
		/// <param name="keyValues"></param>
		/// <returns></returns>
		public Dictionary<string, long> HashIncrement(string hashKey, Dictionary<string, long> keyValues)
		{
			Dictionary<string, long> result = new Dictionary<string, long>();
			var db = GetDatabase();
			foreach (var keyValue in keyValues)
			{
				var r = db.HashIncrement(hashKey, keyValue.Key, keyValue.Value);
				result.Add(hashKey, r);
			}

			return result;
		}
		/// <summary>
		/// 删除hash中一个key值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="hashKey"></param>
		/// <returns></returns>
		public bool HashDelete(string key, string hashKey)
		{
			var db = GetDatabase();
			return db.HashDelete(key, hashKey);
		}
		/// <summary>
		/// 哈希表减少
		/// </summary>
		/// <param name="keyValues"></param>
		/// <returns></returns>
		public Dictionary<string, long> HashDecrement(string hashKey, Dictionary<string, long> keyValues)
		{
			if (keyValues == null || keyValues.Count == 0)
			{
				return null;
			}
			Dictionary<string, long> result = new Dictionary<string, long>();
			var db = GetDatabase();
			foreach (var keyValue in keyValues)
			{
				var r = db.HashDecrement(hashKey, keyValue.Key, keyValue.Value);
				result.Add(hashKey, r);
			}

			return result;

		}
		/// <summary>
		/// 添加哈希
		/// </summary>
		/// <param name="key"></param>
		/// <param name="keyValues"></param>
		public void SetHash(string key, Dictionary<string, string> keyValues, DateTime? invalidTime = null)
		{
			if (keyValues == null || keyValues.Count == 0)
			{
				return;
			}

			HashEntry[] hashEntries = new HashEntry[keyValues.Count];

			int index = 0;
			foreach (var keyValue in keyValues)
			{
				HashEntry hashEntry = new HashEntry(keyValue.Key, keyValue.Value);
				hashEntries[index] = hashEntry;
				index++;
			}

			var db = GetDatabase();
			db.HashSet(key, hashEntries);
			if (invalidTime != null)
			{
				db.KeyExpire(key, invalidTime);
			}
		}

		/// <summary>
		/// 获取哈希表
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public Dictionary<string, string> GetHash(string key)
		{
			var db = GetDatabase();
			var result = db.HashGetAll(key, CommandFlags.PreferSlave);

			var dic = new Dictionary<string, string>();
			for (int i = 0; i < result.Length; i++)
			{
				dic.Add(result[i].Name, (string)result[i].Value);
			}
			return dic;
		}
		/// <summary>
		/// 获取哈希表其中一个值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieldKey"></param>
		/// <returns></returns>
		public string GetHashField(string key, string fieldKey)
		{
			var db = GetDatabase();
			var result = db.HashGet(key, fieldKey, CommandFlags.PreferSlave);
			return result;
		}

		/// <summary>
		/// 获取哈希表其中一个值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="fieldKey"></param>
		/// <returns></returns>
		public async Task<string> GetHashFieldAsync(string key, string fieldKey)
		{
			var db = GetDatabase();
			var result = await db.HashGetAsync(key, fieldKey, CommandFlags.PreferSlave);
			return result;
		}
		/// <summary>
		/// 批量获取哈希
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public List<Dictionary<string, string>> BatchGetHash(IEnumerable<string> keys)
		{
			var db = GetDatabase();
			List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

			foreach (var key in keys)
			{
				var hash = db.HashGetAll(key, CommandFlags.PreferSlave);
				if (hash.Length > 0)
				{
					var dic = new Dictionary<string, string>();
					for (int i = 0; i < hash.Length; i++)
					{
						dic.Add(hash[i].Name, (string)hash[i].Value);
					}
					result.Add(dic);
				}

			}
			return result;
		}

		/// <summary>
		/// 批量获取哈希
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public async Task<List<Dictionary<string, string>>> BatchGetHashAsync(IEnumerable<string> keys)
		{
			var db = GetDatabase();
			List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

			foreach (var key in keys)
			{
				var hash = await db.HashGetAllAsync(key, CommandFlags.PreferSlave);
				if (hash.Length > 0)
				{
					var dic = new Dictionary<string, string>();
					for (int i = 0; i < hash.Length; i++)
					{
						dic.Add(hash[i].Name, (string)hash[i].Value);
					}

					result.Add(dic);
				}
			}
			return await Task.FromResult(result);
		}
		/// <summary>
		/// 哈希中的key是否存在
		/// </summary>
		/// <param name="key"></param>
		/// <param name="hashKey"></param>
		/// <returns></returns>
		public bool HashExists(string key, string hashKey)
		{
			var db = GetDatabase();
			return db.HashExists(key, hashKey, CommandFlags.PreferSlave);
		}
		/// <summary>
		/// 批量判断哈希key是否存在
		/// </summary>
		/// <param name="keyHashKeys"></param>
		/// <returns></returns>
		public List<bool> BatchHashExists(Dictionary<string, string> keyHashKeys)
		{
			var db = GetDatabase();
			List<bool> result = new List<bool>();
			foreach (var item in keyHashKeys)
			{
				result.Add(db.HashExists(item.Key, item.Value, CommandFlags.PreferSlave));
			}
			return result;
		}

		/// <summary>
		/// 查找Key
		/// </summary>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public List<string> SearchKeys(string pattern)
		{
			var keys = new HashSet<string>();

			var db = this.GetDatabase();
			int nextCursor = 0;
			do
			{
				var args = new List<object>();
				args.Add(nextCursor.ToString());
				args.Add("MATCH");
				args.Add(pattern);
				args.Add("COUNT");
				args.Add("1000");
				var redisResult = db.Execute("SCAN", args, CommandFlags.PreferSlave);
				var innerResult = (RedisResult[])redisResult;

				nextCursor = int.Parse((string)innerResult[0]);

				List<string> resultLines = ((string[])innerResult[1]).ToList();

				keys.UnionWith(resultLines);
			}
			while (nextCursor != 0);
			return keys.ToList();
		}


		/// <summary>
		/// 获取所有Key
		/// </summary>
		/// <returns></returns>
		public List<string> GetAllKey()
		{
			return this.SearchKeys("*");
		}

		/// <summary>
		///  BatchDequeue dequeue from right,得到的list需要reverse
		/// </summary>
		/// <param name="listID"></param>
		/// <param name="max_count"></param>
		/// <returns></returns>
		public List<T> BatchRDequeue<T>(string listID, int max_count)
		{
			if (string.IsNullOrWhiteSpace(listID) || max_count <= 0)
			{
				return new List<T>();
			}

			var tran = this.GetDatabase().CreateTransaction();

			var valueList = new List<Task<RedisValue>>();
			for (int i = 0; i < max_count; i++)
			{
				var val = tran.ListRightPopAsync(listID);
				valueList.Add(val);
			}
			tran.Execute();

			var result = new List<T>();
			foreach (var taskVal in valueList)
			{
				var val = this.Deserialize<T>(taskVal.Result);
				if (val != null)
				{
					result.Add(val);
				}
			}
			return result;
		}

		/// <summary>
		/// Redis并发锁，在对redis同一个item进行读写操作，存在并发情况时使用
		/// </summary>
		/// <param name="lockKey"></param>
		/// <param name="expiresTime"></param>
		/// <param name="action"></param>
		public bool ExcuteWithAcquireLock(string lockKey, TimeSpan expiresTime, Action action)
		{
			var token = Guid.NewGuid().ToString("N");
			var db = this.GetDatabase();
			if (!db.LockTake(lockKey, token, expiresTime))
			{
				return false;
			}
			try
			{
				action.Invoke();
			}
			finally
			{
				db.LockRelease(lockKey, token);
			}
			return true;
		}

		/// <summary>
		/// 有序集合增加排名
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="socre"></param>
		/// <returns></returns>
		public double SortedSetIncrement<T>(string key, T value, double socre)
		{
			var db = this.GetDatabase();
			var val = this.Serialize(value);
			return db.SortedSetIncrement(key, val, socre);
		}

		/// <summary>
		/// 删除有序集合中的一个值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool SortedSetRemove<T>(string key, T value)
		{
			var db = this.GetDatabase();
			var val = this.Serialize(value);
			return db.SortedSetRemove(key, val);
		}

		/// <summary>
		/// 按照排名获取有序集合，排名由低到高
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="start"></param>
		/// <param name="stop"></param>
		/// <param name="isDescending">是否反向排列</param>
		/// <returns></returns>
		public List<T> SortedSetRangeByRank<T>(string key, long start = 0, long stop = -1, bool isDescending = true)
		{
			var db = this.GetDatabase();
			var order = Order.Ascending;
			if (isDescending)
			{
				order = Order.Descending;
			}
			var values = db.SortedSetRangeByRank(key, start, stop, order, CommandFlags.PreferSlave);
			var ret = new List<T>();
			foreach (var item in values)
			{
				var val = this.Deserialize<T>(item);
				ret.Add(val);
			}
			return ret;
		}

		/// <summary>
		/// 序列化
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="dateTimeFormat"></param>
		/// <param name="isCamelCase"></param>
		/// <returns></returns>
		protected string Serialize(object obj)
		{
			var setting = JsonHelper.GetDefaultJsonSetting();
			setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
			return JsonHelper.Serialize(obj, setting);
		}

		/// <summary>
		/// 反序列化
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <param name="dateTimeFormat"></param>
		/// <param name="isCamelCase"></param>
		/// <returns></returns>
		protected T Deserialize<T>(string json)
		{
			var setting = JsonHelper.GetDefaultJsonSetting();
			setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
			return JsonHelper.Deserialize<T>(json, setting);
		}

		/// <summary>
		/// 获取数据库操作对象
		/// </summary>
		/// <returns></returns>
		protected IDatabase GetDatabase()
		{
			var context = this.GetLeastLoadedConn();
			return context.GetDatabase();
		}

		/// <summary>
		/// 获取可用连接，最小负载
		/// </summary>
		/// <returns></returns>
		private ConnectionMultiplexer GetLeastLoadedConn()
		{
			if (this.connectionPool.Count < this.poolSize)
			{
				lock (this.objLock)
				{
					if (this.connectionPool.Count < this.poolSize)
					{
						var config = ConfigurationOptions.Parse(this.connectString, true);
						config.AbortOnConnectFail = false;
						config.KeepAlive = 60;
						config.SyncTimeout = Math.Max(config.SyncTimeout, 10000);//默认1秒，最小设置为10秒
						config.SocketManager = new SocketManager();//每个socketmanager维护了线程池，多建立几个提高并发
						config.CommandMap = CommandMap.Create(new HashSet<string> { "SUBSCRIBE" }, false);//禁用订阅发布，否则会多建立一条无用连接
						var conn = ConnectionMultiplexer.Connect(config);
						this.connectionPool.Add(conn);
						return conn;
					}
				}
			}

			//最小负载
			var min = this.connectionPool.OrderBy(m => m.GetCounters().TotalOutstanding).First();
			return min;
		}
	}
}
