using IQF.Framework.DynamicProxy;
using IQF.TradeAccess.Entity;
using System.Collections.Generic;

namespace IQF.TradeAccess.IDao
{
    public interface IFuturesCompany : IProxyService
    {
        IFuturesCompany GetByFcCode(string fcCode);

        IFuturesCompany GetByFcName(string fcName);

        List<IFuturesCompany> GetCompanyList();

        List<IFuturesCompany> GetShowCompanyList();

        List<VM_FutureCompanyPackType> GetShowPackageCompanyList(string paramPackType);

    }
}
