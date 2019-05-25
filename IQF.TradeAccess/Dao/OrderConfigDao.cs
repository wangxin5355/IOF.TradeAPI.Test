using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using IQF.TradeAccess.IDao;

namespace IQF.TradeAccess.Dao
{
    public class OrderConfigDao : IOrderConfigDao
    {
        private readonly IDbSessionFactory factory;

        public OrderConfigDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        public List<OrderConfigEntity> Get(int brokerType, string brokerAccount)
        {
            string sql = "SELECT * FROM [DB_IQFTrade].[dbo].[OrderConfig] where [BrokerType] =@brokerType and [BrokerAccount] =@brokerAccount;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<OrderConfigEntity>(sql, new { brokerType, brokerAccount }).AsList();
            }
        }

        public OrderConfigEntity Get(int brokerType, string brokerAccount, int orderConfigType)
        {
            string sql = "SELECT * FROM [DB_IQFTrade].[dbo].[OrderConfig] where [BrokerType] =@brokerType and [BrokerAccount] =@brokerAccount and [OrderConfigType] =@orderConfigType;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<OrderConfigEntity>(sql, new { brokerType, brokerAccount, orderConfigType }).FirstOrDefault();
            }
        }

        public List<OrderConfigEntity> Get(long tradeAccount)
        {
            string sql = "SELECT * FROM [DB_IQFTrade].[dbo].[OrderConfig] where [TradeAccount] =@tradeAccount;";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<OrderConfigEntity>(sql, new { tradeAccount }).AsList();
            }
        }

        public int AddOrSet(OrderConfigEntity entity)
        {
            string sql = "IF NOT EXISTS(SELECT * FROM dbo.OrderConfig WHERE TradeAccount=@TradeAccount AND OrderConfigType=@OrderConfigType)";
            sql += " INSERT INTO dbo.OrderConfig(TradeAccount, OrderConfigType, OrderType) VALUES(@TradeAccount, @OrderConfigType, @OrderType)";
            sql += " ELSE UPDATE dbo.OrderConfig SET OrderType = @OrderType,UpdateTime = GETDATE() WHERE TradeAccount = @TradeAccount AND OrderConfigType = @orderConfigType; ";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Execute(sql, new { entity.TradeAccount, entity.OrderConfigType, entity.OrderType });
            }
        }

        public bool AddOrSet(OrderConfigEntity entity, out string error)
        {
            error = string.Empty;
            if (entity == null)
            {
                error = "参数为null";
                return false;
            }
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var list = conn.Query("proc_OrderConfig_AddOrSet", new { brokerType = entity.BrokerType, brokerAccount = entity.BrokerAccount, orderConfigType = entity.OrderConfigType, orderType = entity.OrderType }, null, true, null, System.Data.CommandType.StoredProcedure).FirstOrDefault();
                var ret = list as IDictionary<string, object>;
                if (ret == null || ret.Count != 2)
                {
                    error = "插入更新委托配置数据失败";
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
    }
}
