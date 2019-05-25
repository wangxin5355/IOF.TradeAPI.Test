using Dapper;
using IQF.Framework.Dao;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.IDao;
using System.Collections.Generic;
using System.Linq;

namespace IQF.TradeAccess.Dao
{
    public class FuturesCompany : IFuturesCompany
    {
        private readonly IDbSessionFactory factory;

        public FuturesCompany(IDbSessionFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// 根据期货代码获取公司信息
        /// </summary>
        /// <param name="fcCode">公司代码</param>
        /// <returns></returns>
        public IFuturesCompany GetByFcCode(string fcCode)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var ay = SqlMapper.Query<IFuturesCompany>(conn, "select * from FuturesCompany where fcCode=@fcCode",
                    new { fcCode = fcCode }).ToList();
                if (ay.Count == 0)
                {
                    return null;
                }
                else
                {
                    return ay[0];
                }
            }
        }


        /// <summary>
        /// 根据期货公司名称获取实体
        /// </summary>
        /// <param name="fcName"></param>
        /// <returns></returns>
        public IFuturesCompany GetByFcName(string fcName)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var ay = SqlMapper.Query<IFuturesCompany>(conn, "select * from FuturesCompany where fcName=@fcName",
                    new { fcName = fcName.Trim() }).ToList();
                if (ay.Count == 0)
                {
                    return null;
                }
                else
                {
                    return ay[0];
                }
            }
        }


        /// <summary>
        /// 期货公司列表
        /// </summary>
        public List<IFuturesCompany> GetCompanyList()
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var ay = SqlMapper.Query<IFuturesCompany>(conn, "select * from FuturesCompany order by sortNum", null).ToList();

                if (ay.Count == 0)
                {
                    return null;
                }
                else
                {
                    return ay;
                }
            }
        }

        /// <summary>
        /// 显示期货公司列表
        /// </summary>
        public List<IFuturesCompany> GetShowCompanyList()
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var ay = SqlMapper.Query<IFuturesCompany>(conn, "select * from FuturesCompany where isShow=1 order by sortNum", null).ToList();

                if (ay.Count == 0)
                {
                    return null;
                }
                else
                {
                    return ay;
                }
            }
        }


        /// <summary>
        /// 期货公司列表 包的类型
        /// </summary>
        public List<VM_FutureCompanyPackType> GetShowPackageCompanyList(string paramPackType)
        {
            using (var conn = factory.Create(DatabaseName.DB_IQFTrade))
            {
                var ay = SqlMapper.Query<VM_FutureCompanyPackType>(conn, string.Format("select * from VM_FutureCompanyPackType where 1=1 {0} order by sortNum ", paramPackType)).ToList();

                if (ay.Count == 0)
                {
                    return null;
                }
                else
                {
                    return ay;
                }
            }
        }

    }
}
