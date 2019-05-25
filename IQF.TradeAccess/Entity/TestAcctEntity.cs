using IQF.Framework.Dao;

namespace IQF.TradeAccess.Entity
{
    public partial class TestAcctEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long TestAcctID { get; set; }
        /// <summary>
        /// 期货公司柜台
        /// </summary>
        public int CompCounter { get; set; }
        /// <summary>
        /// 期货账号
        /// </summary>
        public string BrokerAccount { get; set; }
        /// <summary>
        /// 交易密码
        /// </summary>
        public string TradePwd { get; set; }
        /// <summary>
        /// 资金密码
        /// </summary>
        public string FundPwd { get; set; }
        /// <summary>
        /// 0:自动测试 1:禁用  2:无效账号
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 开户人
        /// </summary>
        public string AcctHolder { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string DepositBank { get; set; }
    } //end of class
} //end of namespace
