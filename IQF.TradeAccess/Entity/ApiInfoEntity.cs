using IQF.Framework.Dao;
using System;

namespace IQF.TradeAccess.Entity
{
    public class ApiInfoEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long ApiInfoID { get; set; }
        /// <summary>
        /// 期货公司柜台编号
        /// </summary>
        public long CompCounter { get; set; }
        /// <summary>
        /// 柜台配置信息
        /// </summary>
        public string CounterConfig { get; set; }
        /// <summary>
        /// 附加配置信息
        /// </summary>
        public string ConfigExt { get; set; }
        /// <summary>
        /// 0：下线，1：线上  2：灰度 
        /// </summary>
        public int Status { get; set; }

        public string Note { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
