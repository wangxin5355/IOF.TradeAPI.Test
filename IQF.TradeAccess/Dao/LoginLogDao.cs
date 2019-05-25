using Dapper;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    public class LoginLogDao : ILoginLogDao
    {
        private readonly IDbSessionFactory factory;

        public LoginLogDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        public int Add(LoginLogEntity entity)
        {
            if (null == entity)
            {
                return 0;
            }
            string insert = "INSERT INTO [LoginLog] ([UserID],[BrokerAccount],[BrokerType],[AccountType],[IP],[Mac],[PackType],[ClientVersion],[Status],[ErrorNo],[ErrorMsg]) VALUES (@UserID, @BrokerAccount, @BrokerType, @AccountType, @IP, @Mac, @PackType, @ClientVersion,@Status,@ErrorNo,@ErrorMsg)";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Execute(insert, entity);
            }
        }

        /// <summary>
        /// 获取登录日志（按时间倒序）
        /// </summary>
        /// <param name="brokerType"></param>
        /// <param name="brokerAccount"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<LoginLogEntity> Get(int brokerType, string brokerAccount, DateTime beginTime, DateTime endTime)
        {
            string sql = "select * from LoginLog where [BrokerAccount]=@brokerAccount and [BrokerType]=@brokerType and [AddTime] between @beginTime and @endTime order by AddTime desc;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<LoginLogEntity>(sql, new { brokerAccount = brokerAccount, brokerType = brokerType, beginTime = beginTime, endTime = endTime }).AsList();
            }
        }

        /// <summary>
        /// 获取登录登出日志（按时间倒序）
        /// </summary>
        /// <param name="brokerType"></param>
        /// <param name="brokerAccount"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<LoginLogEntity> Get(int brokerType, string brokerAccount, DateTime beginTime, DateTime endTime, int start, int end, out int total)
        {
            total = 0;
            //select top 1000是为了避免数据量过多
            string sql = "select * from(select top 1000 ROW_NUMBER() over(order by AddTime desc) row,* from [DB_IQFTrade].[dbo].[LoginLog] where [BrokerAccount]=@brokerAccount and [BrokerType]=@brokerType and [AddTime] between @beginTime and @endTime) a where a.row between @start and @end;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var list = conn.Query<LoginLogEntity>(sql, new { brokerAccount = brokerAccount, brokerType = brokerType, beginTime = beginTime, endTime = endTime, start = start, end = end }).AsList();
                if (list != null && list.Count > 0)
                {
                    string sql1 = "select COUNT(*) from [DB_IQFTrade].[dbo].[LoginLog] where [BrokerAccount]=@brokerAccount and [BrokerType]=@brokerType and [AddTime] between @beginTime and @endTime";
                    total = conn.Query<int>(sql1, new { brokerAccount = brokerAccount, brokerType = brokerType, beginTime = beginTime, endTime = endTime, start = start, end = end }).First();
                }
                return list;
            }
        }
    }
}
