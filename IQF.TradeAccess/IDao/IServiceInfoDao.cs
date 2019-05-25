using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    public interface IServiceInfoDao : IProxyService
    {
        void Update(int brokerType, string serverIP, int serverPort, int userCount, int procId, int tradingCounter);

        List<int> GetPorts(string serverIp);

        List<string> GetServers();

        List<ServiceInfoEntity> GetAll();
    }
}
