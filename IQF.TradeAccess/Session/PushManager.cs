using IQF.Framework.Cache;
using IQF.TradeAccess.ISession;
using System;

namespace IQF.TradeAccess.Session
{
    /// <summary>
    /// 推送Redis管理
    /// </summary>
    public class PushManager : IPushManager
    {
        private readonly IDistributedCache distributedCache;

        public PushManager(IDistributedCacheFactory distributedCacheFactory)
        {
            this.distributedCache = distributedCacheFactory.Create(DistributedCacheName.Redis_IQFTrade);
        }

        /// <summary>
        /// 保存Push键值
        /// </summary>
        /// <param name="pushKey"></param>
        /// <param name="invalidTime"></param>
        /// <returns></returns>
        public bool Save(string pushKey, DateTime invalidTime)
        {
            if (string.IsNullOrWhiteSpace(pushKey))
            {
                return false;
            }

            var key = GetKey(pushKey);
            var value = this.distributedCache.Get<string>(key);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return this.distributedCache.Set<string>(key, pushKey, invalidTime);
        }

        private static string GetKey(string key)
        {
            return "IQFTrade:Push:" + key;
        }
    }
}
