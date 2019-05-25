using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQF.BizCommon.Data.Entity
{
    /// <summary>
    /// 交易手续费
    /// </summary>
    public class ExchFareEntity
    {
        public ExchFareEntity()
        {
            Amount = 0m;
            Code = string.Empty;
            Exch = string.Empty;
            FareType = 0;
            Ratio = 0m;
            SpecAmount = 0m;
            SpecRatio = 0m;
            Type = string.Empty;
            Exchange = 0;
            VarietyID = 0L;
        }

        /// <summary>
        /// 每手金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 合约编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 交易所代码
        /// </summary>
        public string Exch { get; set; }
        /// <summary>
        /// 手续费类型：1开仓，2平仓，3交割
        /// </summary>
        public int FareType { get; set; }
        /// <summary>
        /// 总金额比例
        /// </summary>
        public decimal Ratio { get; set; }
        /// <summary>
        /// 今仓每手金额(短线)
        /// </summary>
        public decimal SpecAmount { get; set; }
        /// <summary>
        /// 今仓总金额比例(短线开仓费率)
        /// </summary>
        public decimal SpecRatio { get; set; }
        /// <summary>
        /// 品种代码
        /// </summary>
        public string Type { get; set; }

        public int Exchange { get; set; }
        public long VarietyID { get; set; }

    }//End class ExchFareEntity
}//End namespace IQF.BizCommon.Data.Entity
