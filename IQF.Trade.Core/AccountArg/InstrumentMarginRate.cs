using System;

namespace IQF.Trade.Core.AccountArg
{
    /// <summary>
    /// 合约保证金率
    /// </summary>
    [Serializable]
	public class InstrumentMarginRate
	{
		/// <summary>
		/// 合约代码
		/// </summary>
		public string Symbol { get; set; }

		/// <summary>
		/// 多头保证金率（按成交金额计算）
		/// </summary>
		public double LongMarginRatioByMoney { get; set; }

		/// <summary>
		/// 多头保证金费（按单位数量计算）
		/// </summary>
		public double LongMarginByVol { get; set; }

		/// <summary>
		/// 空头保证金率（按成交金额计算）
		/// </summary>
		public double ShortMarginRatioByMoney { get; set; }

		/// <summary>
		/// 空头保证金费（按单位数量计算）
		/// </summary>
		public double ShortMarginByVol { get; set; }
	}
}
