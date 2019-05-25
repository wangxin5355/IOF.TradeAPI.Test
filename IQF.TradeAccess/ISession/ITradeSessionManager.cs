using IQF.Framework.DynamicProxy;
using System;
using System.Collections.Generic;

namespace IQF.TradeAccess.ISession
{
    public interface ITradeSessionManager : IProxyService
    {
        List<TradeSession> GetAllSession(int compCounter, string accountID);
        List<string> GetAllToken(int compCounter, string accountID);
        TradeSession GetSession(string tradeToken);
        bool RemoveSession(string tradeToken);
        bool SaveSession(string tradeToken, TradeSession session);
        bool UpdateSession(string tradeToken, TradeSession session);
    }

    /// <summary>
    /// 用户登录交易信息
    /// </summary>
    public class TradeSession
    {
        public TradeSession()
        {
            this.CreateTime = DateTime.Now;
            this.LastAccessTime = DateTime.Now;
        }

        public long UserID { get; set; }

        public string Mobile { get; set; }

        public string TradeToken { get; set; }

        public int BrokerType { get; set; }

        public string BrokerAccount { get; set; }

        public int CompCounter { get; set; }

        public string Password { get; set; }

        public string RpcAddr { get; set; }

        public DateTime CreateTime { get; private set; }

        public DateTime LastAccessTime { get; set; }

        public string Mac { get; set; }

        public string IP { get; set; }

        public string PackType { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// （未访问）失效时间
        /// </summary>
        public TimeSpan ExpireTime { get; set; }
    }
}
