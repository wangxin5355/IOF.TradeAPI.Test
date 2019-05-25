using IQF.Framework.Cache;
using IQF.Framework.Dao;
using IQF.Framework.DynamicProxy;
using IQF.Framework.Framework;
using IQF.Framework.IModules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IQF.Framework
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// 动态添加服务到IOC容器中
		/// </summary>
		/// <param name="services"></param>
		/// <param name="interfaceTypes">需要动态添加到容器中的接口（单例模式）</param>
		public static void AddServiceDynamic(this IServiceCollection services, params Type[] interfaceTypes)
		{
			services.AddSingleton<IDataConfiguration, DataConfiguration>();
			services.AddSingleton<IDistributedCacheFactory, DistributedCacheFactory>();
			services.AddSingleton<IDbSessionFactory, DbSessionFactory>();

			services.AddServiceDynamic("IQF*.dll", interfaceTypes);
		}

		/// <summary>
		/// 动态添加服务到IOC容器中
		/// </summary>
		/// <param name="services"></param>
		/// <param name="interfaceTypes">需要动态添加到容器中的接口（单例模式）</param>
		public static void AddServiceDynamic(this IServiceCollection services, string assemblySearchPattern = "IQF*.dll", params Type[] interfaceTypes)
		{
			services.AddSingleton<IDynamicProxyFactory, DynamicProxyFactory>();
			//动态添加服务
			var assemblies = GetAllAssembly(assemblySearchPattern);
			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes().Where(t => t.IsClass);
				foreach (var type in types)
				{
					var interfaces = type.GetInterfaces();
					if (interfaces.Contains(typeof(IProxyService)))
					{
						var proxyType = interfaces.FirstOrDefault(t => t != typeof(IProxyService));
						if (proxyType != null)
						{
							//使用动态代理的接口
							services.AddSingleton(proxyType, (serviceProvider) => CreateProxy(type, serviceProvider));
						}
					}
					else
					{
						//用户自定义接口
						var userType = interfaces.FirstOrDefault(a => interfaceTypes.Contains(a));
						if (userType != null)
						{
							services.AddSingleton(userType, type);
						}
					}
				}
			}
		}

		private static object CreateProxy(Type type, IServiceProvider serviceProvider)
		{
			var proxyFac = serviceProvider.GetService<IDynamicProxyFactory>();
			if (proxyFac == null)
			{
				return null;
			}
			return proxyFac.Create(type, serviceProvider);
		}

		private static List<Assembly> GetAllAssembly(string assemblySearchPattern)
		{
			var dir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase.ToString()).LocalPath);
			var paths = Directory.GetFiles(dir, assemblySearchPattern, SearchOption.TopDirectoryOnly);

			var result = new List<Assembly>();
			foreach (var path in paths)
			{
				var assembly = Assembly.LoadFrom(path);
				result.Add(assembly);
			}
			return result;
		}
	}
}
