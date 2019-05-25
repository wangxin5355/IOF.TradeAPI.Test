

namespace IQF.Trade.ClientApi.Bank
{
    [TradeApiInfo("/api/bank/getcontractbank")]
    public class ContractBankReq : TradeRequest
    {
    }

    public class ContractBankResp : TradeResponse
    {
        public ContractBank ContractBank { get; set; }
    }
}
