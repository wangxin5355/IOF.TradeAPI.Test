using System;

namespace IQF.Trade.Core.BankArg
{
    /// <summary>
    /// 签约银行信息
    /// </summary>
    [Serializable]
	public class ContractBank
	{
		/// <summary>
		/// 银行代码
		/// </summary>
		public string BankID { get; set; }

		/// <summary>
		/// 银行名称
		/// </summary>
		public string BankName { get; set; }

		/// <summary>
		/// 银行账号
		/// </summary>
		public string BankAccount { get; set; }
	}
}
