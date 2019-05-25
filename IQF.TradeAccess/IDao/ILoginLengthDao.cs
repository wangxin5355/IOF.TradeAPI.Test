using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    public interface ILoginLengthDao : IProxyService
    {
        int GetLoginAvailableTime(string brokerAccount, BrokerType brokerType);

        bool SetLoginAvailableTime(string brokerAccount, BrokerType brokerType, int minute, out string error);

        LoginLengthEntity GetLoginAvailableTime(long tradeAccount);

        List<LoginLengthEntity> GetLoginAvailableTime(List<long> tradeAccountList);

        int SetLoginAvailableTime(long tradeAccount, int minute);
    }
}
