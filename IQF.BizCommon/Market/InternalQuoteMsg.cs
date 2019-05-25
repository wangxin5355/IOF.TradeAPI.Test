using IQF.Framework;
using IQF.Framework.Util;
using System;
using System.Runtime.InteropServices;

namespace IQF.BizCommon.Market
{

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PRICE_VOLUME
    {
        [MarshalAs(UnmanagedType.R4)]
        public float px;                                //price
        [MarshalAs(UnmanagedType.I4)]
        public int size;                                //volume 
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Snapshot
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] symbol;

        /// <summary>
        /// 交易所   大商所 1,上期所 2,郑商所 3,中金所 4
        /// </summary>
        [MarshalAs(UnmanagedType.I8)]
        public Int64 ExchangeId;

        /// <summary>
        /// 前结算
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public float PreSettlementPrice;

        /// <summary>
        /// 前收盘
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public float PreClosePrice;

        /// <summary>
        /// 昨持仓
        /// </summary>
        [MarshalAs(UnmanagedType.I8)]
        public Int64 PreOpenInterest;

        /// <summary>
        /// 结算价
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public float SettlementPrice;

        /// <summary>
        /// 涨停价
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public float UpperLimitPrice;

        /// <summary>
        /// 跌停价
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public float LowerLimitPrice;

        /// <summary>
        /// 昨虚实度
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public float PreDelta;

        /// <summary>
        /// 今虚实度
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public float CurrDelta;

        /// <summary>
        /// 成交均价
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public float AveragePrice;

        /// <summary>
        /// 持仓量
        /// </summary>
        [MarshalAs(UnmanagedType.I8)]
        public Int64 OpenInterest;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public PRICE_VOLUME[] bidUnits = new PRICE_VOLUME[5];             //买


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public PRICE_VOLUME[] askUnits = new PRICE_VOLUME[5];             //卖

        [MarshalAs(UnmanagedType.R4)]
        public float OpenPx;                               //open price

        [MarshalAs(UnmanagedType.R4)]
        public float HighPx;                               //high price

        [MarshalAs(UnmanagedType.R4)]
        public float LowPx;                                //low price

        [MarshalAs(UnmanagedType.R4)]
        public float LastPx;                               //last price

        /// <summary>
        /// 成交量
        /// </summary>
        [MarshalAs(UnmanagedType.I8)]
        public Int64 liTotalVolumeTrade;                   //成交量

        /// <summary>
        /// 成交额
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public float liTotalValueTrade;                    //turnover (3 decimals) 对于期货,这里放的是持仓量       

        [MarshalAs(UnmanagedType.R4)]
        public float ClosePx;                         //preclose

        /// <summary>
        ///  UTC时间戳， 格林威治时间
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int nTime;

    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class InternalQuoteMsg
    {
        private readonly string quoteID = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 行情编号
        /// </summary>
        public string QuoteID
        {
            get
            {
                return this.quoteID;
            }
        }

        //行情类型
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] symbol;		//股票代码 

        /// <summary>
        ///  UTC时间， 格林威治时间
        /// </summary>
        public int nTime;  // unix时间戳
        //public allStructs allstructs;
        public Snapshot lev1;

        public string GetSymbol()
        {
            int len = 0;
            while (symbol[len] != '\0' && len < symbol.Length)
            {
                len++;
            }
            string ss = new string(symbol, 0, len);
            return ss;
        }

        /// <summary>
        /// 获取北京时间
        ///  - 
        /// DateTime.ToString()格式： 年月日 yyyyMMdd 时分秒 HHmmss
        /// </summary>
        /// <returns></returns>
        public DateTime GetTime()
        {
            var dt = TimeZoneHelper.GetTimeBeijing(nTime);
            return dt;
        }

        /// <summary>
        /// 根据ExchangeId返回市场
        /// </summary>
        /// <returns></returns>
        public Exchange GetMarket()
        {
            Exchange mkt;
            switch (lev1.ExchangeId)
            {
                case 1:
                    mkt = Exchange.DCE;
                    break;
                case 2:
                    mkt = Exchange.SHFE;
                    break;
                case 3:
                    mkt = Exchange.CZCE;
                    break;
                case 4:
                    mkt = Exchange.CFFEX;
                    break;
	            case 5:
		            mkt = Exchange.INE;
		            break;
                default:
                    mkt = Exchange.NONE;
                    break;
            }
            return mkt;
        }
    };
}