using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    public interface IOrderLogDao : IProxyService
    {
        int Add(OrderLogEntity entity);

        List<OrderLogEntity> Get(int brokerType, string brokerAccount, DateTime beginTime, DateTime endTime);

        List<OrderLogEntity> Get(int brokerType, string brokerAccount, DateTime beginTime, DateTime endTime, int start, int end, out int total);

        List<OrderLogEntity> Get(DateTime beginTime, DateTime endTime);
    }
}
