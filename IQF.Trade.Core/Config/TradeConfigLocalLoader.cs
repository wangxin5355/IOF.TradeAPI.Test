using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace IQF.Trade.Core.Config
{
    public class TradeConfigLocalLoader : ITradeConfigLoader
    {
        public TConfig GetConfig<TConfig>(long apiInfoId) 
            where TConfig : TradeConfig
        {
            //期货公司安装模式，直接加载执行目录下的TradeConfig
            var currntDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(currntDir, "TradeConfig","tradeCfg.json");
            if (!File.Exists(path))
            {
                return default(TConfig);
            }
            var txt = File.ReadAllText(path, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(txt))
            {
                return default(TConfig);
            }
            var config = JsonConvert.DeserializeObject<TConfig>(txt);
            return config;
        }
    }
}
