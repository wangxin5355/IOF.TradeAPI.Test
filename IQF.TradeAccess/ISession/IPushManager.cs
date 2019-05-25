using IQF.Framework.DynamicProxy;
using System;

namespace IQF.TradeAccess.ISession
{
    /// <summary>
    /// 推送Redis管理
    /// </summary>
    public interface IPushManager : IProxyService
    {
        bool Save(string pushKey, DateTime invalidTime);
    }
}
