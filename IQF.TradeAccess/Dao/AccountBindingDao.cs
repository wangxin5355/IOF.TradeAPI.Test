using Dapper;
using IQF.Framework.Dao;
using IQF.TradeAccess.IDao;
using IQF.TradeAccess.View;
using System.Collections.Generic;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    public class AccountBindingDao : IAccountBindingDao
    {
        private readonly IDbSessionFactory factory;

        public AccountBindingDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        public List<AccountBindingView> GetDetail(long userID)
        {
            string sql = "SELECT a.*,t.BrokerType,t.BrokerAccount,t.CompCounter FROM [AccountBinding] a inner join [TradeAccount] t on a.TradeAccount=t.TradeAccountID where UserID =@userID order by Sequence;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<AccountBindingView>(sql, new { userID = userID }).AsList();
            }
        }

        public int SetSequence(long userID, int brokerType, string brokerAccount, int sequence)
        {
            string sql = "update AccountBinding set Sequence=@sequence where UserID=@userID and TradeAccount =(select TradeAccountID from [TradeAccount] where BrokerType =@brokerType and BrokerAccount =@brokerAccount);";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Execute(sql, new { sequence = sequence, userID = userID, brokerType = brokerType, brokerAccount = @brokerAccount });
            }
        }

        public int Remove(long userID, int brokerType, string brokerAccount)
        {
            string sql = "delete from AccountBinding where UserID=@userID and TradeAccount =(select TradeAccountID from [TradeAccount] where BrokerType =@brokerType and BrokerAccount =@brokerAccount);";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Execute(sql, new { userID = userID, brokerType = brokerType, brokerAccount = @brokerAccount });
            }
        }

        /// <summary>
        /// 根据期货账户获取用户编号
        /// </summary>
        /// <param name="brokerAccounts"></param>
        /// <returns></returns>
        public List<long> GetBindingUsers(int brokerType, string brokerAccount)
        {
            if (brokerType <= 0 || string.IsNullOrWhiteSpace(brokerAccount))
            {
                return new List<long>();
            }
            var sql = "select distinct(t.userid) from dbo.AccountBinding t where t.TradeAccount = (select t1.TradeAccountID from dbo.TradeAccount t1 where t1.BrokerType = @brokerType and t1.BrokerAccount = @brokerAccount);";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<long>(sql, new { brokerType = brokerType, brokerAccount = @brokerAccount }).AsList();
            }
        }

        /// <summary>
        /// 根据期货账户获取用户编号
        /// </summary>
        /// <param name="brokerAccounts"></param>
        /// <returns></returns>
        public List<long> GetUsersByBrokerAccount(IEnumerable<string> brokerAccounts)
        {
            if (brokerAccounts == null || brokerAccounts.Count() <= 0)
            {
                return new List<long>();
            }
            var sql = "select distinct(a.UserID) from [TradeAccount] t join [AccountBinding] a on t.TradeAccountID =a.TradeAccount where t.BrokerAccount in @brokerAccounts;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<long>(sql, new { brokerAccounts = brokerAccounts }).AsList();
            }
        }

        /// <summary>
        /// 根据代理用户编号获取用户编号
        /// </summary>
        /// <param name="tradeAccountID"></param>
        /// <returns></returns>
        public List<long> GetUsersByTradeAccount(IEnumerable<long> tradeAccountID)
        {
            if (tradeAccountID == null || tradeAccountID.Count() <= 0)
            {
                return new List<long>();
            }
            var sql = "select distinct(UserID) from [AccountBinding] where TradeAccount in @tradeAccountID;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<long>(sql, new { tradeAccountID = tradeAccountID }).AsList();
            }
        }

        /// <summary>
        /// 根据用户编号获取绑定的代理编号
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<long> GetAgentUsersByUser(long userID)
        {
            if (userID <= 0)
            {
                return new List<long>();
            }
            var sql = "select TradeAccount from [AccountBinding] where UserID =@userID;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<long>(sql, new { userID = userID }).AsList();
            }
        }

        /// <summary>
        /// 获取用户绑定期货账户的数量
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int GetBindingCount(long userID)
        {
            if (userID <= 0)
            {
                return 0;
            }
            var sql = "select Count(*) from [AccountBinding] where UserID =@userID;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<int>(sql, new { userID = userID }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取某一期货公司下所有帐号绑定的UserID
        /// </summary>
        /// <param name="brokerType"></param>
        /// <returns></returns>
        public List<long> GetBindingUsers(int brokerType)
        {
            var sql = "select distinct(a.UserID) from [TradeAccount] t join [AccountBinding] a on t.TradeAccountID =a.TradeAccount where t.BrokerType =@brokerType;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<long>(sql, new { brokerType = brokerType }).AsList();
            }
        }
    }
}
