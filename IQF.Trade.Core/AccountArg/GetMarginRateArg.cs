using IQF.Framework;
using System;

namespace IQF.Trade.Core.AccountArg
{
    /// <summary>
    /// 查询保证金率参数
    /// </summary>
    [Serializable]
	public class GetMarginRateArg
	{
		/// <summary>
		/// 合约代码
		/// </summary>
		public string Symbol { get; set; }

		/// <summary>
		/// 交易所
		/// </summary>
		public Exchange Exchange { get; set; }
	}
}
