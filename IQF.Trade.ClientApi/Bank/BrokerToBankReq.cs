

namespace IQF.Trade.ClientApi.Bank
{
    [TradeApiInfo("/api/bank/brokertobank")]
    public class BrokerToBankReq : TradeRequest
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
        /// 转账金额
        /// </summary>
        public double TradeAmount { get; set; }

        /// <summary>
        /// 资金密码
        /// </summary>
        public string FundPassword { get; set; }

        /// <summary>
        /// 银行密码
        /// </summary>
        public string BankPassword { get; set; }
    }

    public class BrokerToBankResp : TradeResponse
    {
        public TransferInfo TransferInfo { get; set; }
    }
}
