

namespace IQF.Trade.ClientApi.Account
{
    [TradeApiInfo("/api/account/login")]
    public class LoginReq : TradeRequest
    {
        public string BrokerID { get; set; }
        /// <summary>
        /// 账户编号
        /// </summary>
        public string AccountID { get; set; }

        /// <summary>
        /// 客户端Mac地址
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// 设备类型：iPhone :1000 Android:1001 PC:1002
        /// </summary>
        public string PackType { get; set; }

        /// <summary>
        /// 终端系统信息
        /// </summary>
        public string SystemInfo { get; set; }

        /// <summary>
        /// 终端系统信息完整度
        /// </summary>
        public string SysInfoIntegrity { get; set; }

        /// <summary>
        /// 加密密钥版本
        /// </summary>
        public string EncrypKeyVersion { get; set; }

        /// <summary>
        /// （终端采集）异常标识
        /// </summary>
        public string ExceptionFlag { get; set; }

        /// <summary>
        /// 终端公网端口号
        /// </summary>
        public int ClientPort { get; set; }
    }

    public class LoginResp : TradeResponse
    {
        /// <summary>
        /// Api接口地址
        /// </summary>
        public string ApiAddr { get; set; }
    }
}
