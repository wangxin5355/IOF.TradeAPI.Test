using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;


namespace IQF.TradeAccess.IDao
{
    public interface IGatewayInfoDao : IProxyService
    {
        List<GateWayInfoEntity> GetAvailable(long compCounter = 0);

        List<GateWayInfoEntity> GetAll(long compCounter);

        List<GateWayInfoEntity> GetAll();
    }
}
