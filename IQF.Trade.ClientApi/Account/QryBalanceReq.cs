

namespace IQF.Trade.ClientApi.Account
{
    [TradeApiInfo("/api/account/qrybalance")]
    public class QryBalanceReq : TradeRequest
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
        /// 银行密码
        /// </summary>
        public string BankPassword { get; set; }

        /// <summary>
        /// 资金密码
        /// </summary>
        public string FundPassword { get; set; }
    }

    public class QryBalanceResp : TradeResponse
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
