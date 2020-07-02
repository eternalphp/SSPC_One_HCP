using Newtonsoft.Json;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Implementations
{
    public class LoginService: ILoginService
    {
        private readonly IEfRepository _rep;
        private readonly string _hostUrl = ConfigurationManager.AppSettings["HostUrl"];
        public readonly string TokenUrl = string.Empty;
        public LoginService(IEfRepository rep)
        {
            _rep = rep;
            TokenUrl = _hostUrl + "/auth/token";
        }
        /// <summary>
        /// 从邮件登录
        /// </summary>
        /// <param name="pp">信息加密字符串</param>
        /// <returns></returns>
        public ReturnValueModel MailLogin(string pp)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            
            return rvm;
        }

        /// <summary>
        /// un统一登录
        /// </summary>
        /// <param name="p">加密字符串</param>
        /// <returns></returns>
        public ReturnValueModel UnLogin(string p)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            
            return rvm;
        }
        
        /// <summary>
        /// 域账户验证并登陆
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public ReturnValueModel LoginSys(UserModel userModel)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var mainInfo = userModel.UserName.Split('\\');
            var doMain = mainInfo[0].ToUpper();
            var userName = mainInfo[1];
            var doMainPath = "";
            switch (doMain)
            {
                case "KABI":
                    doMainPath = ConfigurationManager.AppSettings["KABIPath"];
                    break;
                case "FNC":
                    doMainPath = ConfigurationManager.AppSettings["NetcarePath"];
                    break;
                case "FME":
                    doMainPath = ConfigurationManager.AppSettings["FMCPath"];
                    break;
            }


            if (string.IsNullOrEmpty(doMainPath))
            {
                rvm.Success = false;
                rvm.Msg = "登录失败";
                rvm.Result = false;
                return rvm;
            }


            LdapAuthentication ldap = new LdapAuthentication(doMainPath);
            var isLdap = ldap.IsAuthenticated(doMain, userName, userModel.Password);
            if (!isLdap)
            {
                rvm.Success = false;
                rvm.Msg = "登录失败";
                rvm.Result = false;
                return rvm;
            }
            var token = HttpUtils.PostResponse<TokenModel>(TokenUrl, $"UserName={userModel.UserName}&grant_type=password");
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                token
            };
            return rvm;
        }
        /// <summary>
        /// 修改账户密码
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public ReturnValueModel UpdatePassword(UserModel userModel) {
            ReturnValueModel rvm = new ReturnValueModel();
            //判断需要修改的用户是否存在
            var UpdateUser = _rep.FirstOrDefault<UserModel>(s=>s.Id==userModel.Id && s.IsDeleted!=1);
            //判断是否需要更改
            if (UpdateUser != null)
            {
                if (UpdateUser.Password == userModel.Password)
                {
                    if (userModel.Remark != null && userModel.Remark != "")
                    {
                        //首先判断原密码和新密码是否相同
                        if (userModel.Password.Trim().Equals(userModel.Remark.Trim()))
                        {
                            //原密码和新密码相同，不进行账号修改
                            rvm.Success = false;
                            rvm.Msg = "The original password is the same as the new password, and no modification is required.";
                        }
                        else
                        {
                            //对密码进行修改
                            UpdateUser.Password = userModel.Remark.Trim();
                            _rep.Update(UpdateUser);
                            var send = _rep.SaveChanges();
                            if (send > 0)
                            {
                                rvm.Success = true;
                                rvm.Msg = "success";
                            }
                            //密码未修改
                            else
                            {
                                rvm.Success = false;
                                rvm.Msg = "Failed to change password";
                            }
                        }

                    }
                    else
                    {
                        rvm.Success = false;
                        rvm.Msg = "New password does not exist";

                    }
                }
                else {
                    rvm.Success = false;
                    rvm.Msg = "The original password was entered incorrectly. Please confirm and try again.";
                }
               
            }
            else {
                rvm.Success = false;
                rvm.Msg = "The account to be modified does not exist";
            }
            return rvm;
        }
    }
}
