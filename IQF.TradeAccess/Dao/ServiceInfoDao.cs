using Dapper;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System.Collections.Generic;

namespace IQF.TradeAccess.Dao
{
    public class ServiceInfoDao : IServiceInfoDao
    {
        private readonly IDbSessionFactory factory;

        public ServiceInfoDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }
        
        /// <summary>
        /// 更新服务信息
        /// </summary>
        /// <param name="brokerType"></param>
        /// <param name="serverIP"></param>
        /// <param name="serverPort"></param>
        /// <param name="userCount"></param>
        public void Update(int brokerType, string serverIP, int serverPort, int userCount, int procId, int tradingCounter)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var count = conn.Execute("proc_ServiceInfo_Update", new { brokerType, serverIP, serverPort, userCount, procId, tradingCounter }, null, null, System.Data.CommandType.StoredProcedure);
            }
        }

        public List<int> GetPorts(string serverIp)
        {
            string sql = "SELECT t.ServerPort FROM [DB_IQFTrade].[dbo].[ServiceInfo] t where t.ServerIP = @serverIp;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<int>(sql, new { serverIp }).AsList();
            }
        }

        /// <summary>
        /// 获取服务器数量
        /// </summary>
        /// <returns></returns>
        public List<string> GetServers()
        {
            string sql = "SELECT distinct(serverip) FROM [DB_IQFTrade].[dbo].[ServiceInfo];";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<string>(sql).AsList();
            }
        }

        public List<ServiceInfoEntity> GetAll()
        {
            string sql = "SELECT * FROM [DB_IQFTrade].[dbo].[ServiceInfo];";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<ServiceInfoEntity>(sql).AsList();
            }
        }
    }
}
