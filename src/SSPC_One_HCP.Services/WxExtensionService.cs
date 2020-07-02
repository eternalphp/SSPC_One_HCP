using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using SSPC_One_HCP.Services.Utils;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
namespace SSPC_One_HCP.Services
{
   public class WxExtensionService:IWxExtensionService
    {
        private readonly IEfRepository _rep;
        private readonly ICommonService _commonService;
        private readonly ISystemService _systemService;

        public WxExtensionService(IEfRepository rep, ICommonService commonService, ISystemService systemService)
        {
            _rep = rep;
            _commonService = commonService;
            _systemService = systemService;
        }
        /// <summary>
        /// 根据用户科室查询公众号推广信息
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel WxGetPublicAccount(RowNumModel<PublicAccount> publicaccount, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            //首先根据用户查询用户科室
            var SendUser = _rep.SqlQuery<PublicAccount>("select * from DoctorModel where Id='" + workUser.WxUser.Id + "' or UnionId='" + workUser.WxUser.UnionId + "' && IsDeleted=0").FirstOrDefault();
            if (SendUser != null)
            {
                rvm.Success = false;
                rvm.Msg = "The user does not exist";
            }
            else
            {
                //根据用户科室查询公众号推广信息
                var wxSend = _rep.SqlQuery<PublicAccount>("select * from PublicAccount where dept in('" + workUser.WxUser.DepartmentName + "') && IsDeleted=0");
                var total = wxSend.Count();
                var rows = wxSend.OrderByDescending(o =>o.CreateTime)
                    .ToPaginationList(publicaccount.PageIndex, publicaccount.PageSize);
                rvm.Result = new
                {
                    total = total,
                    rows = rows
                };
            }
            return rvm;
        }
    }
}
