using IQF.Framework;
using IQF.Framework.Dao;
using System;
using System.Data;

namespace IQF.BizCommon
{
	public static class ConnectionString
	{
		/// <summary>
		/// 国内期货
		/// </summary>
		[Obsolete("使用ConnectionString.Create代替")]
		public static readonly string DB_DomesticFutures = ConfigManager.GetConnectString("DB_DomesticFutures");

		/// <summary>
		/// 国内期货公司风险测评
		/// </summary>
		[Obsolete("使用ConnectionString.Create代替")]
		public static readonly string DB_FuturesRiskTest = ConfigManager.GetConnectString("DB_FuturesRiskTest");

		/// <summary>
		/// 国内期货公司软件合作
		/// </summary>
		[Obsolete("使用ConnectionString.Create代替")]
		public static readonly string DB_FuturesCooperate = ConfigManager.GetConnectString("DB_FuturesCooperate");

		/// <summary>
		/// 盈宽-用户
		/// </summary>
		[Obsolete("使用ConnectionString.Create代替")]
		public static readonly string DB_IQFUser = ConfigManager.GetConnectString("DB_IQFUser");

		/// <summary>
		/// 盈宽-数据
		/// </summary>
		[Obsolete("使用ConnectionString.Create代替")]
		public static readonly string DB_IQFData = ConfigManager.GetConnectString("DB_IQFData");

		/// <summary>
		/// 盈宽-交易
		/// </summary>
		[Obsolete("使用ConnectionString.Create代替")]
		public static readonly string DB_IQFTradeConnection = ConfigManager.GetConnectString("DB_IQFTrade");

		/// <summary>
		/// 盈宽-量化模块
		/// </summary>
		[Obsolete("使用ConnectionString.Create代替")]
		public static readonly string DB_IQFQuantConnection = ConfigManager.GetConnectString("DB_IQFQuant");

		/// <summary>
		/// 盈宽-包管理
		/// </summary>
		[Obsolete("使用ConnectionString.Create代替")]
		public static readonly string DB_IQFAppManagerConnection = ConfigManager.GetConnectString("DB_IQFAppManager");

		/// <summary>
		/// 盈宽-量化策略
		/// </summary>
		[Obsolete("使用ConnectionString.Create代替")]
		public static readonly string DB_IQFStrategyConnection = ConfigManager.GetConnectString("DB_IQFStrategy");

		/// <summary>
		/// 期货淘金者-社区
		/// </summary>
		[Obsolete("使用ConnectionString.Create代替")]
		public static readonly string DB_IQFCommunityConnection = ConfigManager.GetConnectString("DB_IQFCommunity");

		/// <summary>
		/// 创建连接会话
		/// </summary>
		/// <typeparam name="databaseName"></typeparam>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public static IDbConnection Create(DatabaseName databaseName, bool isReadOnly = false)
		{
			var connStr = GetConnStr(databaseName.ToString(), isReadOnly);
			return new System.Data.SqlClient.SqlConnection(connStr);
		}

		/// <summary>
		/// 获取连接串
		/// </summary>
		/// <param name="rwConnStr"></param>
		/// <returns></returns>
		private static string GetConnStr(string connStringName, bool isReadOnly)
		{
			var connStr = ConfigManager.GetConnectString(connStringName);
			if (string.IsNullOrWhiteSpace(connStr))
			{
				throw new ApplicationException($"连接串未配置{connStringName}");
			}
			if (isReadOnly)
			{
				return $"{connStr};ApplicationIntent=ReadOnly;MultiSubnetFailover=True";
			}
			return $"{connStr};MultiSubnetFailover=True";
		}

	}
}
