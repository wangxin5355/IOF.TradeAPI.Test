using Dapper;
using IQF.Framework.Cache;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System.Collections.Generic;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    public class ApiInfoDao : IApiInfoDao
    {
        private readonly IDbSessionFactory factory;

        public ApiInfoDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        public List<ApiInfoEntity> GetAllFromDB()
        {
            string sql = "SELECT * FROM [ApiInfo];";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<ApiInfoEntity>(sql).AsList();
            }
        }


        public ApiInfoEntity GetItemById(long nId)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<ApiInfoEntity>("select * from ApiInfo where ApiInfoID=@ApiInfoID", new { ApiInfoID = nId }).FirstOrDefault();
            }
        }

        [MemCache(60)]
        public virtual List<ApiInfoEntity> GetAll()
        {
            return GetAllFromDB();
        }
    }
}
