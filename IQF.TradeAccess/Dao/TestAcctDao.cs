using Dapper;
using IQF.Framework;
using IQF.Framework.Cache;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    /// <summary>
    /// 测试账号
    /// </summary>
    public class TestAcctDao : ITestAcctDao
    {
        private readonly IDbSessionFactory factory;

        public TestAcctDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        [MemCache(3 * 60)]
        public virtual List<TestAcctEntity> GetAll()
        {
            return GetAllFromDB();
        }

        public List<TestAcctEntity> GetAllFromDB()
        {
            string sql = "SELECT * FROM [TestAcct];";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<TestAcctEntity>(sql).AsList();
            }
        }

        public TestAcctEntity GetItemById(long nId)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var ay = conn.Query<TestAcctEntity>("select * from TestAcct where TestAcctID=@TestAcctID ", new { TestAcctID = nId }).ToList();
                return ay.Count == 0 ? null : ay[0];
            }
        }
    }
}
