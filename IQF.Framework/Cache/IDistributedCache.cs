using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IQF.Framework.Cache
{
	public interface IDistributedCache
	{
		void BatchEnqueue<T>(IDictionary<string, List<T>> dict, DateTime invalidTime);
		void BatchEnqueue<T>(string listId, IEnumerable<T> list, DateTime invalidTime);
		List<Dictionary<string, string>> BatchGetHash(IEnumerable<string> keys);
		Task<List<Dictionary<string, string>>> BatchGetHashAsync(IEnumerable<string> keys);
		List<bool> BatchHashExists(Dictionary<string, string> keyHashKeys);
		List<T> BatchRDequeue<T>(string listID, int max_count);
		void BatchSet<T>(IDictionary<string, T> dict);
		T DequeueItemFromList<T>(string listId);
		void EnqueueItemOnList<T>(string listId, T item);
		void EnqueueItemOnList<T>(string listId, T item, DateTime invalidTime);
		bool ExcuteWithAcquireLock(string lockKey, TimeSpan expiresTime, Action action);
		bool Exists(string key);
		T Get<T>(string key);
		IDictionary<string, T> GetAll<T>(IEnumerable<string> keys);
		List<T> GetAllItemsFromList<T>(string listId);
		List<string> GetAllKey();
		Dictionary<string, string> GetHash(string key);
		string GetHashField(string key, string fieldKey);
		Task<string> GetHashFieldAsync(string key, string fieldKey);
		List<T> GetItemsFromList<T>(string listId, long start = 0, long stop = -1);
		long GetListCount(string listId);
		Dictionary<string, long> HashDecrement(string hashKey, Dictionary<string, long> keyValues);
		bool HashDelete(string key, string hashKey);
		bool HashExists(string key, string hashKey);
		Dictionary<string, long> HashIncrement(string hashKey, Dictionary<string, long> keyValues);
		T PopItemFromList<T>(string listId);
		void PushItemToList<T>(string listId, T item);
		bool Remove(string key);
		long RemoveAll(IEnumerable<string> keys);
		bool RemoveAllFromList(string listId);
		long RemoveItemFromList(string listId, string value);
		long RemoveItemFromList<T>(string listId, T value);
		List<string> SearchKeys(string pattern);
		bool Set<T>(string key, T val);
		bool Set<T>(string key, T val, DateTime expiresAt);
		bool Set<T>(string key, T val, TimeSpan expiresIn);
		bool SetContains(string key, string value);
		void SetHash(string key, Dictionary<string, string> keyValues, DateTime? invalidTime = null);
		long StringDecrement(string key, long value = 1);
		long StringIncrement(string key, long value = 1);
		void KeyExpire(IEnumerable<string> keys, DateTime expiresAt);
		/// <summary>
		/// 保留部分元素
		/// </summary>
		/// <param name="listIds"></param>
		/// <param name="start">起始索引</param>
		/// <param name="stop">结束索引</param>
		void BatchListTrim(IEnumerable<string> listIds, long start, long stop);
		void BatchIncrement(IEnumerable<string> keys, long value = 1);
		/// <summary>
		/// 有序集合增加排名
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="socre"></param>
		/// <returns></returns>
		double SortedSetIncrement<T>(string key, T value, double socre);
		/// <summary>
		/// 删除有序集合中的一个值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		bool SortedSetRemove<T>(string key, T value);
		/// <summary>
		/// 按照排名获取有序集合，排名由低到高
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="start"></param>
		/// <param name="stop"></param>
		/// <param name="isDescending">是否反向排列</param>
		/// <returns></returns>
		List<T> SortedSetRangeByRank<T>(string key, long start = 0, long stop = -1, bool isDescending = true);
	}
}