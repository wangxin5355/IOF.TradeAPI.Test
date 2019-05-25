using System;

namespace IQF.Trade.Core.BankArg
{
    /// <summary>
    /// 银行余额
    /// </summary>
    [Serializable]
	public class BankBalance
	{
		/// <summary>
		/// 银行代码
		/// </summary>
		public string BankID { get; set; }

		/// <summary>
		/// 余额
		/// </summary>
		public double Balane { get; set; }
	}
}
