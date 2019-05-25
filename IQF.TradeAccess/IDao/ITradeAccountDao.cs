using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    public interface ITradeAccountDao : IProxyService
    {
        bool AddLoginInfo(LoginLogEntity logEntity, string openMoblie, TradeAccountSource source, out string error);

        bool AddLoginInfo(LoginLogEntity logEntity, string openMoblie, TradeAccountSource source, out string error, out long tradeAccountID);

        TradeAccountEntity Get(int brokerType, string brokerAccount);

        TradeAccountEntity GetAgentUser(long agentID, long tradeAccountID);

        List<TradeAccountEntity> GetAllCache();

        List<TradeAccountEntity> GetAllFromDB();

        int Remove(long tradeAccountID, long agentID);

        List<TradeAccountEntity> GetByPage(long agentID, int page, int pageSize, out int total);

        List<TradeAccountEntity> SearchByPage(long agentID, string brokerAccount, string realName, DateTime beginLoginTime, DateTime endLoginTime, DateTime beginCreateTime, DateTime endCreateTime, int page, int pageSize, out int total);

        //未封装
        //bool AddAgentUser(TradeAccountEntity entiety, out long agentUserID, out string error);
        //未封装
        //bool SetAgentUser(long agentID, long tradeAccountID, string realName, out string error);

        int SetAgent(long tradeAccountID, long agentID);

        int SetMobile(int brokerType, string brokerAccount, string openAcctMobile);
    }
}
