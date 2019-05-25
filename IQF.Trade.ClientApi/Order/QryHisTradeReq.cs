
using System;
using System.Collections.Generic;

namespace IQF.Trade.ClientApi.Order
{
    [TradeApiInfo("/api/order/qryhistrade")]
    public class QryHisTradeReq : TradeRequest
    {
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 请求行数
        /// </summary>
        public int RequestNum { get; set; }
    }

    public class QryHisTradeResp : TradeResponse
    {
        public List<TradeInfo> Trades { get; set; }
    }
}
