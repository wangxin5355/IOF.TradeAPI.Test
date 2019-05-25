using Castle.DynamicProxy;
using IQF.Framework.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace IQF.Framework.Cache
{
	/// <summary>
	/// 内存缓存拦截器
	/// </summary>
	internal class MemCacheInterceptor : IInterceptor
	{
		private readonly static Dictionary<string, MemoryCache> cacheContainer = new Dictionary<string, MemoryCache>();

		private readonly static string defaultCacheName = Guid.NewGuid().ToString("N");

		private readonly static object objLock = new object();

		private readonly MemCacheAttribute memCacheAttribute;

		private readonly MemoryCache memoryCache;

		private string keyPrefix;

		public MemCacheInterceptor(MemCacheAttribute memCacheAttribute)
		{
			this.memoryCache = this.GetOrAddCache(memCacheAttribute.CacheName);
			this.memCacheAttribute = memCacheAttribute;
		}

		public void Intercept(IInvocation invocation)
		{
			if (string.IsNullOrWhiteSpace(this.keyPrefix))//性能优化用
			{
				this.keyPrefix = string.Concat(invocation.TargetType.FullName, ".", invocation.MethodInvocationTarget.Name);
			}
			if (string.IsNullOrWhiteSpace(this.memCacheAttribute.CacheMethodNameForDel))//添加缓存，并发时单线程执行
			{
				var key = this.GetKey(this.keyPrefix, invocation.Arguments);
				var cache = this.memoryCache.Get(key);
				if (cache != null)
				{
					invocation.ReturnValue = cache;
					return;
				}
				lock (String.Intern(key))
				{
					cache = this.memoryCache.Get(key);
					if (cache != null)
					{
						invocation.ReturnValue = cache;
						return;
					}
					invocation.Proceed();
					if (this.memCacheAttribute.IgnoreDefaultValue)//不缓存默认值
					{
						var defaultV = this.GetDefaultV(invocation.Method.ReturnType);
						if ((defaultV == null && invocation.ReturnValue == null) ||
							(defaultV != null && defaultV.Equals(invocation.ReturnValue)))
						{
							return;
						}
					}
					this.memoryCache.Set(key, invocation.ReturnValue, new TimeSpan(0, 0, this.memCacheAttribute.ExpireSeconds));
				}
			}
			else//删除缓存，无需单线程
			{
				invocation.Proceed();
				var prefix = string.Concat(invocation.TargetType.FullName, ".", this.memCacheAttribute.CacheMethodNameForDel);
				var key = this.GetKey(prefix, invocation.Arguments);
				this.memoryCache.Remove(key);
			}
		}

		private object GetDefaultV(Type returnType)
		{
			if (returnType == null)
			{
				return null;
			}
			if (returnType.IsValueType)
			{
				return Activator.CreateInstance(returnType);
			}
			return null;
		}

		private string GetKey(string prefix, object[] args)
		{
			var key = new StringBuilder(prefix);
			foreach (var item in args)
			{
				if (item != null)
				{
					var argType = item.GetType();
					if (argType.IsPrimitive || argType == typeof(string))
					{
						key.Append(item.ToString());
					}
					else
					{
						key.Append(JsonHelper.Serialize(item));
					}
				}
				key.Append('.');
			}
			return key.ToString();
		}

		private MemoryCache GetOrAddCache(string cacheName)
		{
			if (string.IsNullOrWhiteSpace(cacheName))
			{
				cacheName = defaultCacheName;
			}
			lock (objLock)
			{
				if (!cacheContainer.ContainsKey(cacheName))
				{
					cacheContainer.Add(cacheName, new MemoryCache(Options.Create(new MemoryCacheOptions())));
				}
				return cacheContainer[cacheName];
			}
		}
	}
}
