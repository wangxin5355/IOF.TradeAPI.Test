using IQF.Framework.DynamicProxy;
using System;
using System.Collections.Generic;

namespace IQF.TradeAccess.ISession
{
    public interface IRpcCounterManager : IProxyService
    {
        List<RpcCounterModel> GetAll(long compCounterID);
        bool RegRpcAddr(long compCounterID, long apiInfoID, string addr, int userCount, int processId);
        bool Remove(long compCounterID);
    }

    public class RpcCounterModel
    {
        /// <summary>
        /// 柜台编号
        /// </summary>
        public long CompCounter { get; set; }

        /// <summary>
        /// 接口信息编号，每个接口在每台服务器上会有一个进程对应
        /// </summary>
        public long ApiInfoID { get; set; }

        public string ApiAddr { get; set; }

        public int UserCount { get; set; }

        /// <summary>
        /// 进程编号
        /// </summary>
        public int ProcessId { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}