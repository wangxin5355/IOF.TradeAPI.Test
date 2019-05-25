using IQF.Framework.DynamicProxy;

namespace IQF.TradeAccess.ISession
{
    /// <summary>
    /// 柜台帐号管理
    /// </summary>
    public interface ICounterAccountManager : IProxyService
    {
        bool SetAccountInfo(CounterAccountInfo info);

        CounterAccountInfo GetAccountInfo(int counterID, string brokerAccount);
    }

    public class CounterAccountInfo
    {
        public string TradeToken { get; set; }

        /// <summary>
        /// 柜台编号
        /// </summary>
        public int CounterID { get; set; }

        public string BrokerAccount { get; set; }

        /// <summary>
        /// 加密的密码
        /// </summary>
        public string TradePwd { get; set; }

        /// <summary>
        /// 用户客户端IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 用户客户端Mac
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// 用户客户端PackType
        /// </summary>
        public string PackType { get; set; }
    }
}
