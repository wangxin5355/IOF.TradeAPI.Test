
using System.Collections.Generic;

namespace IQF.Trade.ClientApi.Order
{
    [TradeApiInfo("/api/order/qryorder")]
    public class QryOrderReq : TradeRequest
    {
    }

    public class QryOrderResp : TradeResponse
    {
        public List<OrderEx> Orders { get; set; }
    }
}
