using System;

namespace IQF.BizCommon.Market.Entity
{
    public class MainContractDayKline
    {
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        /// <summary>
        /// yyyyMMddHHmmss格式的时间
        /// </summary>
        public DateTime KlineDate { get; set; }
        /// <summary>
        /// 高价
        /// </summary>
        public float HighP { get; set; }
        /// <summary>
        /// 开盘
        /// </summary>
        public float OpenP { get; set; }
        /// <summary>
        /// 低价
        /// </summary>
        public float LowP { get; set; }
        /// <summary>
        /// 新价
        /// </summary>
        public float NowV { get; set; }
        /// <summary>
        /// 量
        /// </summary>
        public long CurVolume { get; set; }
        /// <summary>
        /// 持
        /// </summary>
        public long OpenInterest { get; set; }
        /// <summary>
        /// 结算价
        /// </summary>
        public float SettlementPrice { get; set; }
        /// <summary>
        /// 昨结
        /// </summary>
        public float PreClosePrice { get; set; }


    }
}