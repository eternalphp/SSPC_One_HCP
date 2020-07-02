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
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;

namespace SSPC_One_HCP.Services.Implementations
{
  public class WxExtensionService:IWxExtensionService
    {
        private readonly IEfRepository _rep;

        public WxExtensionService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 获取公众号推广信息
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel WxGetPublicAccount(RowNumModel<PublicAccount> publicaccount,WorkUser workUser) {
            ReturnValueModel rvm = new ReturnValueModel();
            //首先查询用户
            var SendUser = _rep.SqlQuery<DoctorModel>("select * from DoctorModel where id='" + workUser.WxUser.Id+"' or UnionId='"+workUser.WxUser.UnionId+"'and IsDeleted=0").FirstOrDefault();
            if (SendUser == null)
            {
                rvm.Success = false;
                rvm.Msg = "The user does not exist";
            }
            else {
                //根据该用户科室查询公众号推广
                var SendPublic = _rep.SqlQuery<PublicAccount>("select * from PublicAccount where Dept like  '%"+workUser.WxUser.DepartmentName+ "%' and Iseffective=0 and IsDeleted=0").OrderByDescending(c=>c.CreateTime);
                if (SendPublic == null)
                {
                    rvm.Success = true;
                    rvm.Msg = "No Data";
                    rvm.Result = null;
                }
                else {
                    rvm.Success = true;
                    rvm.Msg = "success";
                    var total = SendPublic.Count();
                    var rows = SendPublic.OrderByDescending(s => s.CreateTime) .ToPaginationList(publicaccount.PageIndex,publicaccount.PageSize).ToList();
                    rvm.Result = new
                    {
                        total = total,
                        rows = rows
                    };
                }
            }
            return rvm;
        }
        ///// <summary>
        ///// 统计访问次数
        ///// </summary>
        ///// <param name="qRcodeRecord"></param>
        ///// <param name="workUser"></param>
        ///// <returns></returns>
        //public ReturnValueModel WxGetRecordCount(QRcodeRecord qRcodeRecord,WorkUser workUser) {
        //    ReturnValueModel rvm = new ReturnValueModel();
        //    //统计访问次数
        //    int reCordCount = _rep.SqlQuery<QRcodeRecord>("select * from QRcodeRecord where AppId=201906").Count();
        //    //统计注册次数
        //    int registeredCount = _rep.SqlQuery<QRcodeRecord>("select * from QRcodeRecord Where Isregistered=0 and AppId=201906").Count() ;
        //    if (reCordCount <= 0)
        //    {
        //        rvm.Success = false;
        //        rvm.Msg = "No access record";
                
        //    }
        //    else {
        //        var totalRecordCount = reCordCount;
        //        if (registeredCount <= 0)
        //        {
        //            rvm.Success = false;
        //            rvm.Msg = "No access record";
        //        }
        //        else {
        //            rvm.Success = true;
        //            rvm.Msg = "success";
        //            rvm.Result = new
        //            {
        //                totalRecordCount = totalRecordCount,
        //                totalRegisteredCount = registeredCount
        //            };
        //        }
        //    }

        //    return rvm;
        //}
    }
}
