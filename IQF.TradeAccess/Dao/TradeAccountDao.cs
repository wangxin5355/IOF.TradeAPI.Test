using Dapper;
using IQF.Framework.Cache;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    public class TradeAccountDao : ITradeAccountDao
    {
        private readonly IDbSessionFactory factory;

        public TradeAccountDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        public bool AddLoginInfo(LoginLogEntity logEntity, string openMoblie, TradeAccountSource source, out string error)
        {
            error = string.Empty;
            if (logEntity == null)
            {
                error = "参数为null";
                return false;
            }
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var list = conn.Query("proc_Login_Add", new { userID = logEntity.UserID, brokerAccount = logEntity.BrokerAccount, brokerType = logEntity.BrokerType, accountType = logEntity.AccountType, IP = logEntity.IP, Mac = logEntity.Mac, packType = logEntity.PackType, clientVersion = logEntity.ClientVersion, status = logEntity.Status, errorNo = logEntity.ErrorNo, errorMsg = logEntity.ErrorMsg, openAcctMobile = openMoblie, source = source }, null, true, null, System.Data.CommandType.StoredProcedure).FirstOrDefault();
                var ret = list as IDictionary<string, object>;
                if (ret == null || ret.Count != 2)
                {
                    error = "插入登录数据失败";
                    return false;
                }
                var errorNo = (int)ret.Values.First();
                if (errorNo != 0)
                {
                    error = ret.Values.ElementAt(1).ToString();
                    return false;
                }
                return true;
            }
        }

        public bool AddLoginInfo(LoginLogEntity logEntity, string openMoblie, TradeAccountSource source, out string error, out long tradeAccountID)
        {
            error = string.Empty;
            tradeAccountID = 0;
            if (logEntity == null)
            {
                error = "参数为null";
                return false;
            }
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var list = conn.Query("proc_Login_Add_New", new { userID = logEntity.UserID, brokerAccount = logEntity.BrokerAccount, brokerType = logEntity.BrokerType, accountType = logEntity.AccountType, IP = logEntity.IP, Mac = logEntity.Mac, packType = logEntity.PackType, clientVersion = logEntity.ClientVersion, status = logEntity.Status, errorNo = logEntity.ErrorNo, errorMsg = logEntity.ErrorMsg, openAcctMobile = openMoblie, source = source, compCounter = logEntity.CompCounter }, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
                var ret = list as IDictionary<string, object>;
                if (ret == null)
                {
                    error = "插入登录数据失败";
                    return false;
                }
                var errorNo = (int)ret.Values.First();
                if (errorNo != 0)
                {
                    error = ret.Values.ElementAt(1).ToString();
                    return false;
                }
                tradeAccountID = Convert.ToInt64(ret.Values.ElementAt(2));
                return true;
            }
        }

        public TradeAccountEntity Get(int brokerType, string brokerAccount)
        {
            string sql = "SELECT * FROM [DB_IQFTrade].[dbo].[TradeAccount] where [BrokerType] =@brokerType and [BrokerAccount] =@brokerAccount;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<TradeAccountEntity>(sql, new { brokerType = brokerType, brokerAccount = brokerAccount }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取代理用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public TradeAccountEntity GetAgentUser(long agentID, long tradeAccountID)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var sql = @"SELECT * FROM [dbo].[TradeAccount] t where t.TradeAccountID = @tradeAccountID AND t.agentID = @agentID;";
                return conn.Query<TradeAccountEntity>(sql, new { tradeAccountID = tradeAccountID, agentID = agentID }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取所有账户
        /// </summary>
        /// <returns></returns>
        [MemCache(2 * 60)]
        public virtual List<TradeAccountEntity> GetAllCache()
        {
            return GetAllFromDB();
        }

        public List<TradeAccountEntity> GetAllFromDB()
        {
            string sql = "SELECT * FROM [DB_IQFTrade].[dbo].[TradeAccount];";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<TradeAccountEntity>(sql).AsList();
            }
        }

        public int Remove(long tradeAccountID, long agentID)
        {
            string sql = "update [DB_IQFTrade].[dbo].[TradeAccount] set AgentID = NULL,RealName = NULL where TradeAccountID = @tradeAccountID AND AgentID = @agentID;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Execute(sql, new { tradeAccountID = tradeAccountID, agentID = agentID });
            }
        }

        public List<TradeAccountEntity> GetByPage(long agentID, int page, int pageSize, out int total)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                using (var reader = conn.QueryMultiple("proc_AgentUser_GetByPage", new { agentID = agentID, page = page, pageSize = pageSize }, null, null, CommandType.StoredProcedure))
                {
                    var result = reader.Read<TradeAccountEntity>().AsList();
                    total = reader.Read<int>().FirstOrDefault();
                    return result;
                }
            }
        }

        public List<TradeAccountEntity> SearchByPage(long agentID, string brokerAccount, string realName, DateTime beginLoginTime, DateTime endLoginTime, DateTime beginCreateTime, DateTime endCreateTime, int page, int pageSize, out int total)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                using (var reader = conn.QueryMultiple("proc_TradeAccount_SearchByPage",
                    new
                    {
                        agentID = agentID,
                        page = page,
                        pageSize = pageSize,
                        brokerAccount = brokerAccount,
                        realName = realName,
                        beginLoginTime = beginLoginTime,
                        endLoginTime = endLoginTime,
                        beginCreateTime = beginCreateTime,
                        endCreateTime = endCreateTime
                    }, null, null, CommandType.StoredProcedure))
                {
                    var result = reader.Read<TradeAccountEntity>().AsList();
                    total = reader.Read<int>().FirstOrDefault();
                    return result;
                }
            }
        }

        ///// <summary>
        ///// 添加代理用户
        ///// </summary>
        ///// <param name="entiety"></param>
        ///// <param name="agentUserID"></param>
        ///// <param name="error"></param>
        ///// <returns></returns>
        //public static bool AddAgentUser(TradeAccountEntity entiety, out long agentUserID, out string error)
        //{
        //    var paramters = new List<SqlParameter>();
        //    paramters.Add(new SqlParameter("@agentID", entiety.AgentID));
        //    paramters.Add(new SqlParameter("@brokerType", entiety.BrokerType));
        //    paramters.Add(new SqlParameter("@brokerAccount", entiety.BrokerAccount));
        //    paramters.Add(new SqlParameter("@realName", entiety.RealName));
        //    paramters.Add(new SqlParameter("@source", entiety.Source));
        //    TradeUtils.SpResult sr;
        //    if (!TradeUtils.ExecSp("proc_TradeAccount_AddAgentUser", ConnectionString.DB_IQFTradeConnection, paramters, out sr))
        //    {
        //        agentUserID = -1;
        //        error = sr.err;
        //        return false;
        //    }
        //    if (sr.ret != 0)
        //    {
        //        agentUserID = -1;
        //        error = sr.msg;
        //        return false;
        //    }

        //    agentUserID = Convert.ToInt64(sr.ds.Tables[1].Rows[0][0]);
        //    error = string.Empty;
        //    return true;
        //}

        ///// <summary>
        ///// 编辑代理用户
        ///// </summary>
        ///// <param name="agentID"></param>
        ///// <param name="tradeAccountID"></param>
        ///// <param name="realName"></param>
        ///// <param name="error"></param>
        ///// <returns></returns>
        //public static bool SetAgentUser(long agentID, long tradeAccountID, string realName, out string error)
        //{
        //    var paramters = new List<SqlParameter>();
        //    paramters.Add(new SqlParameter("@agentID", agentID));
        //    paramters.Add(new SqlParameter("@tradeAccountID", tradeAccountID));
        //    paramters.Add(new SqlParameter("@realName", realName));
        //    TradeUtils.SpResult sr;
        //    if (!TradeUtils.ExecSp("proc_TradeAccount_Set", ConnectionString.DB_IQFTradeConnection, paramters, out sr))
        //    {
        //        error = sr.err;
        //        return false;
        //    }
        //    if (sr.ret != 0)
        //    {
        //        error = sr.msg;
        //        return false;
        //    }

        //    error = string.Empty;
        //    return true;
        //}

        /// <summary>
        /// 为帐号设置代理
        /// </summary>
        /// <param name="tradeAccountID"></param>
        /// <param name="agentID"></param>
        /// <returns></returns>
        public int SetAgent(long tradeAccountID, long agentID)
        {
            string sql = "UPDATE [dbo].[TradeAccount] SET [AgentID] = @agentID WHERE [TradeAccountID] = @tradeAccountID;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Execute(sql, new { tradeAccountID = tradeAccountID, agentID = agentID });
            }
        }

        /// <summary>
        /// 更新用户手机号
        /// </summary>
        /// <param name="brokerType"></param>
        /// <param name="brokerAccount"></param>
        /// <param name="openAcctMobile">加密过后的手机号</param>
        /// <returns></returns>
        public int SetMobile(int brokerType, string brokerAccount, string openAcctMobile)
        {
            if (string.IsNullOrWhiteSpace(openAcctMobile))
            {
                return 0;
            }
            string sql = "UPDATE [dbo].[TradeAccount] SET [OpenAcctMobile] = @openAcctMobile WHERE [BrokerType] =@brokerType and [BrokerAccount] =@brokerAccount;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Execute(sql, new { openAcctMobile = openAcctMobile, brokerType = brokerType, brokerAccount = brokerAccount });
            }
        }
    }
}
