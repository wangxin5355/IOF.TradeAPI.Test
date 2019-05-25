using IQF.Framework.Dao;
using System;

namespace IQF.TradeAccess.Entity
{
    public partial class AgentEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long AgentID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 1：启用  0：禁用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime { get; set; }
    } //end of class
} //end of namespace
