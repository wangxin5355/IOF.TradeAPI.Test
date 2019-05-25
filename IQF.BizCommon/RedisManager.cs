using IQF.Framework;

namespace IQF.BizCommon
{
	public static class RedisManager
	{
		#region 行情REDIS配置
		private static readonly string Redis_IQFQuote = ConfigManager.GetReidsConnectionString("Redis_IQFQuote");

		/// <summary>
		/// 行情redis
		/// </summary>
		public static readonly RedisHelper QuoteRedis = new RedisHelper(Redis_IQFQuote);
		#endregion

		#region 用户系统REDIS配置

		private static readonly string Redis_IQFUser = ConfigManager.GetReidsConnectionString("Redis_IQFUser");
		/// <summary>
		/// 用户系统REDIS访问类
		/// </summary>
		public static readonly RedisHelper UserRedis = new RedisHelper(Redis_IQFUser);
		#endregion

		#region 交易系统REDIS配置

		private static readonly string Redis_IQFTrade = ConfigManager.GetReidsConnectionString("Redis_IQFTrade");

		/// <summary>
		/// 交易redis
		/// </summary>
		public static readonly RedisHelper TradeRedis = new RedisHelper(Redis_IQFTrade);

		#endregion

		#region 推送服务redis

		private static readonly string Redis_IQFAppPush = ConfigManager.GetReidsConnectionString("Redis_IQFAppPush");

		/// <summary>
		/// 推送服务redis
		/// </summary>
		public static readonly RedisHelper AppPushRedis = new RedisHelper(Redis_IQFAppPush);
		#endregion
 
		#region 模拟交易 
		private static readonly string IQFVirtualTradeRedisConnStr = ConfigManager.GetReidsConnectionString("Redis_IQFVirtualTrade");
		#endregion

		#region 量化模块
		private static readonly string Redis_IQFQuant = ConfigManager.GetReidsConnectionString("Redis_IQFQuant");

		/// <summary>
		/// 量化模块redis
		/// </summary>
		public static readonly RedisHelper QuantRedis = new RedisHelper(Redis_IQFQuant);
		#endregion

		#region 量化策略
		private static readonly string Redis_IQFStrategy = ConfigManager.GetReidsConnectionString("Redis_IQFStrategy");

		/// <summary>
		/// 量化策略redis
		/// </summary>
		public static readonly RedisHelper StrategyRedis = new RedisHelper(Redis_IQFStrategy);
		#endregion

		#region 代理模块

		private static readonly string Redis_IQFProxy = ConfigManager.GetReidsConnectionString("Redis_IQFProxy");

		/// <summary>
		/// 代理模块redis
		/// </summary>
		public static readonly RedisHelper ProxyFutureRedis = new RedisHelper(Redis_IQFProxy);

		#endregion

		#region 预警模块

		private static readonly string Redis_IQFPrewarning = ConfigManager.GetReidsConnectionString("Redis_IQFPrewarning");

		/// <summary>
		/// 预警Redis
		/// </summary>
		public static readonly RedisHelper PrewarningRedis = new RedisHelper(Redis_IQFPrewarning);

		#endregion

		#region 风控

		private static readonly string Redis_IQFRiskMgr = ConfigManager.GetReidsConnectionString("Redis_IQFRiskMgr");

		/// <summary>
		/// 风控Redis
		/// </summary>
		public static readonly RedisHelper RiskManagementRedis = new RedisHelper(Redis_IQFRiskMgr);
		#endregion

		#region 消息中心

		private static readonly string Redis_IQFMsgCenter = ConfigManager.GetReidsConnectionString("Redis_IQFMsgCenter");

		/// <summary>
		/// 消息中心Redis
		/// </summary>
		public static readonly RedisHelper MsgCenterRedis = new RedisHelper(Redis_IQFMsgCenter);

	    #endregion

      


	}
}
