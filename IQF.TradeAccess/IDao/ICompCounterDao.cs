using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    /// <summary>
    /// 期货公司柜台映射关系
    /// </summary>
    public interface ICompCounterDao : IProxyService
    {
        List<CompCounterEntity> GetAll();

        CompCounterEntity Get(long id);

        List<CompCounterEntity> GetByCompanyID(long companyID);
    }
}
