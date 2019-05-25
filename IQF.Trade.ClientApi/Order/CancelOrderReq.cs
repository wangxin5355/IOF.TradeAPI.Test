

using IQF.Framework;

namespace IQF.Trade.ClientApi.Order
{
    [TradeApiInfo("/api/order/cancelorder")]
    public class CancelOrderReq : TradeRequest
    {
        /// <summary>
        /// 委托编号
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 交易所
        /// 用户不会传，自行映射
        /// </summary>
        public Exchange Exchange { get; set; }

        /// <summary>
        /// 合约代码
        /// 用户不会传，自行映射
        /// </summary>
        public string Symbol { get; set; }
    }
}
