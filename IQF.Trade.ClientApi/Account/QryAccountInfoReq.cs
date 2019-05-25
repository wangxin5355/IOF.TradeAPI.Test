

namespace IQF.Trade.ClientApi.Account
{
    [TradeApiInfo("/api/account/qryaccountinfo")]
    public class QryAccountInfoReq : TradeRequest
    {
    }

    public class QryAccountInfoResp : TradeResponse
    {
        public AccountInfo AccountInfo { get; set; }
    }
}
