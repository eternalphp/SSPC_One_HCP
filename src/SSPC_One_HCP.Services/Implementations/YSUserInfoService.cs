using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Implementations
{
   public class YSUserInfoService: IYSUserInfoService
    {
        
        private readonly IEfRepository _rep;

        public YSUserInfoService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="unionid"></param>
        /// <returns></returns>

        public ReturnValueModel GetUserInfo(string unionid)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            rvm.Success = true;
            var item = _rep.FirstOrDefault<WxUserModel>(x => x.IsDeleted != 1 && x.UnionId == unionid);

            if (item!=null)
            {
              
                rvm.Result = new { name = item.UserName, image = item.WxPicture };
            }
            else
            {
                rvm.Result =null;
            }
            
            return rvm;
       
        }

        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="unionid"></param>
        /// <returns></returns>
        public ReturnValueModel DelUserInfo(string unionid)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            rvm.Success = true;
            var item = _rep.FirstOrDefault<WxUserModel>(x => x.IsDeleted != 1 && x.UnionId == unionid);
            if (item != null)
            {
                item.IsDeleted = 1;
                _rep.SaveChanges();
            }
            else
            {
                rvm.Result = null;
            }

            return rvm;
        }
    }
}
