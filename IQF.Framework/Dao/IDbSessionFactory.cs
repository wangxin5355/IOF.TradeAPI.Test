using System.Data;

namespace IQF.Framework.Dao
{
	/// <summary>
	/// 数据库会话工厂
	/// </summary>
	public interface IDbSessionFactory
	{
		/// <summary>
		/// 创建连接会话
		/// </summary>
		/// <param name="isSlaveDb">是否创建从库（只读）连接</param>
		/// <returns></returns>
		IDbConnection Create(DatabaseName databaseName, bool isSlaveDb = false);
	}

	/// <summary>
	/// 数据库名称
	/// </summary>
	public enum DatabaseName
	{
		/// <summary>
		/// 国内期货
		/// </summary>
		DB_DomesticFutures = 1,
		/// <summary>
		/// 国内期货公司风险测评
		/// </summary>
		DB_FuturesRiskTest = 2,
		/// <summary>
		/// 国内期货公司软件合作
		/// </summary>
		DB_FuturesCooperate = 3,
		/// <summary>
		/// 盈宽-用户
		/// </summary>
		DB_IQFUser = 4,
		/// <summary>
		/// 盈宽-数据
		/// </summary>
		DB_IQFData = 5,
		/// <summary>
		/// 盈宽-交易
		/// </summary>
		DB_IQFTrade = 6,
		/// <summary>
		/// 盈宽-量化模块
		/// </summary>
		DB_IQFQuant = 7,
		/// <summary>
		/// 盈宽-包管理
		/// </summary>
		DB_IQFAppManager = 8,
		/// <summary>
		/// 盈宽-量化策略
		/// </summary>
		DB_IQFStrategy = 9,
		/// <summary>
		/// 期货淘金者-社区
		/// </summary>
		DB_IQFCommunity = 10,
		/// <summary>
		/// 模拟交易
		/// </summary>
		DB_IQFVirtual = 11
	}
}
