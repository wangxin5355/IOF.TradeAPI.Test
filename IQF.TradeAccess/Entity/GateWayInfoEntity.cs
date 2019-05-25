using IQF.Framework.Dao;
using System;

namespace IQF.TradeAccess.Entity
{
    public class GateWayInfoEntity : IEntity
    {
        /// <summary>
		/// 网关编号
		/// </summary>
		public long GatewayInfoID { get; set; }

        /// <summary>
        /// 期货公司柜台编号
        /// </summary>
        public long CompCounter { get; set; }

        /// <summary>
        /// 网关地址
        /// </summary>
        public string GatewayAddr { get; set; }

        /// <summary>
        /// 是否可用（0：不可用，1：可用）
        /// </summary>
        public int Available { get; set; }

        /// <summary>
        /// 网关地址名称
        /// </summary>
        public string Name { get; set; }

        public DateTime AddTime { get; set; }
    }
}
