using IQF.Framework;
using IQF.Trade.Core.OrderArg;
using IQF.TradeAccess.ISession;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IQF.Trade.Core.Session
{
    /// <summary>
    /// 交易适配器容器
    /// </summary>
    public class SessionManager : ISessionManager
    {
        /// <summary>
        /// 会话临时缓存
        /// </summary>
        private readonly IMemoryCache sessionTempCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

        /// <summary>
        /// 锁
        /// </summary>
        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 适配器容器
        /// </summary>
        private readonly Dictionary<string, ITradeExAdapter> adapterCacheDict = new Dictionary<string, ITradeExAdapter>();

        private readonly ITradeSessionManager tradeSessionManager;

        private readonly IServiceProvider serviceProvider;

        public SessionManager(ITradeSessionManager tradeSessionManager,IServiceProvider serviceProvider)
        {
            this.tradeSessionManager = tradeSessionManager;
            this.serviceProvider = serviceProvider;
            var thr = new Thread(this.CheckSession);
            thr.Name = "CheckSession";
            thr.Start();
        }

        /// <summary>
        /// 获取适配器
        /// </summary>
        /// <param name="tradeToken"></param>
        /// <returns></returns>
        public ITradeExAdapter Get(string tradeToken)
        {
            try
            {
                rwLock.EnterReadLock();
                if (!adapterCacheDict.ContainsKey(tradeToken))
                {
                    return null;
                }
                return adapterCacheDict[tradeToken];
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// 获取会话数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return adapterCacheDict.Count;
        }

        /// <summary>
        /// 获取或创建适配器
        /// </summary>
        /// <param name="tradeToken"></param>
        /// <returns></returns>
        public ITradeExAdapter GetOrCreate(string tradeToken)
        {
            try
            {
                rwLock.EnterWriteLock();
                if (adapterCacheDict.ContainsKey(tradeToken))
                {
                    return adapterCacheDict[tradeToken];
                }
                var adapter = serviceProvider.GetService(typeof(ITradeExAdapter)) as ITradeExAdapter;
                if (adapter == null)
                {
                    return null;
                }
                adapter.OnOrderChanged += Adapter_OnOrderChanged;
                adapterCacheDict.Add(tradeToken, adapter);
                //临时存放刚创建的Token
                sessionTempCache.Set(tradeToken, DateTime.Now, DateTime.Now.AddSeconds(30));

                return adapter;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        private void Adapter_OnOrderChanged(OrderEx arg1, ITradeExAdapter arg2)
        {
            if (arg2 == null || arg2.AdapterAccountCfg == null)
            {
                return;
            }
            var tradeToken = arg2.AdapterAccountCfg.TradeToken;
            //ExecReportProcessor.OnOrderChanged(tradeToken, arg1);
        }

        /// <summary>
        /// 删除适配器
        /// </summary>
        /// <param name="tradeToken"></param>
        /// <returns>true表示找到并移除适配器，否则为false</returns>
        public bool Remove(string tradeToken)
        {
            try
            {
                rwLock.EnterWriteLock();
                if (!adapterCacheDict.ContainsKey(tradeToken))
                {
                    return false;
                }

                var adapter = adapterCacheDict[tradeToken];
                if (adapter != null)
                {
                    adapter.Dispose();
                }
                adapterCacheDict.Remove(tradeToken);
                return true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 检查会话有效性
        /// </summary>
        private void CheckSession()
        {
            while (true)
            {
                try
                {
                    var keys = GetAllKey();
                    foreach (var tradeToken in keys)
                    {
                        try
                        {
                            CheckSession(tradeToken);
                        }
                        catch (Exception ex1)
                        {
                            LogError(ex1.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                }

                Thread.Sleep(1000 * 60);
            }
        }

        /// <summary>
        /// 检查会话有效性
        /// </summary>
        /// <param name="tradeToken"></param>
        private void CheckSession(string tradeToken)
        {
            if (string.IsNullOrWhiteSpace(tradeToken))
            {
                return;
            }
            //刚创建的实例，tradeToken还未来得及存至Redis，暂缓移除
            if (sessionTempCache.Get(tradeToken) == null)
            {
                return;
            }
            var session = this.tradeSessionManager.GetSession(tradeToken);
            if (session == null)
            {
                bool del = Remove(tradeToken);
                LogInfo(tradeToken + "已从Redis中移除，移除本地会话：" + del);
                return;
            }
            //会话超时
            if (session.LastAccessTime.Add(session.ExpireTime) < DateTime.Now)
            {
                bool del1 = this.tradeSessionManager.RemoveSession(tradeToken);
                bool del2 = Remove(tradeToken);
                LogInfo(string.Format("{0}超过{1}分钟未访问。移除Redis：{2}，移除本地：{3}", tradeToken, session.ExpireTime.TotalMinutes, del1, del2));
                return;
            }
        }

        /// <summary>
        /// 获取所有的KEY
        /// </summary>
        /// <returns></returns>
        private List<string> GetAllKey()
        {
            try
            {
                rwLock.EnterReadLock();
                return adapterCacheDict.Keys.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }        

        private void LogInfo(string msg)
        {
            LogRecord.writeLogsingle("SessionManager.log", msg);
        }

        private void LogError(string error)
        {
            LogRecord.writeLogsingle("SessionManagerError.log", error);
        }

        private void LogFatal(string error)
        {
            LogRecord.writeLogsingle("SessionManagerFatal.log", error);
        }
    }
}
