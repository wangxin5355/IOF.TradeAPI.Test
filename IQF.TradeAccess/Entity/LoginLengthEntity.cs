using IQF.Framework.Dao;
using System;

namespace IQF.TradeAccess.Entity
{
    public partial class LoginLengthEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BrokerAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BrokerType { get; set; }
        /// <summary>
        /// 交易账户编码
        /// </summary>
        public long TradeAccount { get; set; }
        /// <summary>
        /// 有效时间（分钟）
        /// </summary>
        public int AvailableTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastModifyTime { get; set; }
    } //end of class
} //end of namespace
