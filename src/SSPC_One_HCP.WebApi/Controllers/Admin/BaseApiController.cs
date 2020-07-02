using log4net;
using Newtonsoft.Json;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.WebApi.CustomerAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    [CustomAuth]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BaseApiController'
    public class BaseApiController : ApiController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BaseApiController'
    {
        private readonly ILog _logger = LogManager.GetLogger("WarnFileLogger");
        /// <summary>
        /// 当前操作用户
        /// </summary>
        public WorkUser WorkUser
        {
            get
            {
                var sapCode = User.Identity.Name;
                var cache = ContainerManager.Resolve<ICacheManager>();
                var rep = ContainerManager.Resolve<IEfRepository>();
                var workUser = cache.Get<WorkUser>(sapCode);
               
                if (workUser == null)
                {
                    _logger.Warn($"20190612-cache WorkUser{sapCode}:{JsonConvert.SerializeObject(workUser ?? new WorkUser() { })}");
                    workUser = new WorkUser();
                    var userCode = "";
                    if (!string.IsNullOrEmpty(sapCode))
                    {
                        userCode = sapCode;
                    }
                    var user = rep.Where<UserModel>(s => s.Code == userCode).FirstOrDefault();
                    workUser.User = user;
                    workUser.Roles = (from a in rep.Table<RoleInfo>()
                                      join b in rep.Table<UserRole>() on a.Id equals b.RoleId
                                      where b.SapCode == user.Code && a.IsDeleted != 1 && b.IsDeleted != 1
                                      select a).ToList();

                    cache.Set(sapCode, workUser, 12);
                }

                if (workUser != null && workUser.Organization == null)
                {
                    var organizationId = workUser.User?.OrganizationId;
                    if (!string.IsNullOrEmpty(organizationId))
                    {
                        workUser.Organization = (from a in rep.Table<Organization>()
                                                 where a.Id == organizationId && a.IsDeleted != 1
                                                 select a).FirstOrDefault();
                        cache.Set(sapCode, workUser, 12);
                    }
                }

                return workUser;
            }
        }

     
    }
}
