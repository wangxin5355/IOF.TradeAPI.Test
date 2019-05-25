using IQF.Framework.Dao;
using System;

namespace IQF.TradeAccess.Entity
{
    public class LoginLogEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BrokerAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BrokerType { get; set; }
        /// <summary>
        /// 期货公司柜台编号
        /// </summary>
        public int CompCounter { get; set; }
        /// <summary>
        /// 客户类型（0：外部用户，1：内部用户）
        /// </summary>
        public int AccountType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PackType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public LoginStatus Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ErrorNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime { get; set; }
    } //end of class

    public enum LoginStatus
    {
        Login = 0,
        Logout = 1
    }

} //end of namespace
