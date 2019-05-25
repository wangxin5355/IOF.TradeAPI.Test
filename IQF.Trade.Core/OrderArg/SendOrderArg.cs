using IQF.Framework;
using System;

namespace IQF.Trade.Core.OrderArg
{
    /// <summary>
    /// 下单请求参数定义
    /// </summary>
    [Serializable]
	public class SendOrderArg
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
	}
}
