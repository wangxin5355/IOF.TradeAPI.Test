using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    public interface ILoginLogDao : IProxyService
    {
        int Add(LoginLogEntity entity);

        List<LoginLogEntity> Get(int brokerType, string brokerAccount, DateTime beginTime, DateTime endTime);

        List<LoginLogEntity> Get(int brokerType, string brokerAccount, DateTime beginTime, DateTime endTime, int start, int end, out int total);
    }
}
