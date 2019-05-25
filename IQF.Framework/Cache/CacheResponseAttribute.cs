using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using IQF.Framework.Serialization;

namespace IQF.Framework.Cache
{
	/// <summary>
	/// MVC框架提供了一个缓存（ResponseCache），支持客户端缓存，仅仅支持GET和HEAD
	/// GET和HEAD方法可以使用ResponseCache，POST只能使用CacheResponse，CacheResponse也支持GET和HEAD
	/// 缓存action响应结果到内存中
	/// 缓存策略：缓存的主键为action参数拼接的字符串，参数的变化将导致重新请求
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class CacheResponseAttribute : ActionFilterAttribute
	{
		private readonly static IMemoryCache memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

		private readonly int miliSeconds = 30 * 1000;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="miliSeconds">缓存到期时间，单位：毫秒</param>
		public CacheResponseAttribute(int miliSeconds = 30 * 1000)
		{
			this.miliSeconds = miliSeconds;
		}

		public bool Ignore { get; set; }

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (this.miliSeconds <= 0)
			{
				base.OnActionExecuting(context);
				return;
			}
			var dict = context.ActionArguments.OrderBy(o => o.Key);
			var cacheKey = context.HttpContext.Request.Path.Value + JsonHelper.Serialize(dict);
			context.HttpContext.Items["__private_cache_key__"] = cacheKey;

			var ret = memoryCache.Get(cacheKey) as IActionResult;
			if (ret == null)
			{
				base.OnActionExecuting(context);
				return;
			}
			context.Result = ret;

			base.OnActionExecuting(context);
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			if (this.miliSeconds <= 0)
			{
				base.OnActionExecuted(context);
				return;
			}
			if (context.Result == null)
			{
				base.OnActionExecuted(context);
				return;
			}
			var cacheKey = context.HttpContext.Items["__private_cache_key__"].SafeToString();
			if (!string.IsNullOrWhiteSpace(cacheKey))
			{
				memoryCache.Set(cacheKey, context.Result, DateTime.Now.AddMilliseconds(this.miliSeconds));
			}
			base.OnActionExecuted(context);
		}
	}
}


