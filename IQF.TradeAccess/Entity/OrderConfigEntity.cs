using IQF.Framework;
using IQF.Framework.Dao;
using System;
using System.ComponentModel;

namespace IQF.TradeAccess.Entity
{
    public class OrderConfigEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long ConfigID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BrokerType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BrokerAccount { get; set; }
        /// <summary>
        /// 交易账户编号
        /// </summary>
        public long TradeAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OrderConfigType OrderConfigType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OrderType OrderType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// 交易委托配置类型
    /// </summary>
    public enum OrderConfigType
    {
        /// <summary>
        /// 手动下单
        /// </summary>
        [Description("手动下单")]
        ManualOrder = 1,
        /// <summary>
        /// 快速平仓
        /// </summary>
        [Description("快速平仓")]
        QuickClose = 2,
        /// <summary>
        /// 止盈平仓
        /// </summary>
        [Description("止盈平仓")]
        StopProfit = 3,
        /// <summary>
        /// 止损平仓
        /// </summary>
        [Description("止损平仓")]
        StopLoss = 4,
        /// <summary>
        /// 反手下单
        /// </summary>
        [Description("反手下单")]
        ReverseOrder = 5
    }
}
