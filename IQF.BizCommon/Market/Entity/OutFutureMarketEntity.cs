namespace IQF.BizCommon.Market.Entity
{
	public class OutFutureMarketEntity
	{
		/// <summary>
		/// 日期 yyyyMMdd
		/// </summary>
		public string HqDate { get; set; }
		/// <summary>
		/// 时间 HHmmss
		/// </summary>
		public string HqTime { get; set; }
		public string ContractName { get; set; }
		public decimal NowV { get; set; }

		public decimal UpDown { get; set; }
		public decimal UpDownRate { get; set; }
		public string Symbol { get; set; }
		public int OrderNum { get; set; }
		public int Volume { get; set; }
		/// <summary>
		/// 最高价
		/// </summary>
		public decimal HighPx { get; set; }
		/// <summary>
		/// 最低价
		/// </summary>
		public decimal LowPx { get; set; }
		/// <summary>
		/// 昨收/昨结
		/// </summary>
		public decimal PreClosePx { get; set; }
		/// <summary>
		/// 开盘价
		/// </summary>
		public decimal OpenPx { get; set; }
		/// <summary>
		/// 申买价一
		/// </summary>
		public decimal BidPx1 { get; set; }
		/// <summary>
		/// 申买量一
		/// </summary>
		public int BidVol1 { get; set; }
		/// <summary>
		/// 申卖价一
		/// </summary>
		public decimal AskPx1 { get; set; }
		/// <summary>
		/// 申卖量一
		/// </summary>
		public int AskVol1 { get; set; }
		/// <summary>
		/// 持仓量
		/// </summary>
		public long OpenInterest { get; set; }
		public int VarietyID { get; set; }
	}
}