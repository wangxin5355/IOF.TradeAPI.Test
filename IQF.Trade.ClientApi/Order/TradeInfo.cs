
using IQF.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQF.Trade.ClientApi.Order
{
	/// <summary>
	/// 成交信息
	/// </summary>
	[Serializable]
	public class TradeInfo
	{
		/// <summary>
		/// 券商账户编号
		/// </summary>
		public string BrokerAccount { get; set; }

		/// <summary>
		/// 成交编号
		/// </summary>"
		public string ExecutionID { get; set; }

		/// <summary>
		/// 委托编号，交易所记录的
		/// </summary>
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
		/// 数量
		/// </summary>
		public int Volume { get; set; }

		/// <summary>
		/// 成交价格
		/// </summary>
		public double Price { get; set; }

		/// <summary>
		/// 成交时间
		/// </summary>
		public DateTime ExecutionTime { get; set; }

		/// <summary>
		/// 手续费
		/// </summary>
		public double Fee { get; set; }

		/// <summary>
		/// 平仓盈亏，根据开仓价格来计算
		/// </summary>
		public double DropIncome { get; set; }

		/// <summary>
		/// 主场单号
		/// </summary>
		public string ConfirmID { get; set; }

		/// <summary>
		/// 合同编号
		/// </summary>
		public string ReportID { get; set; }
	}
}
