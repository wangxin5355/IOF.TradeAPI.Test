using IQF.Framework.Dao;
using System;

namespace IQF.TradeAccess.Entity
{
    public partial class OrderLogEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BrokerAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BrokerType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Exchange { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderSide { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 委托编号
        /// </summary>
        public string OderID { get; set; }
        /// <summary>
        /// 业务类型（0：委托，1：撤单）
        /// </summary>
        public int BizType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ErrorNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PackType { get; set; }
        /// <summary>
        /// 交易日
        /// </summary>
        public DateTime TradeDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime { get; set; }
    } //end of class

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

} //end of namespace
