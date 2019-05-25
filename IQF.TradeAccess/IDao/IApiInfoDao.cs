using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    public interface IApiInfoDao : IProxyService
    {
        List<ApiInfoEntity> GetAll();

        List<ApiInfoEntity> GetAllFromDB();

        ApiInfoEntity GetItemById(long nId);
    }
}
