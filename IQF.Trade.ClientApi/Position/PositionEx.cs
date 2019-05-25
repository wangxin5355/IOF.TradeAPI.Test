
using IQF.Framework;
using System;

namespace IQF.Trade.ClientApi.Position
{
    [Serializable]
	public class PositionEx
	{
		/// <summary>
		/// 券商账户编号
		/// </summary>
		public string BrokerAccount { get; set; }

		/// <summary>
		/// 合约代码
		/// </summary>
		public string Symbol { get; set; }

		/// <summary>
		/// 交易所
		/// </summary>
		public Exchange Exchange { get; set; }

		/// <summary>
		/// 持仓总数量。今仓+昨仓
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// 冻结总数量。今仓+昨仓
		/// </summary>
		public int Frozen { get; set; }

		/// <summary>
		/// 持仓总成本（持仓均价*手数*单位）
		/// </summary>
		public double TotalCost { get; set; }

		/// <summary>
		/// 持仓方向
		/// </summary>
		public PosSide PosSide { get; set; }

		/// <summary>
		/// 持仓市值
		/// </summary>
		public double MarketValue { get; set; }

		/// <summary>
		/// 保证金
		/// </summary>
		public double Margin { get; set; }

		/// <summary>
		/// 今日持仓
		/// </summary>
		public int TodayPositon { get; set; }

		/// <summary>
		/// 今可用持仓
		/// </summary>
		public int TodayEnablePosition { get; set; }

		/// <summary>
		/// 开仓成本
		/// </summary>
		public double OpenCost { get; set; }

		/// <summary>
		/// 开仓均价
		/// </summary>
		public double OpenAvgPrice { get; set; }

		/// <summary>
		/// 持仓均价
		/// </summary>
		public double HoldAvgPrice { get; set; }

		/// <summary>
		/// 浮动盈亏
		/// </summary>
		public double DropIncomeFloat { get; set; }

		/// <summary>
		/// 盯市浮盈
		/// </summary>
		public double DropIncome { get; set; }
	}
}
