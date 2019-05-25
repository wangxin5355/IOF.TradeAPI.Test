using System;

namespace IQF.Trade.Core
{
    /// <summary>
    /// 适配器账户配置信息
    /// 这个接口可能需要经常新增字段
    /// </summary>
    public class AdapterAccountCfg
    {
        /// <summary>
        /// 券商账户
        /// </summary>
        public string BrokerAccount { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIP { get; set; }
        /// <summary>
        /// 客户端Mac地址
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 设备类型：iPhone :1000 Android:1001 PC:1002
        /// </summary>
        public string PackType { get; set; }

        /// <summary>
        /// 交易会话
        /// </summary>
        public string TradeToken { get; set; }

        /// <summary>
        /// 期货公司类型
        /// </summary>
        public int BrokerType { get; set; }

        /// <summary>
        /// （穿透式监管）采集信息
        /// </summary>
        public CollectionInfo CollectionInfo { get; set; }
    }

    public class CollectionInfo
    {
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

        /// <summary>
        /// 终端登录时间
        /// </summary>
        public DateTime LoginTime { get; set; } = DateTime.MinValue;
    }
}
