using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SSPC_One_HCP.WebApi
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'RouteConfig'
    public class RouteConfig
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'RouteConfig'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'RouteConfig.RegisterRoutes(RouteCollection)'
        public static void RegisterRoutes(RouteCollection routes)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'RouteConfig.RegisterRoutes(RouteCollection)'
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "web/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
