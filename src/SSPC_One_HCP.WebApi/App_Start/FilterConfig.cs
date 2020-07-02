using System.Web;
using System.Web.Mvc;

namespace SSPC_One_HCP.WebApi
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'FilterConfig'
    public class FilterConfig
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'FilterConfig'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'FilterConfig.RegisterGlobalFilters(GlobalFilterCollection)'
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'FilterConfig.RegisterGlobalFilters(GlobalFilterCollection)'
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
