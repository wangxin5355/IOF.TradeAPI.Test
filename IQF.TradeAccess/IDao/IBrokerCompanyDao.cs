using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    public interface IBrokerCompanyDao : IProxyService
    {
        List<BrokerCompanyEntity> GetAll();

        string GetFcCode(int brokerType);

        BrokerCompanyEntity Get(int brokerType);

        BrokerCompanyEntity GetByID(long id);
    }
}
