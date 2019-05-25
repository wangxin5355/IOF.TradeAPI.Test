using System;

namespace IQF.Trade.ClientApi
{
    public interface ITradeRequest
    {
        /// <summary>
        /// 交易Token
        /// </summary>
        string TradeToken { get; set; }
    }

    public class TradeRequest : ITradeRequest
    {
        /// <summary>
        /// 交易Token
        /// </summary>
        public string TradeToken { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 客户端请求IP
        /// </summary>
        public string ClientIP { get; set; }
    }

    /// <summary>
    /// 交易接口信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TradeApiInfoAttribute : Attribute
    {
        public TradeApiInfoAttribute(string apiUrl)
        {
            this.ApiUrl = apiUrl;
        }

        public string ApiUrl { get; private set; }
    }

}
