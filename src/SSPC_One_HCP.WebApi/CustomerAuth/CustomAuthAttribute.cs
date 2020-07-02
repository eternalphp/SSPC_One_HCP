using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SSPC_One_HCP.WebApi.CustomerAuth
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CustomAuthAttribute'
    public class CustomAuthAttribute : AuthorizeAttribute
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CustomAuthAttribute'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CustomAuthAttribute.OnAuthorization(HttpActionContext)'
        public override void OnAuthorization(HttpActionContext actionContext)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CustomAuthAttribute.OnAuthorization(HttpActionContext)'
        {
            var sapCode = actionContext.RequestContext.Principal.Identity.Name;
            if (string.IsNullOrEmpty(sapCode))
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                var cache = ContainerManager.Resolve<ICacheManager>();
                var workUser = cache.Get<WorkUser>(sapCode);
                if (workUser == null)
                {
                    this.HandleUnauthorizedRequest(actionContext);
                }
                else
                {
                    if (workUser.User == null)
                    {
                        this.HandleUnauthorizedRequest(actionContext);
                    }
                    else
                    {
                        base.OnAuthorization(actionContext);
                    }
                }
            }

        }
    }
}