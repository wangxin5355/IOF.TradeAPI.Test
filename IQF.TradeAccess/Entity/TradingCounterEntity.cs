using IQF.Framework.Dao;

namespace IQF.TradeAccess.Entity
{
    public partial class TradingCounterEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public int CounterID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ExeName { get; set; }
        /// <summary>
        /// 单进程最大用户数
        /// </summary>
        public int MaxUserPerProc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Note { get; set; }
    } //end of class
} //end of namespace
