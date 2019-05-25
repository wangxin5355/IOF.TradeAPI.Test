using IQF.Framework.Cache;
using IQF.Framework.IModules;
using System;
using System.Collections.Generic;

namespace IQF.Framework.Framework
{
	public class DistributedCacheFactory : IDistributedCacheFactory
	{
		private readonly static Dictionary<DistributedCacheName, IDistributedCache> cacheContainer = new Dictionary<DistributedCacheName, IDistributedCache>();

		private readonly object objLock = new object();

		private readonly IDataConfiguration dataConfiguration;

		public DistributedCacheFactory(IDataConfiguration dataConfiguration)
		{
			this.dataConfiguration = dataConfiguration;
		}

		public IDistributedCache Create(DistributedCacheName distributedCacheName)
		{
			var connStr = this.dataConfiguration.GetDistributedCacheConnStr(distributedCacheName.ToString());
			if (string.IsNullOrWhiteSpace(connStr))
			{
				throw new ApplicationException($"未找到{distributedCacheName.ToString()}的连接配置");
			}
			lock (objLock)
			{
				if (!cacheContainer.ContainsKey(distributedCacheName))
				{
					cacheContainer.Add(distributedCacheName, this.CreateInstance(connStr));
				}
				return cacheContainer[distributedCacheName];
			}
		}

		/// <summary>
		/// 创建实例
		/// </summary>
		/// <param name="connStr"></param>
		/// <returns></returns>
		protected virtual IDistributedCache CreateInstance(string connStr)
		{
			return new RedisHelper(connStr);
		}
	}
}
