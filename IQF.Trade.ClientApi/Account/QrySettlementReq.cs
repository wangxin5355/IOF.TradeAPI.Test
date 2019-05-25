

namespace IQF.Trade.ClientApi.Account
{
    [TradeApiInfo("/api/account/qrysettlement")]
    public class QrySettlementReq : TradeRequest
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public int BeginDate { get; set; }

        /// <summary>
        /// 到期日期
        /// </summary>
        public int EndDate { get; set; }
    }

    public class QrySettlementResp : TradeResponse
    {
        public string Text { get; set; }
    }
}
