using System;

namespace IQF.Trade.ClientApi.Bank
{
    [Serializable]
	public class TransferInfo
	{
		public TransferType TransferType { get; set; }

		/// <summary>
		/// 盈宽账户编号
		/// </summary>
		public string InnerAccountID { get; set; }

		/// <summary>
		/// 券商账户编号
		/// </summary>
		public string BrokerAccount { get; set; }

		/// <summary>
		/// 银行代码
		/// </summary>
		public string BankID { get; set; }

		/// <summary>
		/// 银行帐号
		/// </summary>
		public string BankAccount { get; set; }

		/// <summary>
		/// 交易时间
		/// </summary>
		public DateTime TradeTime { get; set; }

		/// <summary>
		/// 银行流水号
		/// </summary>
		public string BankSerial { get; set; }

		/// <summary>
		/// 转账金额
		/// </summary>
		public double TradeAmount { get; set; }

		/// <summary>
		/// 应收客户费用
		/// </summary>
		public double CustFee { get; set; }

		/// <summary>
		/// 应收券商公司费用
		/// </summary>
		public double BrokerFee { get; set; }

		/// <summary>
		/// 发送方给接收方的信息
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 处理状态
		/// </summary>
		public TransferStatus TransferStatus { get; set; }
	}


	/// <summary>
	/// 转账类型
	/// </summary>
	public enum TransferType
	{
		None = 0,
		/// <summary>
		/// 银行转券商
		/// </summary>
		BankToBroker = 1,
		/// <summary>
		/// 券商转银行
		/// </summary>
		BrokerToBank = 2,
		/// <summary>
		/// 查询余额
		/// </summary>
		QryBalance = 3
	}

	/// <summary>
	/// 转账状态
	/// </summary>
	public enum TransferStatus
	{
		/// <summary>
		/// 正常
		/// </summary>
		Normal = 1,
		/// <summary>
		/// 取消
		/// </summary>
		Repealed = 2,
	}
}
