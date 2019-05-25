using Dapper;
using IQF.Framework.Cache;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    /// <summary>
    /// 期货柜台
    /// </summary>
    public class TradingCounterDao : ITradingCounterDao
    {
        private readonly IDbSessionFactory factory;

        public TradingCounterDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        [MemCache(10 * 60)]
        public virtual List<TradingCounterEntity> GetAll()
        {
            string sql = "SELECT * FROM [TradingCounter];";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<TradingCounterEntity>(sql).AsList();
            }
        }

        public TradingCounterEntity Get(long id)
        {
            var all = GetAll();
            if (null == all)
            {
                return null;
            }
            return all.FirstOrDefault(a => (int)a.CounterID == id);
        }
    }
}
