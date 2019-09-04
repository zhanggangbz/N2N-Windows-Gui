using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace CommonTools
{
    /// <summary>
    /// 封装的NLog日志
    /// </summary>
    public class LogHelper
    {
        #region 单例实现日志类
        private static volatile LogHelper instance;
        private static readonly object obj = new object();

        public static LogHelper Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (obj)
                    {
                        if (null == instance)
                        {
                            instance = new LogHelper();
                        }
                    }

                }
                return instance;
            }
        }


        private NLog.Logger loginfo;
        LogHelper()
        {
            Setup();
            loginfo = LogManager.GetLogger("swLog");
        }

        private void Setup()
        {
            LoggingConfiguration config = new LoggingConfiguration();

            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            FileTarget fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            consoleTarget.Layout = "${longdate} ${message} ${exception:format=tostring}";
            fileTarget.FileName = "${basedir}/Logs/${shortdate}/${level}.txt";
            fileTarget.Layout = "${longdate} [${threadid}] | ${message} ${onexception:${exception:format=tostring} ${newline} ${stacktrace} ${newline}";

            // Step 4. Define rules 

            LoggingRule rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            LoggingRule rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration 

            LogManager.Configuration = config;
        }
        #endregion

        /// <summary>
        /// 写入日志信息Info级别
        /// </summary>
        /// <param name="info">要写入的日志信息</param>
        public void WriteInfo(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }
        /// <summary>
        /// 写入日志信息Log级别
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="arg">参数</param>
        public void WriteInfo(string format, params object[] arg)
        {
            string str = string.Format(format, arg);
            WriteInfo(str);
        }
        /// <summary>
        /// 把异常信息写入一般日志中
        /// </summary>
        /// <param name="info">异常信息</param>
        public void WriteInfo(Exception info)
        {
            if (loginfo.IsErrorEnabled)
            {
                loginfo.Info(info);
            }
        }

        /// <summary>
        /// 写入日志信息Error级别
        /// </summary>
        /// <param name="info">日志信息</param>
        public void WriteError(string info)
        {
            if (loginfo.IsErrorEnabled)
            {
                loginfo.Error(info);
            }
        }
        /// <summary>
        /// 把错误对象输出到日志
        /// </summary>
        /// <param name="info">异常对象</param>
        public void WriteError(Exception info)
        {
            if (loginfo.IsErrorEnabled)
            {
                loginfo.Error(info);
            }
        }
        /// <summary>
        /// 写入日志Error级别
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="arg">日志信息</param>
        public void WriteError(string format, params object[] arg)
        {
            string str = string.Format(format, arg);
            WriteError(str);
        }

        /// <summary>
        /// 写入日志Wain级别
        /// </summary>
        /// <param name="info">日志信息</param>
        public void WriteWain(string info)
        {
            if (loginfo.IsWarnEnabled)
            {
                loginfo.Warn(info);
            }
        }
        /// <summary>
        /// 把异常信息写入警告
        /// </summary>
        /// <param name="info">异常信息</param>
        public void WriteWain(Exception info)
        {
            if (loginfo.IsErrorEnabled)
            {
                loginfo.Warn(info);
            }
        }
        /// <summary>
        /// 写入日志Wain级别
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="arg">日志信息</param>
        public void WriteWain(string format, params object[] arg)
        {
            string str = string.Format(format, arg);
            WriteWain(str);
        }
    }
}
