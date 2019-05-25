using Castle.DynamicProxy;
using IQF.Framework.Serialization;
using System;
using System.Text;

namespace IQF.Framework.Cache
{
	/// <summary>
	/// 分布式缓存拦截器
	/// </summary>
	internal class DistributedCacheInterceptor : IInterceptor
	{
		private readonly IDistributedCache cache;
		private readonly DistributedCacheAttribute cacheAttribute;

		public DistributedCacheInterceptor(IDistributedCacheFactory cacheFactory, DistributedCacheAttribute cacheAttribute)
		{
			this.cache = cacheFactory.Create(cacheAttribute.CacheName);
			this.cacheAttribute = cacheAttribute;
		}

		public void Intercept(IInvocation invocation)
		{
			var key = this.GetKey(this.cacheAttribute.PrefixKey, invocation.Arguments);
			if (this.cacheAttribute.IsRemoveCache)
			{
				invocation.Proceed();
				this.cache.Remove(key);
			}
			else
			{
				var cache = this.cache.Get<object>(key);
				if (cache != null && cache.GetType() == invocation.MethodInvocationTarget.ReturnType)
				{
					invocation.ReturnValue = cache;
					return;
				}
				invocation.Proceed();
				if (this.cacheAttribute.IgnoreDefaultValue)//不缓存默认值
				{
					var defaultV = this.GetDefaultV(invocation.Method.ReturnType);
					if ((defaultV == null && invocation.ReturnValue == null) ||
						(defaultV != null && defaultV.Equals(invocation.ReturnValue)))
					{
						return;
					}
				}
				this.cache.Set(key, invocation.ReturnValue, new TimeSpan(0, 0, this.cacheAttribute.ExpireSeconds));
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

		private string GetKey(string prefixKey, object[] args)
		{
			var key = new StringBuilder(prefixKey);
			foreach (var item in args)
			{
				var val = string.Empty;
				if (item != null)
				{
					var argType = item.GetType();
					if (argType.IsPrimitive || argType == typeof(string))
					{
						val = item.ToString();
					}
					else
					{
						val = JsonHelper.Serialize(item);
					}
				}
				key.Append(":");
				key.Append(val);
			}
			return key.ToString();
		}
	}
}
