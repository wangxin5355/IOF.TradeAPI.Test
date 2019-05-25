
using IQF.Framework;
using System.Collections.Generic;

namespace IQF.Trade.ClientApi.Account
{
    [TradeApiInfo("/api/account/qrymarginrate")]
    public class QryMarginRateReq : TradeRequest
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public Exchange Exchange { get; set; }
    }

    public class QryMarginRateResp : TradeResponse
    {
        public List<InstrumentMarginRate> Data { get; set; }
    }
}
