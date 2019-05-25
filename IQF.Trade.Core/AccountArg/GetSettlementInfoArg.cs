using System;

namespace IQF.Trade.Core.AccountArg
{
    /// <summary>
    /// 获取结算单参数
    /// </summary>
    [Serializable]
	public class GetSettlementInfoArg
	{
		/// <summary>
		/// 起始日期
		/// </summary>
		public int BeginDate { get; set; }

		/// <summary>
		/// 到期日期
		/// </summary>
		public int EndDate { get; set; }
	}
}
