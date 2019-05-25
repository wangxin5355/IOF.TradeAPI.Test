using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace IQF.Framework
{
    public static class ConfigManager
    {
        /// <summary>
        /// 配置
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 从配置文件的AppSettings里面获取配置项。如果配置项不存在或者没有值则返回defaultVal
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key, string defaultVal = null)
        {
            return GetCustomCfg("appSettings", key, defaultVal);
        }

        /// <summary>
        /// 从配置文件的AppSettings里面获取配置项。如果配置项不存在或者没有值则返回defaultVal
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static int GetAppSetting(string key, int defaultVal)
        {
            string val = GetAppSetting(key, defaultVal.ToString());
            return val.ToInt(defaultVal);
        }

        /// <summary>
        /// 获取数据库连接串
        /// </summary>
        /// <param name="dbName">数据库配置项目名称</param>
        /// <returns></returns>
        public static string GetConnectString(string dbName)
        {
            return Configuration.GetConnectionString(dbName);
        }

        /// <summary>
        /// 获取REDIS连接串
        /// </summary>
        /// <param name="redisName"></param>
        /// <returns></returns>
        public static string GetReidsConnectionString(string redisName)
        {
            return GetCustomCfg("redisConnectionStrings", redisName);
        }

        /// <summary>
        /// 获取自定义配置项
        /// </summary>
        /// <param name="sectionKey"></param>
        /// <param name="key"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static string GetCustomCfg(string sectionKey, string key, string defaultVal = null)
        {
            if (Configuration == null ||
                string.IsNullOrWhiteSpace(sectionKey) ||
                string.IsNullOrWhiteSpace(key))
            {
                return defaultVal;
            }
            var section = Configuration.GetSection(sectionKey);
            if (!section.Exists())
            {
                return defaultVal;
            }
            var child = section.GetSection(key);
            if (child.Exists())
            {
                return section[key];
            }
            return defaultVal;
        }

        public static bool SetConfig(string sectionKey, string key, string val)
        {
            if (Configuration == null ||
                string.IsNullOrWhiteSpace(sectionKey) ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            var section = Configuration.GetSection(sectionKey);
            if (!section.Exists())
            {
                return false;
            }
            var child = section.GetSection(key);
            if (child.Exists())
            {
                section[key] = val;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取自定义配置项
        /// </summary>
        /// <param name="key">
        /// 多级以分号分隔
        /// ConnectionStrings:DefaultConnection 
        /// appSettings:logpath
        /// </param>
        /// <returns></returns>
        public static string GetCustomCfg(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            return Configuration[key];
        }
    }
}
