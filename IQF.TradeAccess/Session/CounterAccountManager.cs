using IQF.Framework.Cache;
using IQF.TradeAccess.ISession;

namespace IQF.TradeAccess.Session
{
    /// <summary>
    /// 柜台帐号管理
    /// </summary>
    public class CounterAccountManager : ICounterAccountManager
    {
        private readonly IDistributedCache distributedCache;

        public CounterAccountManager(IDistributedCacheFactory distributedCacheFactory)
        {
            this.distributedCache = distributedCacheFactory.Create(DistributedCacheName.Redis_IQFTrade);
        }

        /// <summary>
		/// 保存帐号信息
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public bool SetAccountInfo(CounterAccountInfo info)
        {
            string key = GetAccountKey(info.CounterID, info.BrokerAccount);
            return this.distributedCache.Set<CounterAccountInfo>(key, info);
        }

        public CounterAccountInfo GetAccountInfo(int counterID, string brokerAccount)
        {
            string key = GetAccountKey(counterID, brokerAccount);
            return this.distributedCache.Get<CounterAccountInfo>(key);
        }

        private static string GetAccountKey(int counterID, string brokerAccount)
        {
            return string.Format("Account:{0}:{1}", counterID, brokerAccount);
        }
    }

}
