using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;

namespace IQF.TradeAccess.IDao
{
    public interface IAgentDao : IProxyService
    {
        AgentEntity GetDBAgent(long userID);
    }
}
