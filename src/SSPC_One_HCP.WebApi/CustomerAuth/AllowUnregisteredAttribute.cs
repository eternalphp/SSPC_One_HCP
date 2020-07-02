using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;

namespace SSPC_One_HCP.WebApi.CustomerAuth
{
    /// <summary>
    /// 允许未注册用户访问的自定义属性
    /// </summary>
    public class AllowUnregisteredAttribute : Attribute
    {
    }
}