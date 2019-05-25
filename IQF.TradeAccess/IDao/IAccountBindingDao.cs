using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.View;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    public interface IAccountBindingDao : IProxyService
    {
        List<AccountBindingView> GetDetail(long userID);

        int SetSequence(long userID, int brokerType, string brokerAccount, int sequence);

        int Remove(long userID, int brokerType, string brokerAccount);

        List<long> GetBindingUsers(int brokerType, string brokerAccount);

        List<long> GetUsersByBrokerAccount(IEnumerable<string> brokerAccounts);

        List<long> GetUsersByTradeAccount(IEnumerable<long> tradeAccountID);

        List<long> GetAgentUsersByUser(long userID);

        int GetBindingCount(long userID);

        List<long> GetBindingUsers(int brokerType);
    }
}
