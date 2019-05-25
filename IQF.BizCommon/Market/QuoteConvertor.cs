using IQF.BizCommon.Helper;
using System;
using System.Collections.Generic;

namespace IQF.BizCommon.Market
{
	/// <summary>
	/// 解析行情数据
	/// </summary>
	public class QuoteConvertor
    {
        private static Snapshot snap1 = new Snapshot();
        private static int iQuote4TransLen = 188;
        /// <summary>
        /// built all length  and (in quote len) and (deal len);
        /// </summary>
        private static int headerbyteLen = 4 + 4 + 4 + 4 + 1;
        private static int iQuote4TransLen_withheader = PacketFactory.GetPackSize(snap1) + headerbyteLen;
        private static int restLen = 0;
        private static byte[] restBuf = new byte[iQuote4TransLen_withheader];

        /// <summary>
        /// 解析行情数据
        /// </summary>
        /// <param name="btData">输入数据</param>
        /// <param name="inputLen">输入数据长度</param>
        /// <param name="outputQuote">输出行情对象数组</param>
        /// <param name="proced">已处理的数据字节数</param>
        public static List<InternalQuoteMsg> ToQuoteMsgList(byte[] btData, int inputLen, out int proced)
        {
            var result = new List<InternalQuoteMsg>();
            int i = 0;
            if ((inputLen - i) > iQuote4TransLen_withheader)
            {
                for (; i < inputLen;)
                {
                    if ((inputLen - i) > iQuote4TransLen_withheader)
                    {
                        if (btData[i] == (byte)('H') && btData[i + 1] == (byte)('X') && btData[i + 2] == (byte)('0'))
                        {
                            i = i + 12;
                            if (i + iQuote4TransLen < inputLen)
                            {
                                byte[] qbs = new byte[iQuote4TransLen];
                                Array.Copy(btData, i, qbs, 0, iQuote4TransLen);
                                Snapshot snap = (Snapshot)PacketFactory.ReadBody(qbs, typeof(Snapshot));
                                if (snap.LastPx != (float)0 ||
                                    snap.ClosePx != (float)0 || snap.bidUnits[0].px != (float)0 || snap.bidUnits[0].size != (float)0
                                    || snap.askUnits[0].px != (float)0 || snap.askUnits[0].size != (float)0)
                                {
                                    InternalQuoteMsg quoteMsg = new InternalQuoteMsg { lev1 = snap };
                                    if (snap.ClosePx != 0F && snap.OpenPx == 0F)
                                    {
                                        quoteMsg.lev1.OpenPx = snap.ClosePx;
                                    }

                                    quoteMsg.symbol = snap.symbol;
                                    quoteMsg.nTime = snap.nTime;
                                    lock (result)
                                    {
                                        result.Add(quoteMsg);
                                    }
                                }
                                i += iQuote4TransLen + 5;
                            }
                            else
                                break;
                        }
                        else
                        {
                            bool find4 = false;
                            int j = i;
                            for (; j < inputLen; j++)
                            {
                                if (btData[j] == (byte)'$')
                                {
                                    find4 = true;
                                    i = j + 1;
                                    break;
                                }
                            }

                            if (!find4)
                            {
                                i = j;
                            }

                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            proced = i;
            if (inputLen - i > 0)
            {
                restLen = inputLen - i;

                if (restLen > iQuote4TransLen_withheader)
                {
                    System.Diagnostics.Debug.Assert(false);
                }

                restBuf = new byte[iQuote4TransLen_withheader];

                Array.Copy(btData, i, restBuf, 0, inputLen - i);
            }
            else
            {
                restLen = 0;
                restBuf = null;
            }

            return result;
        }
    }
}
