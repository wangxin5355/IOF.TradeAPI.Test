using IQF.Framework.DynamicProxy;
using System;
using System.Collections.Generic;


namespace IQF.TradeAccess.ISession
{
    public interface IOrderChangeManager : IProxyService
    {
        void Enqueue(OrderChangeInfo info, DateTime invalidTime);

        OrderChangeInfo Dequeue();

        List<string> GetAllList();

        List<OrderChangeInfo> DequeueAll(int maxCount = 100);
    }

    public class OrderChangeInfo
    {
        public int BrokerType { get; set; }

        public string BrokerAccount { get; set; }
    }
}
