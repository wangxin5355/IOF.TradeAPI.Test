using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    /// <summary>
    /// 测试账号
    /// </summary>
    public interface ITestAcctDao : IProxyService
    {
        List<TestAcctEntity> GetAll();

        List<TestAcctEntity> GetAllFromDB();

        TestAcctEntity GetItemById(long nId);
    }
}
