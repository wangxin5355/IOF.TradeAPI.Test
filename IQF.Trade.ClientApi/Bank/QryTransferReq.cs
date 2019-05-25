

using System.Collections.Generic;

namespace IQF.Trade.ClientApi.Bank
{
    [TradeApiInfo("/api/bank/qrytransfer")]
    public class QryTransferReq : TradeRequest
    {
        /// <summary>
        /// 银行代码
        /// </summary>
        public string BankID { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public int StartDate { get; set; }

        /// <summary>
        /// 到期日期
        /// </summary>
        public int EndDate { get; set; }
    }

    public class QryTransferResp : TradeResponse
    {
        public List<TransferSerial> Data { get; set; }
    }
}
