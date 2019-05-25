using System;
using System.Collections.Generic;
using System.Text;

namespace IQF.Trade.Core.Config
{
    public interface ITradeConfigLoader
    {
         TConfig GetConfig<TConfig>(long ApiInfoId) where TConfig : TradeConfig;
    }
}
