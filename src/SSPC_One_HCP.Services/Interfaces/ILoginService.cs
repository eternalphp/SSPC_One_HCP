using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface ILoginService
    {
        /// <summary>
        /// 从邮件登录
        /// </summary>
        /// <param name="pp">信息加密字符串</param>
        /// <returns></returns>
        ReturnValueModel MailLogin(string pp);

        /// <summary>
        /// un统一登录
        /// </summary>
        /// <param name="p">加密字符串</param>
        /// <returns></returns>
        ReturnValueModel UnLogin(string p);

        /// <summary>
        /// 域账户验证并登陆
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        ReturnValueModel LoginSys(UserModel userModel);
        /// <summary>
        /// 修改账户密码
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        ReturnValueModel UpdatePassword(UserModel userModel);
    }
}
