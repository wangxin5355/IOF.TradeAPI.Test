

namespace IQF.Trade.ClientApi.Account
{
    [TradeApiInfo("/api/account/qryasset")]
    public class QryAssetReq : TradeRequest
    {
    }

    public class QryAssetResp : TradeResponse
    {
        public AssetInfoEx Asset { get; set; }
    }
}
