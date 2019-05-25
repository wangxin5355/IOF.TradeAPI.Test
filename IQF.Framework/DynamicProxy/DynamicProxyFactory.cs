using Castle.DynamicProxy;
using IQF.Framework.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IQF.Framework.DynamicProxy
{
	/// <summary>
	/// 代理工厂
	/// </summary>
	internal class DynamicProxyFactory : IDynamicProxyFactory
	{
		private readonly IDistributedCacheFactory distributedCacheFactory;

		public DynamicProxyFactory(IDistributedCacheFactory distributedCacheFactory)
		{
			this.distributedCacheFactory = distributedCacheFactory;
		}

		/// <summary>
		/// 创建代理
		/// </summary>
		/// <param name="classType">需要代理的类</param>
		/// <param name="constructorArguments">需要代理的类的构造函数参数</param>
		/// <returns></returns>
		public TClass Create<TClass>(params object[] constructorArguments)
			where TClass : class
		{
			var ret = this.Create(typeof(TClass), constructorArguments) as TClass;
			return ret;
		}

		/// <summary>
		/// 创建代理
		/// </summary>
		/// <typeparam name="TClass">需要代理的类</typeparam>
		/// <param name="constructorArguments">需要代理的类的构造函数参数</param>
		/// <returns></returns>
		public object Create(Type classType, params object[] constructorArguments)
		{
			var generator = new ProxyGenerator();
			var option = new ProxyGenerationOptions() { Selector = new InnerInterceptorSelector(distributedCacheFactory) };
			var ret = generator.CreateClassProxy(classType, option, constructorArguments);
			return ret;
		}

		/// <summary>
		/// 创建代理，使用IOC容器进行初始化
		/// </summary>
		/// <param name="type">需要代理的类</param>
		/// <param name="serviceProvider">容器</param>
		/// <returns></returns>
		public object Create(Type type, IServiceProvider serviceProvider)
		{
			if (type == null || serviceProvider == null)
			{
				return null;
			}
			var constructor = type.GetConstructors().FirstOrDefault();
			if (constructor == null)
			{
				return this.Create(type);
			}
			var parameters = new List<object>();
			foreach (var parameter in constructor.GetParameters())
			{
				var service = serviceProvider.GetService(parameter.ParameterType);
				parameters.Add(service);
			}
			return this.Create(type, parameters.ToArray());
		}
	}

	/// <summary>
	/// 内部拦截选择器
	/// </summary>
	internal class InnerInterceptorSelector : IInterceptorSelector
	{
		private readonly IDistributedCacheFactory distributedCacheFactory;

		public InnerInterceptorSelector(IDistributedCacheFactory distributedCacheFactory)
		{
			this.distributedCacheFactory = distributedCacheFactory;
		}

		public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
		{
			if (interceptors != null && interceptors.Length > 0)
			{
				return interceptors;
			}
			var result = new List<IInterceptor>();
			foreach (var attribute in method.GetCustomAttributes())
			{
				var interceptor = this.Create(attribute);
				if (interceptor != null)
				{
					result.Add(interceptor);
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// 高并发时会重复创建
		/// </summary>
		/// <param name="attribute"></param>
		/// <returns></returns>
		private IInterceptor Create(Attribute attribute)
		{
			if (attribute == null)
			{
				return null;
			}
			if (attribute is MemCacheAttribute)
			{
				return new MemCacheInterceptor(attribute as MemCacheAttribute);
			}
			else if (attribute is DistributedCacheAttribute)
			{
				return new DistributedCacheInterceptor(distributedCacheFactory, attribute as DistributedCacheAttribute);
			}
			return null;
		}
	}
}
