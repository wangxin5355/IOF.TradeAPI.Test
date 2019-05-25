
using System.Collections.Generic;

namespace IQF.Trade.ClientApi.Order
{
    [TradeApiInfo("/api/order/qrytrade")]
    public class QryTradeReq : TradeRequest
    {
    }

    public class QryTradeResp : TradeResponse
    {
        public List<TradeInfo> Trades { get; set; }
    }
}
