

namespace IQF.Trade.ClientApi.Bank
{
    [TradeApiInfo("/api/bank/banktobroker")]
    public class BankToBrokerReq : TradeRequest
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
        /// 银行卡密码
        /// </summary>
        public string BankPassword { get; set; }

        /// <summary>
        /// 资金密码
        /// </summary>
        public string FundPassword { get; set; }
    }

    public class BankToBrokerResp : TradeResponse
    {
        public TransferInfo TransferInfo { get; set; }
    }
}
