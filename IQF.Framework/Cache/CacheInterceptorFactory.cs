using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Text;

namespace IQF.Framework.Cache
{
	/// <summary>
	/// 缓存拦截器工厂，线程安全，并发仅执行一次
	/// </summary>
	[Obsolete("使用动态代理代替，MemCacheAttribute")]
	public static class CacheInterceptorFactory
	{
		/// <summary>
		/// 创建内存缓存拦截器
		/// </summary>
		/// <param name="@delegate">被拦截的方法</param>
		/// <param name="expirateSeconds">缓存到期秒数，单位：秒</param>
		/// <returns>内存缓存拦截器</returns>
		public static ICacheInterceptor Create<TResult>(Func<TResult> @delegate, int expirateSeconds = 60 * 60)
		{
			return new MemoryCacheInterceptor(@delegate, expirateSeconds);
		}

		/// <summary>
		/// 创建内存缓存拦截器
		/// </summary>
		/// <typeparam name="TArg">被拦截方法第一个参数的类型</typeparam>
		/// <param name="@delegate">被拦截的方法</param>
		/// <param name="expirateSeconds">缓存到期秒数，单位：秒</param>
		/// <returns>内存缓存拦截器</returns>
		public static ICacheInterceptor Create<TArg, TResult>(Func<TArg, TResult> @delegate, int expirateSeconds = 60 * 60)
		{
			return new MemoryCacheInterceptor(@delegate, expirateSeconds);
		}

		/// <summary>
		/// 创建内存缓存拦截器
		/// </summary>
		/// <typeparam name="TArg1">被拦截方法第一个参数的类型</typeparam>
		/// <typeparam name="TArg2">被拦截方法第二个参数的类型</typeparam>
		/// <param name="@delegate">被拦截的方法</param>
		/// <param name="expirateSeconds">缓存到期秒数，单位：秒</param>
		/// <returns>内存缓存拦截器</returns>
		public static ICacheInterceptor Create<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> @delegate, int expirateSeconds = 60 * 60)
		{
			return new MemoryCacheInterceptor(@delegate, expirateSeconds);
		}

		/// <summary>
		/// 创建内存缓存拦截器
		/// </summary>
		/// <typeparam name="TArg1">被拦截方法第一个参数的类型</typeparam>
		/// <typeparam name="TArg2">被拦截方法第二个参数的类型</typeparam>
		/// <typeparam name="TArg3">被拦截方法第三个参数的类型</typeparam>
		/// <param name="@delegate">被拦截的方法</param>
		/// <param name="expirateSeconds">缓存到期秒数，单位：秒</param>
		/// <returns>内存缓存拦截器</returns>
		public static ICacheInterceptor Create<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, TResult> @delegate, int expirateSeconds = 60 * 60)
		{
			return new MemoryCacheInterceptor(@delegate, expirateSeconds);
		}
	}

	/// <summary>
	/// 缓存拦截器
	/// </summary>
	public interface ICacheInterceptor : IMethodInterceptor
	{
		/// <summary>
		/// 缓存容器
		/// </summary>
		object CacheContainer { get; }

		/// <summary>
		/// 缓存键前缀：无参函数前缀即为缓存键，有参函数需要拼接参数才能获取缓存键
		/// </summary>
		string CacheKeyPrefix { get; }

		/// <summary>
		/// 缓存到期时间
		/// </summary>
		DateTime AbsoluteExpirate { get; set; }

		/// <summary>
		/// 获取缓存键：无参函数前缀即为缓存键，有参函数需要拼接参数才能获取缓存键
		/// </summary>
		/// <returns>缓存键</returns>
		string GetCacheKey(params object[] args);

		/// <summary>
		/// 删除缓存
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		bool Remove(params object[] args);
	}

	/// <summary>
	/// 内存缓存拦截器
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class MemoryCacheInterceptor : MethodInterceptor, ICacheInterceptor
	{
		private static IMemoryCache defaultCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

		private readonly int expirateSeconds = 60 * 60;

		/// <summary>
		/// 内存缓存拦截器构造函数
		/// </summary>
		/// <param name="delegate">拦截的方法</param>
		/// <param name="expirateSeconds">缓存时间 单位：秒</param>
		/// <param name="userDefaultContainer">是否使用默认缓存容器</param>
		public MemoryCacheInterceptor(Delegate @delegate, int expirateSeconds = 60 * 60, bool userDefaultContainer = true)
			: base(@delegate)
		{
			this.CacheKeyPrefix = Guid.NewGuid().ToString("N");
			if (userDefaultContainer)
			{
				this.CacheContainer = defaultCache;
			}
			else
			{
				this.CacheContainer = new MemoryCache(Options.Create(new MemoryCacheOptions()));
			}
			this.expirateSeconds = expirateSeconds;
			this.AbsoluteExpirate = DateTime.Now.AddSeconds(expirateSeconds);
		}

		/// <summary>
		/// 缓存容器
		/// </summary>
		public object CacheContainer { get; private set; }

		/// <summary>
		/// 缓存键前缀
		/// </summary>
		public string CacheKeyPrefix { get; }

		/// <summary>
		/// 缓存到期时间
		/// </summary>
		public DateTime AbsoluteExpirate { get; set; }

		/// <summary>
		/// 获取缓存键
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual string GetCacheKey(params object[] args)
		{
			if (args == null || args.Length <= 0)
			{
				return this.CacheKeyPrefix;
			}
			var sb = new StringBuilder(this.CacheKeyPrefix);
			foreach (var arg in args)
			{
				sb.AppendFormat(".", arg.SafeToString());
			}
			return sb.ToString();
		}

		public bool Remove(params object[] args)
		{
			var container = CacheContainer as IMemoryCache;
			if (container == null)
			{
				return false;
			}
			var key = this.GetCacheKey(args);
			container.Remove(key);
			return true;
		}

		protected override ResultInfo AfterExecute(object result, params object[] args)
		{
			if (!Check(result))
			{
				return new ResultInfo(-1, "执行后返回结果检查失败");
			}
			var container = CacheContainer as IMemoryCache;
			if (container == null)
			{
				return new ResultInfo(-1, "内存容器为空");
			}
			var key = this.GetCacheKey(args);
			this.AbsoluteExpirate = DateTime.Now.AddSeconds(this.expirateSeconds);
			container.Set(key, result, this.AbsoluteExpirate);
			return new ResultInfo();
		}

		protected override ResultInfo<object> BeforeExecute(params object[] args)
		{
			var container = CacheContainer as IMemoryCache;
			if (container == null)
			{
				return new ResultInfo<object>(-1, "CurrMemCache为空");
			}
			var key = this.GetCacheKey(args);
			var cache = container.Get(key);
			if (Check(cache))
			{
				return new ResultInfo<object>(0, string.Empty, cache);
			}
			return new ResultInfo<object>(-1, "缓存检查失败");
		}

		/// <summary>
		/// 检查返回结果是否可以当做有效缓存
		/// </summary>
		/// <param name="cache"></param>
		/// <returns></returns>
		protected virtual bool Check(object cache)
		{
			if (cache == null)
			{
				return false;
			}

			var enumerable = cache as IEnumerable;
			if (enumerable != null)
			{
				while (enumerable.GetEnumerator().MoveNext())
				{
					return true;
				}
				return false;
			}

			var ret = cache as IResultInfo;
			if (ret != null && ret.IsError())
			{
				return false;
			}
			return true;
		}
	}

	/// <summary>
	/// 方法拦截器
	/// </summary>
	public interface IMethodInterceptor
	{
		/// <summary>
		/// 执行次数
		/// </summary>
		int ExecTimes { get; }

		/// <summary>
		/// 执行拦截
		/// 先执行拦截操作，符合条件执行返回
		/// 不符合条件则调用被拦截方法，进一步处理被拦截方法的返回结果
		/// </summary>
		/// <typeparam name="TRet">被拦截方法的执行结果类型</typeparam>
		/// <param name="defaultVal">拦截失败或出错时的默认结果</param>
		/// <param name="args">被拦截方法的参数列表</param>
		/// <returns>执行结果</returns>
		TResult Execute<TResult>(TResult defaultVal = default(TResult), params object[] args);
	}

	/// <summary>
	/// 方法拦截器
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal abstract class MethodInterceptor : IMethodInterceptor
	{
		protected readonly Delegate @delegate = null;

		protected int execTimes = 0;

		private object objLock = new object();

		public MethodInterceptor(Delegate @delegate)
		{
			this.@delegate = @delegate;
		}

		/// <summary>
		/// 执行前调用
		/// </summary>
		/// <returns></returns>
		protected abstract ResultInfo<object> BeforeExecute(params object[] args);

		public int ExecTimes
		{
			get
			{
				return this.execTimes;
			}
		}

		/// <summary>
		/// 执行拦截
		/// </summary>
		/// <returns></returns>
		public TResult Execute<TResult>(TResult defaultVal = default(TResult), params object[] args)
		{
			var ret = this.BeforeExecute();
			if (!ret.IsError() && ret.Data != null && ret.Data is TResult)
			{
				return (TResult)ret.Data;
			}

			lock (objLock)//高并发时单线程执行
			{
				ret = this.BeforeExecute();
				if (!ret.IsError() && ret.Data != null && ret.Data is TResult)
				{
					return (TResult)ret.Data;
				}

				var val = this.@delegate.DynamicInvoke(args);
				this.execTimes++;
				if (val == null || !(val is TResult))
				{
					return defaultVal;
				}
				this.AfterExecute(val);
				return (TResult)val;
			}
		}

		/// <summary>
		/// 执行方法后调用
		/// </summary>
		/// <param name="result">执行结果</param>
		protected abstract ResultInfo AfterExecute(object result, params object[] args);
	}
}
