namespace IQF.Framework.IModules
{
	/// <summary>
	/// 数据访问配置信息
	/// </summary>
	public interface IDataConfiguration
	{
		/// <summary>
		/// 获取连接串
		/// </summary>
		/// <param name="connStrName"></param>
		/// <returns></returns>
		string GetDbConnStr(string connStrName);

		/// <summary>
		/// 获取分布式缓存连接串
		/// </summary>
		/// <param name="connStrName"></param>
		/// <returns></returns>
		string GetDistributedCacheConnStr(string connStrName);
	}
}
