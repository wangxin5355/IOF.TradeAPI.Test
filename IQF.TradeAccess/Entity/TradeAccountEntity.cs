using IQF.Framework.Dao;
using System;

namespace IQF.TradeAccess.Entity
{
    public partial class TradeAccountEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long TradeAccountID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BrokerType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BrokerAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long AgentID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OpenAcctMobile { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public TradeAccountSource Source { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }
    } //end of class

    /// <summary>
    /// 期货帐号来源
    /// </summary>
    public enum TradeAccountSource
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        UserLogin = 0,
        /// <summary>
        /// 管理后台添加
        /// </summary>
        Management = 1,
        /// <summary>
        /// 开户添加
        /// </summary>
        OpenAccount = 2
    }
} //end of namespace
