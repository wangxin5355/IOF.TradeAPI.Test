using IQF.Framework;
using IQF.Framework.Util;
using IQF.Trade.Core.Config;
using IQF.Trade.Core.Session;
using IQF.TradeAccess.ISession;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace IQF.Trade.Core
{
    public class TradeContext : ITradeContext
    {
        private readonly ISessionManager sessionManager;
        private readonly IRpcCounterManager rpcCounterManager;
        private readonly IConfiguration configuration;
        private readonly ITradeConfigLoader _tradeConfigLoader;

        public static IServerAddressesFeature ServerAddresses { get; set; }

        private TradeConfig tradeConfig;

        public TradeContext(ISessionManager sessionManager,
            IRpcCounterManager rpcCounterManager,
            IConfiguration configuration,
            ITradeConfigLoader tradeConfigLoader)
        {
            this.sessionManager = sessionManager;
            this.rpcCounterManager = rpcCounterManager;
            this.configuration = configuration;
            _tradeConfigLoader = tradeConfigLoader;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="configDir">配置文件路径</param>
        /// <param name="serverIp"></param>
        /// <param name="port"></param>
        public void Init()
        {

            var address = ServerAddresses.Addresses.Single();
            var match = Regex.Match(address, @"^.+:(\d+)$");
            if (!match.Success)
            {
                LogFatal($"Can not parsing server port,address{address}");
                throw new Exception("Can not parsing server port");
            }
            int port = Int32.Parse(match.Groups[1].Value);
            LogInfo($"service listhen on port:{port}");
            var serverIp = this.configuration["server"];
            long apiInfoId = this.configuration["api"].ToLong();
            if (string.IsNullOrWhiteSpace(serverIp) || port <= 0|| apiInfoId<0)
            {
                throw new ApplicationException(string.Format("参数异常，ApiInfoId:{0}，ServerIp:{1}，Port:{2}", apiInfoId, serverIp, port));
            }
            tradeConfig = _tradeConfigLoader.GetConfig<TradeConfig>(apiInfoId);
            if (tradeConfig == null)
            {
                throw new ApplicationException($"加载配置文件失败！ ApiInfoId:{apiInfoId}");
            }

            var dirName = tradeConfig.ApiInfoId + "#" + Process.GetCurrentProcess().Id;
            var logPath = Path.Combine(ConfigManager.GetAppSetting("logpath"), tradeConfig.BrokerType, dirName + "\\");
            LogRecord.SetLogPath(logPath);
            LogInfo($"开始启动，BrokerType:{tradeConfig.BrokerType}，ApiInfoId:{tradeConfig.ApiInfoId}，CompCounter:{tradeConfig.CompCounter}，ServerIp:{1}，Port:{2}，status:{tradeConfig.ServiceStatus}");

            //延迟注册，防止监听未准备好就注册成功
            var thReg = new Thread(RegApiAddr);
            thReg.Name = "RegApiAddr";
            thReg.Start($"http://{serverIp}:{port}/");

            LogInfo($"启动完成，BrokerType:{tradeConfig.BrokerType}，ApiInfoId:{tradeConfig.ApiInfoId}，CompCounter:{tradeConfig.CompCounter}，ServerIp:{1}，Port:{2}，status:{tradeConfig.ServiceStatus}");
        }

        private void RegApiAddr(object state)
        {
            var url = state as string;
            if (string.IsNullOrWhiteSpace(url))
            {
                LogFatal($"监听地址不能为空，BrokerType:{tradeConfig.BrokerType}，ApiInfoId:{tradeConfig.ApiInfoId}，CompCounter:{tradeConfig.CompCounter}，ServerIp:{1}，Port:{2}，status:{tradeConfig.ServiceStatus}");
                return;
            }
            var process = Process.GetCurrentProcess();
            while (true)
            {
                try
                {
                    var ret = this.rpcCounterManager.RegRpcAddr(this.tradeConfig.CompCounter, this.tradeConfig.ApiInfoId, url, this.sessionManager.GetCount(), process.Id);
                    var msg = $"期货柜台：{tradeConfig.CompCounter}注册API地址：{url}，执行结果：{ret}，状态：{tradeConfig.ServiceStatus}";
                    if (ret && RandomUtil.RandomRate(10))
                    {
                        LogInfo(msg);
                    }
                    else if (!ret)
                    {
                        LogFatal(msg);
                    }
                }
                catch (Exception ex)
                {
                    LogFatal(ex.ToString());
                }
                Thread.Sleep(1000 * 5);
            }
        }

        void LogInfo(string msg)
        {
            LogRecord.writeLogsingle("TradeContext.log", msg);
        }

        void LogFatal(string error)
        {
            LogRecord.writeLogsingle("TradeContextFatal.log", error);
        }
    }
}
