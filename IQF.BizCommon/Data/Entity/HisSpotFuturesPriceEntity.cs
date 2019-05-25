using System;

namespace IQF.BizCommon.Data.Entity
{
    public class HisSpotFuturesPriceEntity
    {
        /// <summary>
        /// 品种代码
        /// </summary>
        public string VarietyCode { get; set; }
        public string VarietyName { get; set; }
        public int VarietyId { get; set; }
        /// <summary>
        /// 现货价格
        /// </summary>
        public float SpotPrice { get; set; }
        /// <summary>
        /// 最新期货代码
        /// </summary>
        public string LastFutureSymbol { get; set; }
        /// <summary>
        /// 最新期货价格
        /// </summary>
        public float LastFuturePrice { get; set; }
        /// <summary>
        /// 主力期货代码
        /// </summary>
        public string MainFutureSymbol { get; set; }
        /// <summary>
        /// 主力期货价格
        /// </summary>
        public float MainFuturePrice { get; set; }
        /// <summary>
        /// 行情日期
        /// </summary>
        public DateTime HqTime { get; set; }
    }
}