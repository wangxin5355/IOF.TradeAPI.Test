using IQF.Framework.Dao;
using System;

namespace IQF.TradeAccess.Entity
{
    public partial class AccountBindingEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long BindingID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long TradeAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime { get; set; }
    } //end of class
} //end of namespace
