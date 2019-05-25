using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;


namespace IQF.TradeAccess.IDao
{
    public interface IOrderConfigDao : IProxyService
    {
        List<OrderConfigEntity> Get(int brokerType, string brokerAccount);

        OrderConfigEntity Get(int brokerType, string brokerAccount, int orderConfigType);

        List<OrderConfigEntity> Get(long tradeAccount);

        int AddOrSet(OrderConfigEntity entity);

        bool AddOrSet(OrderConfigEntity entity, out string error);
    }
}
