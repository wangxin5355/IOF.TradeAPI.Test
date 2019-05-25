using IQF.Framework.IModules;
using System;
using System.Data;

namespace IQF.Framework.Dao
{
	internal class DbSessionFactory : IDbSessionFactory
	{
		private IDataConfiguration daoConfiguration;

		public DbSessionFactory(IDataConfiguration daoConfiguration)
		{
			this.daoConfiguration = daoConfiguration;
		}

		/// <summary>
		/// 创建连接会话
		/// </summary>
		/// <typeparam name="databaseName"></typeparam>
		/// <param name="isReadOnly"></param>
		/// <returns></returns>
		public IDbConnection Create(DatabaseName databaseName, bool isReadOnly = false)
		{
			var connStr = this.GetConnStr(databaseName.ToString(), isReadOnly);
			return new System.Data.SqlClient.SqlConnection(connStr);
		}

		/// <summary>
		/// 获取连接串
		/// </summary>
		/// <param name="rwConnStr"></param>
		/// <returns></returns>
		private string GetConnStr(string connStringName, bool isReadOnly)
		{
			var connStr = this.daoConfiguration.GetDbConnStr(connStringName);
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
