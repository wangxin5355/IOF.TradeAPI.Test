using IQF.TradeAccess.Entity;

namespace IQF.TradeAccess.View
{
    public class AccountBindingView : AccountBindingEntity
    {
        /// <summary>
        /// 期货公司柜台编号
        /// </summary>
        public long CompCounter { get; set; }

        public int BrokerType { get; set; }

        public string BrokerAccount { get; set; }
    }
}
