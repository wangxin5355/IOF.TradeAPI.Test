using IQF.Framework.Cache;
using IQF.TradeAccess.Entity;
using IQF.TradeAccess.ISession;

namespace IQF.TradeAccess.Session
{
    public class LoginAccountManager : ILoginAccountManager
    {
        private readonly IDistributedCache distributedCache;

        public LoginAccountManager(IDistributedCacheFactory distributedCacheFactory)
        {
            this.distributedCache = distributedCacheFactory.Create(DistributedCacheName.Redis_IQFTrade);
        }

        /// <summary>
        /// 保存帐号信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool SetAccountInfo(LoginAccountInfo info)
        {
            string key = GetAccountKey(info.BrokerType, info.BrokerAccount);
            return this.distributedCache.Set<LoginAccountInfo>(key, info);
        }

        /// <summary>
        /// 获取帐号信息
        /// </summary>
        /// <param name="brokerType"></param>
        /// <param name="brokerAccount"></param>
        /// <returns></returns>
        public LoginAccountInfo GetAccountInfo(BrokerType brokerType, string brokerAccount)
        {
            string key = GetAccountKey(brokerType, brokerAccount);
            return this.distributedCache.Get<LoginAccountInfo>(key);
        }
        private static string GetAccountKey(BrokerType brokerType, string accountID)
        {
            return string.Format("IQFAccount:{0}:{1}", (int)brokerType, accountID);
        }
    }
}
