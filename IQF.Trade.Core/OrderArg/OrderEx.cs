using IQF.Framework;
using System;

namespace IQF.Trade.Core.OrderArg
{
    [Serializable]
	public class OrderEx
	{
		/// <summary>
		/// 用户自定义的委托单号
		/// </summary>
		public string ClientOrderID { get; set; }

		/// <summary>
		/// 券商账户编号
		/// </summary>
		public string BrokerAccount { get; set; }

		/// <summary>
		/// 委托编号
		/// </summary>"
		public string OrderID { get; set; }

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
		/// 开平仓
		/// </summary>
		public Offset Offset { get; set; }

		/// <summary>
		/// 委托数量
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// 委托价格
		/// </summary>
		public double Price { get; set; }

		/// <summary>
		/// 已成交数量
		/// </summary>
		public int Filled { get; set; }

		/// <summary>
		/// 委托时间
		/// </summary>
		public DateTime OrderTime { get; set; }

		/// <summary>
		/// 委托状态
		/// </summary>
		public OrderStatus Status { get; set; }

		/// <summary>
		/// 委托类型
		/// </summary>
		public OrderType OrderType { get; set; }

		/// <summary>
		/// 当前交易日
		/// </summary>
		public DateTime TradeDate { get; set; }

		/// <summary>
		/// 撤单或废单备注信息
		/// </summary>
		public string Note { get; set; }

		/// <summary>
		/// 主场单号
		/// </summary>
		public string ComfirmID { get; set; }

		/// <summary>
		/// 合同号
		/// </summary>
		public string ReportID { get; set; }

		/// <summary>
		/// （预冻结）手续费
		/// </summary>
		public double Fee { get; set; }

		/// <summary>
		/// （预冻结）保证金
		/// </summary>
		public double Margin { get; set; }

		/// <summary>
		/// 是否打开的订单
		/// </summary>
		/// <returns></returns>
		public bool IsOpen()
		{
			return this.Status != OrderStatus.Cancelled && this.Status != OrderStatus.Filled && this.Status != OrderStatus.Rejected;
		}
	}
}
