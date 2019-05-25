using Dapper;
using IQF.Framework.Cache;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System.Collections.Generic;

namespace IQF.TradeAccess.Dao
{
    public class GatewayInfoDao : IGatewayInfoDao
    {
        private readonly IDbSessionFactory factory;

        public GatewayInfoDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        public List<GateWayInfoEntity> GetAvailable(long compCounter = 0)
        {
            var list = GetAll();
            if (list == null)
            {
                return null;
            }
            list = list.FindAll(a => a.Available == 1);
            if (list == null)
            {
                return null;
            }
            if (compCounter == 0)
            {
                return list;
            }
            return list.FindAll(a => a.CompCounter == compCounter);
        }

        public List<GateWayInfoEntity> GetAll(long compCounter)
        {
            var list = GetAll();
            if (list == null)
            {
                return null;
            }
            return list.FindAll(a => a.CompCounter == compCounter);
        }

        [MemCache(5 * 60)]
        public virtual List<GateWayInfoEntity> GetAll()
        {
            string sql = "SELECT * FROM [GatewayInfo];";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<GateWayInfoEntity>(sql).AsList();
            }
        }

    }
}
