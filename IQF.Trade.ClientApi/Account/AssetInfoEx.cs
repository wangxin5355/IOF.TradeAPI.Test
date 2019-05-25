
using IQF.Framework;
using System;

namespace IQF.Trade.ClientApi.Account
{
    [Serializable]
	public class AssetInfoEx 
	{
		/// <summary>
		/// 券商账户编号
		/// </summary>
		public string BrokerAccount { get; set; }

		/// <summary>
		/// 总资产
		/// </summary>
		public double TotalAsset { get; set; }

		/// <summary>
		/// 可用资金
		/// </summary>
		public double Available { get; set; }

		/// <summary>
		/// 可取资金
		/// </summary>
		public double Withdraw { get; set; }

		/// <summary>
		/// 现金余额
		/// </summary>
		public double CashBalance { get; set; }

		/// <summary>
		/// 保证金
		/// 期货等有意义，股票没有意义
		/// </summary>
		public double Margin { get; set; }

		/// <summary>
		/// 币种
		/// </summary>
		public Currency Currency { get; set; }

		/// <summary>
		/// 期初权益
		/// </summary>
		public double BeginBalance { get; set; }

		/// <summary>
		/// 转入金额
		/// </summary>
		public double InBalance { get; set; }

		/// <summary>
		/// 转出金额
		/// </summary>
		public double OutBalance { get; set; }

		/// <summary>
		/// 挂单（冻结）保证金
		/// </summary>
		public double FrozenMargin { get; set; }

		/// <summary>
		/// 冻结资金
		/// </summary>
		public double FrozenBalance { get; set; }

		/// <summary>
		/// 手续费
		/// </summary>
		public double Commission { get; set; }

		/// <summary>
		/// 冻结手续费
		/// </summary>
		public double FrozenCommission { get; set; }

		/// <summary>
		/// 盯市浮盈
		/// </summary>
		public double DropIncome { get; set; }

		/// <summary>
		/// 平仓盈亏
		/// </summary>
		public double CloseProfit { get; set; }
        /// <summary>
        /// 市值权益
        /// </summary>
        public double MarketBalance { get; set; }
        /// <summary>
        /// 权利金
        /// </summary>
        public double Premium { get; set; }
        /// <summary>
        /// 质入金额
        /// </summary>
        public double MortgageIn { get; set; }
        /// <summary>
        /// 质出金额
        /// </summary>
        public double MortgageOut { get; set; }
        /// <summary>
        /// 质押余额
        /// </summary>
        public double MortgageAvailable { get; set; }
        /// <summary>
        /// 可质押货币金额
        /// </summary>
        public double MortgageableFund { get; set; }
    }
}
