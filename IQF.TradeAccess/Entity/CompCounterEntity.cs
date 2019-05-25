using IQF.Framework.Dao;

namespace IQF.TradeAccess.Entity
{
    public partial class CompCounterEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long CompCounterID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TradingCounter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long BrokerCompany { get; set; }
        /// <summary>
        /// 是否主席  1:true 0:false
        /// </summary>
        public int IsMain { get; set; }
        /// <summary>
        /// 是否由守护进程管理
        /// </summary>
        public int IsDeamon { get; set; }
        /// <summary>
        /// 柜台名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 0:下线 1:已上线 2:内测
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Note { get; set; }
    } //end of class
} //end of namespace
