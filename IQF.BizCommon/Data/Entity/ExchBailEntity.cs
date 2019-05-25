using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQF.BizCommon.Data.Entity
{
    /// <summary>
    /// 保证金
    /// </summary>
    public class ExchBailEntity
    {
        public ExchBailEntity()
        {
            Amount = 0;
            Code = string.Empty;
            Exch = string.Empty;
            Hedge = string.Empty;
            Ratio = 0m;
            Type = string.Empty;
            Exchange = 0;
            VarietyID = 0L;
        }
        public int Amount { get; set; }
        public string Code { get; set; }
        public string Exch { get; set; }
        public string Hedge { get; set; }
        public decimal Ratio { get; set; }
        public string Type { get; set; }
        public int Exchange { get; set; }
        public long VarietyID { get; set; }

    }//End class ExchBailEntity
}//End namespace IQF.BizCommon.Data.Entity
