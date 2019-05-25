
using System.Collections.Generic;

namespace IQF.Trade.ClientApi.Bank
{
    [TradeApiInfo("/api/bank/getcontractbanklist")]
    public class ContractBankListReq : TradeRequest
    {
    }

    public class ContractBankListResp : TradeResponse
    {
        public List<ContractBank> BankList { get; set; }
    }
}
