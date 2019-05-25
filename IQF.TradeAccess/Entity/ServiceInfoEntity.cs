using IQF.Framework.Dao;
using System;

namespace IQF.TradeAccess.Entity
{
    public partial class ServiceInfoEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
		/// 进程ID
		/// </summary>
		public int ProcessID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ServerPort { get; set; }
        /// <summary>
        /// 实时用户数
        /// </summary>
        public int UserCount { get; set; }
        /// <summary>
        /// 最大用户数
        /// </summary>
        public int MaxUsers { get; set; }
        /// <summary>
        /// 期货公司柜台
        /// </summary>
        public long CompCounter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ApiInfoID { get; set; }
        /// <summary>
        /// 0：正常
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime { get; set; }
    } //end of class
} //end of namespace
