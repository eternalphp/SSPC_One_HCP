using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Implementations
{
    public class DiscoveryService: IDiscoveryService
    {
        private readonly IEfRepository _rep;

        public DiscoveryService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 根据当前用户获取到相关的会议和学术知识
        /// </summary>
        /// 
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetDiscoveryHome(WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var model = new DiscoveryViewModel();

            var user = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == workUser.WxUser.UnionId);
            if (user.IsCompleteRegister == 1)
            {
                model.IsRegister = 1;
                model.MeetInfos = _rep.Where<MeetInfo>(s => s.MeetDep.Contains(user.DepartmentName)).ToList().OrderByDescending(s => s.CreateTime).Take(2);
                model.Academic = _rep.Where<ProductTypeInfo>(s => s.ContentDepType.Contains(user.DepartmentName) && s.TypeId == 2).ToList().OrderByDescending(s => s.CreateTime).Take(2);

                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    model = model
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }

            return rvm;
        }
    }
}
