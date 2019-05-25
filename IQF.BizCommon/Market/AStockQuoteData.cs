using IQF.Framework.Serialization;
using IQF.Framework.Util;
using System.Collections.Generic;

namespace IQF.BizCommon.Market
{
    public class AStockQuoteData
    {
        private const string ApiHost = "https://inapi.inquant.cn/astockdata/";

        /// <summary>
        /// 根据A股内码获取行情数据
        /// </summary>
        /// <param name="innerCodeList">股票内码集合</param>
        /// <returns></returns>
        public static List<AStockQuoteDetail> GetAStockQuoteByCode(List<long> innerCodeList)
        {
            if (innerCodeList == null || innerCodeList.Count <= 0)
            {
                return null;
            }
            var apiUrl = string.Format("{0}/basicInfo/GetAStockQuoteByCode.ashx?innerCodeList={1}", ApiHost, string.Join(",", innerCodeList));
            var resp = HttpWebResponseUtility.HttpGet(apiUrl);
            JsonString js = new JsonString(resp);
            if (js.GetInt("code") != 0)
            {
                return null;
            }
            var arrData = js.GetArray("data");
            var allDetail=new List<AStockQuoteDetail>();
            foreach (var item in arrData)
            {
                var detail=new AStockQuoteDetail();
                detail.Innercode = item.Get("innercode");
                detail.Stockcode = item.Get("stockcode");
                detail.Stockname = item.Get("stockname");
                detail.Nowv = item.Get("nowv");
                detail.updown = item.Get("updown");
                detail.updownrate = item.Get("updownrate");
                detail.ask1p = item.Get("ask1p");
                detail.bid1p = item.Get("bid1p");
                detail.litotalvolumetrade = item.Get("litotalvolumetrade");
                detail.litotalvaluetrade = item.Get("litotalvaluetrade");
                detail.highp = item.Get("highp");
                detail.lowp = item.Get("lowp");
                detail.openp = item.Get("openp");
                detail.preclose = item.Get("preclose");
                detail.turnoverrate = item.Get("turnoverrate");
                detail.qratio = item.Get("qratio");
                detail.innervol = item.Get("innervol");
                detail.outervol = item.Get("outervol");
                detail.market = item.Get("market");
                allDetail.Add(detail);
            }

            return allDetail;
        }
    }

    public class AStockQuoteDetail
    {
        /// <summary>
        /// 内码
        /// </summary>
        public string Innercode { get; set; }
        /// <summary>
        /// 交易代码
        /// </summary>
        public string Stockcode { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Stockname { get; set; }
        public string Nowv { get; set; }
        public string updown { get; set; }
        public string updownrate { get; set; }
        public string ask1p { get; set; }
        public string bid1p { get; set; }
        /// <summary>
        /// 成交量
        /// </summary>
        public string litotalvolumetrade { get; set; }
        /// <summary>
        /// 成交额
        /// </summary>
        public string litotalvaluetrade { get; set; }
        public string highp { get; set; }
        public string lowp { get; set; }
        public string openp { get; set; }
        public string preclose { get; set; }
        public string turnoverrate { get; set; }
        /// <summary>
        /// 量比
        /// </summary>
        public string qratio { get; set; }
        /// <summary>
        /// 内盘
        /// </summary>
        public string innervol { get; set; }
        /// <summary>
        /// 外盘
        /// </summary>
        public string outervol { get; set; }
        public string market { get; set; }

        public AStockQuoteDetail()
        {
            this.Nowv = "--";
            this.updown = "--";
            this.updownrate = "--";
            this.ask1p = "--";
            this.bid1p = "--";
            this.litotalvaluetrade = "--";
            this.litotalvolumetrade = "--";
            this.highp = "--";
            this.lowp = "--";
            this.openp = "--";
            this.preclose = "--";
            this.turnoverrate = "--";
            this.qratio = "--";
            this.outervol = "--";
            this.innervol = "--";
            this.market = "";

        }
    }
}