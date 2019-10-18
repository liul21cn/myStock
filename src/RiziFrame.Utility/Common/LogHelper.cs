using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace RiziFrame.Utility.Common
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class LogHelper
    {
       /// <summary>
        /// 日志实例
        /// </summary>        
        private static ILog logInstance=LogManager.GetLogger("");


        public static void WriteLog(string message ,LogLevel level) {
            switch (level) {
                case LogLevel.Debug:
                    logInstance.Debug(message);
                    break;
                case LogLevel.Error:
                    logInstance.Error(message);
                    break;
                case LogLevel.Fatal:
                    logInstance.Fatal(message);
                    break;
                case LogLevel.Info:
                    logInstance.Info(message);
                    break;
                case LogLevel.Warn:
                    logInstance.Warn(message);
                    break;
                default:
                    logInstance.Info(message);
                    break;
            }
        }
    }

    public enum LogLevel {
        Debug=0,
        Error=1,
        Fatal=2,
        Info=3,
        Warn=4
    }
}
