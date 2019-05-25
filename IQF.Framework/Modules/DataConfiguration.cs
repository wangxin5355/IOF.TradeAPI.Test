using IQF.Framework.IModules;
using Microsoft.Extensions.Configuration;

namespace IQF.Framework.Framework
{
	public class DataConfiguration : IDataConfiguration
	{
		private readonly IConfiguration configuration;

		public DataConfiguration(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public string GetDbConnStr(string connStrName)
		{
			return this.configuration.GetConnectionString(connStrName);
		}

		public string GetDistributedCacheConnStr(string connStrName)
		{
			return GetCustomCfg("redisConnectionStrings", connStrName);
		}

		/// <summary>
		/// 获取自定义配置项
		/// </summary>
		/// <param name="sectionKey"></param>
		/// <param name="key"></param>
		/// <param name="defaultVal"></param>
		/// <returns></returns>
		private string GetCustomCfg(string sectionKey, string key, string defaultVal = null)
		{
			if (this.configuration == null ||
				string.IsNullOrWhiteSpace(sectionKey) ||
				string.IsNullOrWhiteSpace(key))
			{
				return defaultVal;
			}
			var section = this.configuration.GetSection(sectionKey);
			if (section.Exists())
			{
				return section[key];
			}
			return defaultVal;
		}
	}
}
