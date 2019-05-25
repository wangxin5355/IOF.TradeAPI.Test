using System;

namespace IQF.Framework.Cache
{
	/// <summary>
	/// 分布式缓存声明
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class DistributedCacheAttribute : Attribute
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="cacheName">连接字符串名称</param>
		/// <param name="prefixKey">缓存主键前缀</param>
		/// <param name="expireSeconds">缓存时间，单位：秒</param>
		/// <param name="cacheName">缓存关联的方法名，需要删除缓存时填写</param>
		/// <param name="ignoreDefaultValue">是否忽略返回值的默认值</param>
		public DistributedCacheAttribute(DistributedCacheName cacheName, string prefixKey, int expireSeconds = 5 * 60, bool isRemoveCache = false, bool ignoreDefaultValue = true)
		{
			this.CacheName = cacheName;
			this.PrefixKey = prefixKey;
			this.ExpireSeconds = expireSeconds;
			this.IsRemoveCache = isRemoveCache;
			this.IgnoreDefaultValue = ignoreDefaultValue;
		}

		/// <summary>
		/// 缓存容器名称，为空使用默认容器
		/// </summary>
		public DistributedCacheName CacheName { get; set; }

		/// <summary>
		/// 缓存主键前缀
		/// </summary>
		public string PrefixKey { get; set; }

		/// <summary>
		/// 缓存时间，单位：秒
		/// </summary>
		public int ExpireSeconds { get; set; }

		/// <summary>
		/// 是否删除缓存
		/// </summary>
		public bool IsRemoveCache { get; set; }

		/// <summary>
		/// 是否忽略返回值的默认值
		/// </summary>
		public bool IgnoreDefaultValue { get; set; }
	}
}
