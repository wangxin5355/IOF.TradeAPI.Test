using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;

namespace IQF.TradeAccess.ISession
{
    public interface ILoginAccountManager : IProxyService
    {
        bool SetAccountInfo(LoginAccountInfo info);

        LoginAccountInfo GetAccountInfo(BrokerType brokerType, string brokerAccount);
    }

    public class LoginAccountInfo
    {
        public BrokerType BrokerType { get; set; }

        public string BrokerAccount { get; set; }

        public string TradeToken { get; set; }
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
