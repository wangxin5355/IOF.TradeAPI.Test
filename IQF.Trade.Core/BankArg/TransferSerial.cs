using System;

namespace IQF.Trade.Core.BankArg
{
    /// <summary>
    /// 转账流水信息
    /// </summary>
    [Serializable]
	public class TransferSerial
	{
		/// <summary>
		/// 交易代码
		/// </summary>
		public string TradeCode { get; set; }

		/// <summary>
		/// 交易时间
		/// </summary>
		public DateTime TradeTime { get; set; }

		/// <summary>
		/// 银行代码
		/// </summary>
		public string BankID { get; set; }

		/// <summary>
		/// 银行名称
		/// </summary>
		public string BankName { get; set; }

		/// <summary>
		/// 银行帐号
		/// </summary>
		public string BankAccount { get; set; }

		/// <summary>
		/// 交易金额
		/// </summary>
		public double TradeAmount { get; set; }

		/// <summary>
		/// 转账类型
		/// </summary>
		public TransferType TransferType { get; set; }

		/// <summary>
		/// 处理状态（0：成功，1：失败）
		/// </summary>
		public int EntrustNo { get; set; }

		/// <summary>
		/// 处理状态名称
		/// </summary>
		public string EntrustName { get; set; }

		/// <summary>
		/// 银行错误信息
		/// </summary>
		public string ErrorMsg { get; set; }
	}
}
