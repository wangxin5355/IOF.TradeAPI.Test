using Dapper;
using IQF.Framework.Cache;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System.Collections.Generic;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    /// <summary>
    /// 期货公司柜台映射关系
    /// </summary>
    public class CompCounterDao : ICompCounterDao
    {
        private readonly IDbSessionFactory factory;

        public CompCounterDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        [MemCache(10 * 60)]
        public virtual List<CompCounterEntity> GetAll()
        {
            string sql = "SELECT * FROM [CompCounter];";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<CompCounterEntity>(sql).AsList();
            }
        }

        public CompCounterEntity Get(long id)
        {
            var all = GetAll();
            if (null == all)
            {
                return null;
            }
            return all.FirstOrDefault(a => a.CompCounterID == id);
        }

        public List<CompCounterEntity> GetByCompanyID(long companyID)
        {
            var all = GetAll();
            if (null == all)
            {
                return null;
            }
            return all.FindAll(a => a.BrokerCompany == companyID);
        }

    }
}
