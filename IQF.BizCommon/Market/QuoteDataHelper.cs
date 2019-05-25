using System;

namespace IQF.BizCommon.Market
{
    public class QuoteDataHelper
    {
        /// <summary>
        /// 沉淀资金
        /// </summary>
        /// <param name="openInterest">持仓量</param>
        /// <param name="lastPx">最新价</param>
        /// <param name="lot">单位手数</param>
        /// <param name="ratio">保证金比例</param>
        /// <returns></returns>
        public static double GetPrecipitationFunds(Int64 openInterest, float lastPx, int lot, double ratio)
        {
            return openInterest * lastPx * lot * ratio;
        }

        /// <summary>
        /// 获取资金流向
        /// </summary>
        /// <param name="openInterest">持仓</param>
        /// <param name="lastPx">最新价</param>
        /// <param name="preOpenInterest">昨持仓</param>
        /// <param name="preClosePx">昨收盘价</param>
        /// <param name="lot">交易单位</param>
        /// <param name="ratio">保证金比例</param>
        /// <returns></returns>
        public static double GetFundFlow(Int64 openInterest, float lastPx, Int64 preOpenInterest, float preClosePx,
            int lot, double ratio)
        {
            return (openInterest * lastPx - preOpenInterest * preClosePx) * lot * ratio;
        }

        /// <summary>
        /// 获取开平方向
        /// </summary>
        /// <param name="addPos">增仓</param>
        /// <param name="lastPx">最新价</param>
        /// <param name="sellPrice">卖价</param>
        /// <param name="buyPrice">买价</param>
        /// <returns></returns>
        public static string GetKaiPing(int addPos, float lastPx, float sellPrice, float buyPrice)
        {
            if (addPos == 0) return "换手";

            var color = 0;
            if (lastPx >= sellPrice) color = 1;
            if (lastPx <= buyPrice) color = -1;
            if (color == 1)
            {
                if (addPos > 0) return "多开";
                // if (addPos == 0) return "换手（对换）";
                if (addPos < 0) return "空平";
            }
            if (color == -1)
            {
                if (addPos > 0) return "空开";
                //if (addPos == 0) return "换手（空换）";
                if (addPos < 0) return "多平";
            }
            if (color == 0)
            {
                if (addPos > 0) return "双开";
                // if (addPos == 0) return "换手（双换）";
                if (addPos < 0) return "双平";
            }
            return "";
        }
    }
}