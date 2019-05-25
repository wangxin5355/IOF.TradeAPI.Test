using Dapper;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    public class AgentDao : IAgentDao
    {
        private readonly IDbSessionFactory factory;

        public AgentDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// 获取代理
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public AgentEntity GetDBAgent(long userID)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var sql = string.Format(@"select TOP 1 * from dbo.Agent t where t.UserID = {0};", userID);
                return conn.Query<AgentEntity>(sql).FirstOrDefault();
            }
        }
    }
}
