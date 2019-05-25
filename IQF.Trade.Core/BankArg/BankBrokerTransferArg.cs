using System;

namespace IQF.Trade.Core.BankArg
{
    /// <summary>
    /// 银期转账请求参数
    /// </summary>
    [Serializable]
	public class BankBrokerTransferArg
	{
		/// <summary>
		/// 银行代码
		/// </summary>
		public string BankID { get; set; }

		/// <summary>
		/// 银行帐号
		/// </summary>
		public string BankAccount { get; set; }

		/// <summary>
		/// 资金密码
		/// </summary>
		public string FundPassword { get; set; }

		/// <summary>
		/// 转账金额
		/// </summary>
		public double TradeAmount { get; set; }

		/// <summary>
		/// 银行密码
		/// </summary>
		public string BankPassword { get; set; }
	}
}
