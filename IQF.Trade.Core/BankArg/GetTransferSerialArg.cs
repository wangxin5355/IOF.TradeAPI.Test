using System;

namespace IQF.Trade.Core.BankArg
{
    /// <summary>
    /// 获取转账流水请求参数
    /// </summary>
    [Serializable]
	public class GetTransferSerialArg
	{
		/// <summary>
		/// 银行代码
		/// </summary>
		public string BankID { get; set; }

		/// <summary>
		/// 开始日期
		/// </summary>
		public int StartDate { get; set; }

		/// <summary>
		/// 到期日期
		/// </summary>
		public int EndDate { get; set; }
	}
}
