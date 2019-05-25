


using IQF.Framework;

namespace IQF.Trade.ClientApi.Order
{
    [TradeApiInfo("/api/order/sendorder")]
    public class SendOrderReq : TradeRequest
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public Exchange Exchange { get; set; }

        /// <summary>
        /// 买卖方向
        /// </summary>
        public OrderSide OrderSide { get; set; }

        /// <summary>
        /// 委托价格
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 委托数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 委托类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 开平仓
        /// </summary>
        public Offset Offset { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public OrderSource Source { get; set; }
    }

    public class SendOrderResp : TradeResponse
    {
        public string OrderID { get; set; }
    }

    public enum OrderSource
    {
        /// <summary>
        /// 手动委托
        /// </summary>
        SendOrder = 0,
        /// <summary>
        /// 快平
        /// </summary>
        QuickClose = 1,
        /// <summary>
        /// 止盈止损
        /// </summary>
        AutoStopPrice = 2,
        /// <summary>
        /// 云托管
        /// </summary>
        FollowOrder = 3,
        /// <summary>
        /// 条件单
        /// </summary>
        ConditionOrder = 4,
        /// <summary>
        /// 反手单
        /// </summary>
        ReverseOrder = 5
    }
}
