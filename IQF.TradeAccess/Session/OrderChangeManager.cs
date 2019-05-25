using IQF.Framework.Cache;
using IQF.Framework.Serialization;
using IQF.TradeAccess.ISession;
using System;
using System.Collections.Generic;


namespace IQF.TradeAccess.Session
{
    public class OrderChangeManager : IOrderChangeManager
    {
        private readonly static string OrderChangeKey = "IQFTrade:OrderC";

        private readonly IDistributedCache distributedCache;

        public OrderChangeManager(IDistributedCacheFactory distributedCacheFactory)
        {
            this.distributedCache = distributedCacheFactory.Create(DistributedCacheName.Redis_IQFTrade);
        }

        public void Enqueue(OrderChangeInfo info, DateTime invalidTime)
        {
            if (null == info)
            {
                return;
            }
            string msg = JsonHelper.Serialize(info);
            this.distributedCache.EnqueueItemOnList<string>(OrderChangeKey, msg, invalidTime);
        }

        public OrderChangeInfo Dequeue()
        {
            var msg = this.distributedCache.DequeueItemFromList<string>(OrderChangeKey);
            if (string.IsNullOrWhiteSpace(msg))
            {
                return null;
            }
            return JsonHelper.Deserialize<OrderChangeInfo>(msg);
        }

        public List<string> GetAllList()
        {
            return this.distributedCache.GetAllItemsFromList<string>(OrderChangeKey);
        }

        public List<OrderChangeInfo> DequeueAll(int maxCount = 100)
        {
            var msgList = this.distributedCache.BatchRDequeue<string>(OrderChangeKey, maxCount);
            if (msgList == null || msgList.Count == 0)
            {
                return null;
            }
            var list = new List<OrderChangeInfo>();
            foreach (var msg in msgList)
            {
                OrderChangeInfo info = JsonHelper.Deserialize<OrderChangeInfo>(msg);
                if (null != info)
                {
                    list.Add(info);
                }
            }
            return list;
        }
    }
}
