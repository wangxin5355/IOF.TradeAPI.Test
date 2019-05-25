using IQF.Framework.Cache;
using IQF.TradeAccess.ISession;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IQF.TradeAccess.Session
{
    public class RpcCounterManager : IRpcCounterManager
    {
        private readonly IDistributedCache distributedCache;

        public RpcCounterManager(IDistributedCacheFactory distributedCacheFactory)
        {
            this.distributedCache = distributedCacheFactory.Create(DistributedCacheName.Redis_IQFTrade);
        }

        public List<RpcCounterModel> GetAll(long compCounterID)
        {
            string key = GetRedisKey(compCounterID);
            return this.distributedCache.Get<List<RpcCounterModel>>(key) ?? new List<RpcCounterModel>();
        }

        public bool RegRpcAddr(long compCounterID, long apiInfoID, string addr, int userCount, int processId)
        {
            if (string.IsNullOrWhiteSpace(addr))
            {
                return false;
            }

            string key = GetRedisKey(compCounterID);
            var models = this.distributedCache.Get<List<RpcCounterModel>>(key) ?? new List<RpcCounterModel>();

            //删除十分钟都没更新的服务
            var avlModels = models.Where(e => (DateTime.Now - e.LastUpdateTime).TotalSeconds <= 10 * 60).ToList();
            var model = avlModels.FirstOrDefault(f => f.ApiAddr == addr);
            if (model == null)
            {
                model = new RpcCounterModel();
                avlModels.Add(model);
            }
            model.ApiAddr = addr;
            model.UserCount = userCount;
            model.LastUpdateTime = DateTime.Now;
            model.CompCounter = compCounterID;
            model.ApiInfoID = apiInfoID;
            model.ProcessId = processId;

            return this.distributedCache.Set<List<RpcCounterModel>>(key, avlModels);
        }

        /// <summary>
        /// 删除某个期货柜台的Rpc地址
        /// </summary>
        /// <param name="compCounterID"></param>
        /// <returns></returns>
        public bool Remove(long compCounterID)
        {
            string key = GetRedisKey(compCounterID);
            return this.distributedCache.Remove(key);
        }

        private string GetRedisKey(long compCounterID)
        {
            return string.Format("Trade:Counter:{0}", compCounterID);
        }
    }
}
