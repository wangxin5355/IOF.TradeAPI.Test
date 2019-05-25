using Dapper;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    public class LoginLengthDao : ILoginLengthDao
    {
        private readonly IDbSessionFactory factory;

        public LoginLengthDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// 获取用户设置的登录有效时间（分钟）
        /// </summary>
        /// <param name="brokerAccount"></param>
        /// <param name="brokerType"></param>
        /// <returns>有效时间（分钟）</returns>
        public int GetLoginAvailableTime(string brokerAccount, BrokerType brokerType)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                using (var reader = conn.QueryMultiple("proc_LoginLength_Get", new { brokerAccount, brokerType }, null, null, CommandType.StoredProcedure))
                {
                    return reader.Read<int>().FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// 设置登录有效时间
        /// </summary>
        /// <param name="brokerAccount"></param>
        /// <param name="brokerType"></param>
        /// <param name="length">有效时间（分钟）</param>
        /// <returns></returns>
        public bool SetLoginAvailableTime(string brokerAccount, BrokerType brokerType, int minute, out string error)
        {
            error = string.Empty;
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var list = conn.Query("proc_LoginLength_Set", new { brokerAccount, brokerType, length = minute }, null, true, null, System.Data.CommandType.StoredProcedure).FirstOrDefault();
                var ret = list as IDictionary<string, object>;
                if (ret == null || ret.Count != 2)
                {
                    return false;
                }
                var errorNo = (int)ret.Values.First();
                if (errorNo != 0)
                {
                    error = ret.Values.ElementAt(1).ToString();
                    return false;
                }
            }
            return true;
        }

        public LoginLengthEntity GetLoginAvailableTime(long tradeAccount)
        {
            string sql = "SELECT * FROM LoginLength WHERE TradeAccount =" + tradeAccount;
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<LoginLengthEntity>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 批量获取用户的登录有效时长
        /// </summary>
        /// <param name="tradeAccountList"></param>
        /// <returns></returns>
        public List<LoginLengthEntity> GetLoginAvailableTime(List<long> tradeAccountList)
        {
            if (tradeAccountList == null || tradeAccountList.Count == 0)
            {
                return null;
            }
            string accounts = string.Join(",", tradeAccountList);
            string sql = string.Format("SELECT * FROM LoginLength WHERE TradeAccount in ({0})", accounts);
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<LoginLengthEntity>(sql).AsList();
            }
        }

        public int SetLoginAvailableTime(long tradeAccount, int minute)
        {
            string sql = "IF EXISTS(SELECT * FROM LoginLength WHERE TradeAccount =@tradeAccount) ";
            sql += "BEGIN UPDATE LoginLength SET AvailableTime = @minute WHERE TradeAccount =@tradeAccount END ";
            sql += "ELSE BEGIN INSERT INTO LoginLength(TradeAccount, AvailableTime) VALUES(@tradeAccount, @minute) END";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Execute(sql, new { tradeAccount, minute });
            }
        }
    }
}
