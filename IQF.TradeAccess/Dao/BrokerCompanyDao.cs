using Dapper;
using IQF.Framework.Cache;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System.Collections.Generic;


namespace IQF.TradeAccess.Dao
{
    public class BrokerCompanyDao : IBrokerCompanyDao
    {
        private readonly IDbSessionFactory factory;

        public BrokerCompanyDao(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// 获取所有期货公司信息（包含未上线）
        /// </summary>
        /// <returns></returns>
        [MemCache(10 * 60)]
        public virtual List<BrokerCompanyEntity> GetAll()
        {
            string sql = "SELECT * FROM [BrokerCompany];";
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                return conn.Query<BrokerCompanyEntity>(sql).AsList();
            }
        }

        public string GetFcCode(int brokerType)
        {
            var broker = Get(brokerType);
            if (null == broker)
            {
                return string.Empty;
            }
            return broker.FcCode;
        }

        /// <summary>
        /// 获取期货公司信息
        /// </summary>
        /// <param name="brokerType"></param>
        /// <returns></returns>
        public BrokerCompanyEntity Get(int brokerType)
        {
            var brokers = GetAll();
            if (null == brokers)
            {
                return null;
            }
            return brokers.Find(a => (int)a.BrokerType == brokerType);
        }

        /// <summary>
        /// 获取期货公司信息
        /// </summary>
        /// <param name="brokerType"></param>
        /// <returns></returns>
        public BrokerCompanyEntity GetByID(long id)
        {
            var brokers = GetAll();
            if (null == brokers)
            {
                return null;
            }
            return brokers.Find(a => a.CompanyID == id);
        }
    }
}
