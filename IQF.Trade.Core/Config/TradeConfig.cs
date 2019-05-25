using IQF.BizCommon.Helper;

namespace IQF.Trade.Core.Config
{
    public class TradeConfig
    {
        /// <summary>
        /// API配置编号
        /// </summary>
        public long ApiInfoId { get; set; }

        /// <summary>
        /// 期货公司柜台编号
        /// </summary>
        public long CompCounter { get; set; }

        /// <summary>
        /// 期货公司类型
        /// </summary>
        public string BrokerType { get; set; }

        /// <summary>
        /// 0：线上  1：测试
        /// </summary>
        public int ServiceStatus { get; set; }

        public string ArdAppID { get; set; }

        public string IosAppID { get; set; }

        public string WinAppID { get; set; }

        public string GetAppID(int packType)
        {
            if (PackManager.IsAndroid(packType))
            {
                return ArdAppID;
            }
            else if (PackManager.IsIos(packType))
            {
                return IosAppID;
            }
            else if (PackManager.IsWindows(packType))
            {
                return WinAppID;
            }
            return ArdAppID;
        }

        public string ReplyAppID { get; set; }
    }
}
