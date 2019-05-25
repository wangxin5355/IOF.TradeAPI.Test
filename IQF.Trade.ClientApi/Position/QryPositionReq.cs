
using System.Collections.Generic;

namespace IQF.Trade.ClientApi.Position
{
    [TradeApiInfo("/api/position/qryposition")]
    public class QryPositionReq : TradeRequest
    {
    }

    public class QryPositionResp : TradeResponse
    {
        public List<PositionEx> Positions { get; set; }
    }
}
