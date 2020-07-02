using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Data.Data;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.WebApi.CustomerAuth;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 微信基础控制器，用于扩展
    /// </summary>
    [WxAuth]
    public class WxBaseApiController : ApiController
    {
        /// <summary>
        /// 当前操作用户
        /// </summary>
        public WorkUser WorkUser
        {
            get
            {
                var userid = User.Identity.Name;
                if (string.IsNullOrEmpty(userid)) return null;

                var wxRegister = ContainerManager.Resolve<IWxRegisterService>();
                return wxRegister.GetWorkUser(userid);
            }
        }
    }
}
