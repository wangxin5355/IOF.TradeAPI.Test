using System;
using System.Collections.Concurrent;
using System.IO;

namespace IQF.Framework
{
    /// <summary>
    /// 日志记录类。
    /// </summary>
    public class LogRecord
    {
        private readonly static object logLock = new object();

        private static string logpath = string.Empty;

        private static ConcurrentDictionary<string, object> fileLockDic = new ConcurrentDictionary<string, object>();

        static LogRecord()
        {
            var path = ConfigManager.GetAppSetting("logpath", Directory.GetCurrentDirectory());

            SetLogPath(path);
        }

        public static void SetLogPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            logpath = path;
        }

        public static void writeLogsingle(string filename, string format, params object[] arg)
        {
            writeLogsingle(filename, string.Format(format, arg));
        }

        public static void writeLogsingle(string filename, string logMessage)
        {
            object lockObject = fileLockDic.GetOrAdd(filename, new object());

            lock (lockObject)
            {
                logMessage = string.Format("{0} {1}\r\n", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), logMessage);
                string fLogName = Path.Combine(logpath, System.DateTime.Now.ToString("yyyyMMdd_HH_") + filename);//按照“linux应用部署规范”进行文件命名
                try
                {
                    if (!fLogName.EndsWith(".log"))
                    {
                        fLogName += ".log";
                    }
                    using (FileStream fs = new FileStream(fLogName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (BinaryWriter w = new BinaryWriter(fs))
                        {
                            w.Write(logMessage.ToCharArray());
                        }
                    }
                }
                catch (Exception exp)
                {
                    string s = exp.Message;
                }
            }
        }
    }
}
