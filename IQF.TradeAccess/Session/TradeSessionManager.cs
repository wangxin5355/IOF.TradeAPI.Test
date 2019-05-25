using IQF.Framework.Cache;
using IQF.TradeAccess.ISession;
using System.Collections.Generic;
using System.Linq;

namespace IQF.TradeAccess.Session
{
    public class TradeSessionManager : ITradeSessionManager
    {
        private readonly IDistributedCache distributedCache;

        public TradeSessionManager(IDistributedCacheFactory distributedCacheFactory)
        {
            this.distributedCache = distributedCacheFactory.Create(DistributedCacheName.Redis_IQFTrade);
        }

        /// <summary>
        /// 保存会话信息，并更新该会话对应账号下面的TradeToken
        /// </summary>
        /// <param name="tradeToken"></param>
        /// <param name="session">存储的交易账户信息，过期时间为0代表永久有效</param>
        /// <returns></returns>
        public bool SaveSession(string tradeToken, TradeSession session)
        {
            if (string.IsNullOrWhiteSpace(tradeToken))
            {
                return false;
            }
            if (session == null || string.IsNullOrEmpty(session.BrokerAccount))
            {
                return false;
            }

            var allToken = InnerGetAllToken(session.CompCounter, session.BrokerAccount);
            if (allToken == null)
            {
                allToken = new List<string>();
            }
            allToken.Add(tradeToken);
            this.distributedCache.Set(GetBrokerAccountKey(session.CompCounter, session.BrokerAccount), allToken);

            return UpdateSession(tradeToken, session);
        }

        /// <summary>
        /// 更新会话信息
        /// </summary>
        /// <param name="tradeToken"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool UpdateSession(string tradeToken, TradeSession session)
        {
            if (string.IsNullOrWhiteSpace(tradeToken))
            {
                return false;
            }
            string key = GetRedisKey(tradeToken);
            if (session.ExpireTime.TotalMinutes > 0)
            {
                return this.distributedCache.Set<TradeSession>(key, session, session.ExpireTime);
            }
            return this.distributedCache.Set<TradeSession>(key, session);
        }

        /// <summary>
        /// 获取期货帐号对应的所有有效的TradeToken
        /// </summary>
        /// <param name="brokerType"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public List<string> GetAllToken(int compCounter, string accountID)
        {
            var allSession = GetAllSession(compCounter, accountID);
            if (allSession == null)
            {
                return null;
            }
            return allSession.Select(s => s.TradeToken).ToList();
        }

        private List<string> InnerGetAllToken(int compCounter, string accountID)
        {
            var brokerAccountKey = GetBrokerAccountKey(compCounter, accountID);
            return this.distributedCache.Get<List<string>>(brokerAccountKey);
        }

        /// <summary>
        /// 获取期货帐号对应的所有有效的Session信息
        /// </summary>
        /// <param name="brokerType"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public List<TradeSession> GetAllSession(int compCounter, string accountID)
        {
            var allToken = InnerGetAllToken(compCounter, accountID);
            if (null == allToken)
            {
                return null;
            }

            var list = new List<TradeSession>();
            foreach (var token in allToken)
            {
                var session = GetSession(token);
                if (session != null)
                {
                    list.Add(session);
                }
            }

            if (allToken.Count != list.Count)
            {
                allToken = list.Select(s => s.TradeToken).ToList();
                this.distributedCache.Set(GetBrokerAccountKey(compCounter, accountID), allToken);
            }

            return list;
        }

        public TradeSession GetSession(string tradeToken)
        {
            if (string.IsNullOrWhiteSpace(tradeToken))
            {
                return null;
            }

            var value = this.distributedCache.Get<TradeSession>(GetRedisKey(tradeToken));
            return value;
        }

        /// <summary>
        /// 移除Session信息，并更新期货帐号下的tradeToken列表
        /// </summary>
        /// <param name="tradeToken"></param>
        /// <returns></returns>
        public bool RemoveSession(string tradeToken)
        {
            if (string.IsNullOrWhiteSpace(tradeToken))
            {
                return false;
            }

            var session = GetSession(tradeToken);
            if (null == session)
            {
                return false;
            }

            var allToken = InnerGetAllToken(session.CompCounter, session.BrokerAccount);
            if (allToken != null)
            {
                allToken.Remove(tradeToken);
                this.distributedCache.Set(GetBrokerAccountKey(session.CompCounter, session.BrokerAccount), allToken);
            }

            string key = GetRedisKey(tradeToken);
            return this.distributedCache.Remove(key);
        }


        private static string GetRedisKey(string key)
        {
            return "TradeSession:" + key;
        }

        private static string GetBrokerAccountKey(int compCounter, string accountID)
        {
            return string.Format("TradeAcctSession:{0}:{1}", compCounter, accountID);
        }
    }
}
