using Castle.DynamicProxy;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSPC_One_HCP.AOP.AopInterceptor
{
    public class RepositoryInterceptor : IInterceptor
    {
        private readonly ILog _timelogger = LogManager.GetLogger("ExcuteTimeLogFileLogger");
        private readonly ILog _errLogger = LogManager.GetLogger("ErrorFileLogger");
        private readonly Stopwatch _stopwatch = new Stopwatch();
        public void Intercept(IInvocation invocation)
        {
            var cache = ContainerManager.Resolve<ICacheManager>();
            //var workUser = cache.Get<WorkUser>("CurrentUser");
            //var ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            try
            {
                _stopwatch.Reset();
                _stopwatch.Restart();
                invocation.Proceed();
                _stopwatch.Stop();
                var arguments = invocation.Arguments.Any() ? invocation.Arguments.Aggregate((s, a) => s + "," + a) : "";
                _timelogger.Info("******************************************************************");
                _timelogger.Info($" MethodName：[[{invocation.MethodInvocationTarget.DeclaringType.FullName + "." + invocation.Method.Name }]] ");
                _timelogger.Info($" Arguments：[[{arguments}]]");
                _timelogger.Info($" Take[[{_stopwatch.ElapsedMilliseconds}]]Milliseconds");
                _timelogger.Info("******************************************************************");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var msg = string.Empty;
                var errors = (from u in ex.EntityValidationErrors select u.ValidationErrors).ToList();
                foreach (var item in errors)
                    msg += item.FirstOrDefault().ErrorMessage;

                _errLogger.Error($"--------------------------------------------------------------------------------");
                _errLogger.Error($"[MSG]:{ex.Message};\n");
                _errLogger.Error($"[Source]:{ex.Source}\n");
                _errLogger.Error($"[StackTrace]:{ex.StackTrace}\n");
                _errLogger.Error($"[StackTrace]:{ex.TargetSite.Name}\n");
                _errLogger.Error($"[Details]:{errors}\n");
                _errLogger.Error($"--------------------------------------------------------------------------------");

                throw new Exception(msg, ex);
            }
            catch (Exception e)
            {
                var methodName = (invocation?.MethodInvocationTarget?.DeclaringType?.FullName ?? "") + "." + (invocation?.Method?.Name ?? "");
                var arguments = (invocation?.Arguments?.Any() ?? false) ? invocation.Arguments.Aggregate((s, a) => s + "," + a) : "";

                _errLogger.Error($"--------------------------------------------------------------------------------");
                _errLogger.Error($"[MSG]:{e.Message};\n");
                _errLogger.Error($"[Source]:{e.Source}\n");
                _errLogger.Error($"[StackTrace]:{e.StackTrace}\n");
                _errLogger.Error($"[StackTrace]:{e.TargetSite.Name}\n");
                _errLogger.Error($"[MethodName]：[[{methodName}]]\n");
                _errLogger.Error($"[Arguments]：[[{arguments}]]");
                _errLogger.Error($"--------------------------------------------------------------------------------");
            }
        }
    }
}
