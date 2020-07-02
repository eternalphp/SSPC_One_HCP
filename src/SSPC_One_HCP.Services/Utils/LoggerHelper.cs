using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Utils
{
    public static class LoggerHelper
    {
        private static readonly ILog _logger = LogManager.GetLogger("DebugFileLogger");
        private static readonly ILog _errLogger = LogManager.GetLogger("ErrorFileLogger");
        private static readonly ILog _infoLogger = LogManager.GetLogger("InfoFileLogger");
        
        private static readonly string IsWrite = ConfigurationManager.AppSettings["IsWriteLog"];
        private static readonly string IsWriteTest = ConfigurationManager.AppSettings["IsWriteTestLog"];
        public static void WriteLogInfo(string Content)
        {
            if (IsWrite == "1")
            {
                _logger.Warn(Content);
            } 
        }
        public static void Error(string Content)
        {
            if (IsWrite == "1")
            {
                _errLogger.Error(Content);
            }
        }

        public static void WarnInTimeTest(string Content)
        {
            if (IsWriteTest == "1")
            {
                _infoLogger.Warn(Content);
            }
        }
    }
}
