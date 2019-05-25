using System;

namespace IQF.Framework.Cache
{
	/// <summary>
	/// 内存级缓存声明
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class MemCacheAttribute : Attribute
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="expireSeconds">缓存时间，单位：秒</param>
		/// <param name="cacheName">缓存容器名称，为空使用默认缓存容器</param>
		/// <param name="cacheMethodNameForDel">设置缓存方法名用于删除缓存</param>
		/// <param name="ignoreDefaultValue">是否忽略返回值的默认值</param>
		public MemCacheAttribute(int expireSeconds = 5 * 60, string cacheName = null, string cacheMethodNameForDel = null, bool ignoreDefaultValue = true)
		{
			this.ExpireSeconds = expireSeconds;
			this.CacheName = cacheName;
			this.CacheMethodNameForDel = cacheMethodNameForDel;
			this.IgnoreDefaultValue = ignoreDefaultValue;
		}

		/// <summary>
		/// 缓存时间，单位：秒
		/// </summary>
		public int ExpireSeconds { get; set; }

		/// <summary>
		/// 缓存名称，为空使用默认缓存
		/// </summary>
		public string CacheName { get; set; }

		/// <summary>
		/// 设置缓存方法名用于删除缓存
		/// </summary>
		public string CacheMethodNameForDel { get; set; }

		/// <summary>
		/// 是否忽略返回值的默认值
		/// </summary>
		public bool IgnoreDefaultValue { get; set; }
	}
}
