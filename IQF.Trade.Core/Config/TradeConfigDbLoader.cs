
using IQF.TradeAccess.Dao;
using IQF.TradeAccess.IDao;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace IQF.Trade.Core.Config
{
    public class TradeConfigDbLoader : ITradeConfigLoader
    {
        private readonly IApiInfoDao _apiInfoDao;
        private readonly IBrokerCompanyDao _brokerCompanyDao;
        private readonly ICompCounterDao _compCounterDao;
        private readonly ILogger _logger;

        public TradeConfigDbLoader(IApiInfoDao apiInfoDao, IBrokerCompanyDao brokerCompanyDao,ICompCounterDao compCounterDao, ILogger<TradeConfigDbLoader> logger)
        {
            _apiInfoDao = apiInfoDao;
            _brokerCompanyDao = brokerCompanyDao;
            _compCounterDao = compCounterDao;
            _logger = logger;
        }
        public TConfig GetConfig<TConfig>(long apiInfoId) where TConfig : TradeConfig
        {
            _logger.LogError("配置文件不能为空:" + apiInfoId);
            var configDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            DirectoryInfo path_exe = new DirectoryInfo(configDir); //exe目录
            string path= path_exe.FullName;//上一级目录
            path = Path.Combine(path, "TradeConfig");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //获取公司信息
            var apiInfo= _apiInfoDao.GetItemById(apiInfoId);
            if (apiInfo == null)
                throw new Exception($"Illegal ApiInfoId:{apiInfoId}");
            var compCounter = _compCounterDao.Get(apiInfo.CompCounter);
            if(compCounter==null)
                throw new Exception($"ApiInfoId:{apiInfoId} has no  CompCounter");
            var company = _brokerCompanyDao.GetByID(compCounter.BrokerCompany);
            if (company == null)
                throw new Exception($"ApiInfoId:{apiInfoId} has no  Company");
            var companyName= company.BrokerType.ToString();
            string dir= Path.Combine(path, companyName, apiInfoId.ToString());
            string tradeConfigPath = Path.Combine(path, companyName, apiInfoId.ToString());
            string configPath = SaveConfig(apiInfoId, apiInfo.CompCounter, company.BrokerType.ToString(), apiInfo.CounterConfig, apiInfo.ConfigExt, tradeConfigPath, apiInfo.UpdateTime);
            if (configPath==null)
            {
               throw new Exception($"ApiInfoId:{apiInfoId} can not get config file from db");
            }
            //加载
            var txt = File.ReadAllText(configPath, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(txt))
            {
                return default(TConfig);
            }
            var config = JsonConvert.DeserializeObject<TConfig>(txt);
            return config;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="apiInfoId"></param>
        /// <param name="compCounter"></param>
        /// <param name="brokerType"></param>
        /// <param name="config"></param>
        /// <param name="configExt"></param>
        /// <param name="dir">配置文件文件夹</param>
        /// <param name="lastUpdateTime"></param>
        /// <returns></returns>
        private string SaveConfig(long apiInfoId, long compCounter, string brokerType, string config, string configExt,string tradConfigPath, DateTime lastUpdateTime)
        {
            if (string.IsNullOrWhiteSpace(config))
            {
                _logger.LogError("配置文件不能为空:" + apiInfoId);
                return null;
            }
            if (!Directory.Exists(tradConfigPath))
            {
                Directory.CreateDirectory(tradConfigPath);
            }
            var configPath = Path.Combine(tradConfigPath, "tradeCfg.json");
            if (File.Exists(configPath))
            {
                var lastWriteTime = File.GetLastWriteTime(configPath);
                if (lastUpdateTime < lastWriteTime)
                {
                    _logger.LogInformation($"跳过更新配置文件:{apiInfoId}，本地文件最后更新时间:{lastWriteTime},服务器最后更新时间:{lastUpdateTime}");
                    return configPath;
                }
            }
            var jsonObject = JObject.Parse(config);
            jsonObject.Add("apiInfoId", apiInfoId);
            jsonObject.Add("compCounter", compCounter);
            jsonObject.Add("brokerType", brokerType);
            File.WriteAllText(configPath, jsonObject.ToString(), Encoding.UTF8);

            if (string.IsNullOrWhiteSpace(configExt))
            {
                return configPath;
            }
            var dict = JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(configExt);
            foreach (var key in dict.Keys)
            {
                var path = Path.Combine(tradConfigPath, key);
                File.WriteAllBytes(path, dict[key]);
            }
            return configPath;
        }
    }
}
