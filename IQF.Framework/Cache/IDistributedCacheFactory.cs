namespace IQF.Framework.Cache
{
	public interface IDistributedCacheFactory
	{
		IDistributedCache Create(DistributedCacheName distributedCacheName);
	}

	/// <summary>
	/// 分布式缓存连接名
	/// </summary>
	public enum DistributedCacheName
	{
		/// <summary>
		/// 行情
		/// </summary>
		Redis_IQFQuote = 1,
		/// <summary>
		/// 用户
		/// </summary>
		Redis_IQFUser = 2,
		/// <summary>
		/// 交易
		/// </summary>
		Redis_IQFTrade = 3,
		/// <summary>
		/// 推送
		/// </summary>
		Redis_IQFAppPush = 4,
		/// <summary>
		/// 模拟主推
		/// </summary>
		Redis_IQFVirtualPush = 5,
		/// <summary>
		/// 模拟交易
		/// </summary>
		Redis_IQFVirtualTrade = 6,
		/// <summary>
		/// 量化跟投
		/// </summary>
		Redis_IQFQuant = 7,
		/// <summary>
		/// 量化策略
		/// </summary>
		Redis_IQFStrategy = 8,
		/// <summary>
		/// 代理模块
		/// </summary>
		Redis_IQFProxy = 9,
		/// <summary>
		/// 预警模块
		/// </summary>
		Redis_IQFPrewarning = 10,
		/// <summary>
		/// 风控管理
		/// </summary>
		Redis_IQFRiskMgr = 11,
		/// <summary>
		/// 消息中心
		/// </summary>
		Redis_IQFMsgCenter = 12
	}
}
