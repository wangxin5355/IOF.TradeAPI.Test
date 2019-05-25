using Dapper;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    public class OrderLogDao : IOrderLogDao
    {
        private readonly IDbSessionFactory factory;

        public OrderLogDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        public int Add(OrderLogEntity entity)
        {
            if (null == entity)
            {
                return 0;
            }
            string insert = "INSERT INTO [OrderLog] ([UserID], [BrokerAccount], [BrokerType], [Symbol], [Exchange], [OrderSide], [Price], [Quantity], [OrderType], [Offset], [Source], [OderID],[BizType], [ErrorNo], [ErrorMsg], [IP], [Mac], [PackType], [TradeDate]) VALUES (@UserID, @BrokerAccount, @BrokerType, @Symbol, @Exchange, @OrderSide, @Price, @Quantity, @OrderType, @Offset, @Source, @OderID, @BizType, @ErrorNo, @ErrorMsg, @IP ,@Mac ,@PackType ,@TradeDate)";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Execute(insert, entity);
            }
        }

        /// <summary>
        /// 获取委托撤单日志（按时间倒序）
        /// </summary>
        /// <param name="brokerType"></param>
        /// <param name="brokerAccount"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<OrderLogEntity> Get(int brokerType, string brokerAccount, DateTime beginTime, DateTime endTime)
        {
            string sql = "select * from OrderLog where [BrokerAccount]=@brokerAccount and [BrokerType]=@brokerType and [AddTime] between @beginTime and @endTime order by AddTime desc;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<OrderLogEntity>(sql, new { brokerAccount = brokerAccount, brokerType = brokerType, beginTime = beginTime, endTime = endTime }).AsList();
            }
        }

        /// <summary>
        /// 获取委托撤单日志（按时间倒序）
        /// </summary>
        /// <param name="brokerType"></param>
        /// <param name="brokerAccount"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<OrderLogEntity> Get(int brokerType, string brokerAccount, DateTime beginTime, DateTime endTime, int start, int end, out int total)
        {
            total = 0;
            string sql = "select * from(select ROW_NUMBER() over(order by AddTime desc) row,* from [DB_IQFTrade].[dbo].[OrderLog] where [BrokerAccount]=@brokerAccount and [BrokerType]=@brokerType and [AddTime] between @beginTime and @endTime) a where a.row between @start and @end;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var list = conn.Query<OrderLogEntity>(sql, new { brokerAccount = brokerAccount, brokerType = brokerType, beginTime = beginTime, endTime = endTime, start = start, end = end }).AsList();
                if (list != null && list.Count > 0)
                {
                    string sql1 = "select COUNT(*) from [DB_IQFTrade].[dbo].[OrderLog] where [BrokerAccount]=@brokerAccount and [BrokerType]=@brokerType and [AddTime] between @beginTime and @endTime";
                    total = conn.Query<int>(sql1, new { brokerAccount = brokerAccount, brokerType = brokerType, beginTime = beginTime, endTime = endTime, start = start, end = end }).First();
                }
                return list;
            }
        }

        /// <summary>
        /// 获取某一时间段内的所有委托撤单记录
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<OrderLogEntity> Get(DateTime beginTime, DateTime endTime)
        {
            string sql = "select * from OrderLog where [AddTime] between @beginTime and @endTime order by AddTime desc;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<OrderLogEntity>(sql, new { beginTime = beginTime, endTime = endTime }).AsList();
            }
        }
    }
}
