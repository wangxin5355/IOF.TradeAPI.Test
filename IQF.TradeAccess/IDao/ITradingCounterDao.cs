using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    /// <summary>
    /// 期货柜台
    /// </summary>
    public interface ITradingCounterDao : IProxyService
    {
        List<TradingCounterEntity> GetAll();

        TradingCounterEntity Get(long id);
    }
}
