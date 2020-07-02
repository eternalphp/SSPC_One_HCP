using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.KBS;
using SSPC_One_HCP.KBS.Webs.Clients;
using SSPC_One_HCP.Services.Services.WeChat.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WeChat.Implementations
{
    /// <summary>
    /// 销售 AD登录
    /// </summary>
    public class WcSaleAdService : IWcSaleAdService
    {
        private readonly IEfRepository _rep;
        public WcSaleAdService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 多福-销售注册
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> Login(LoginInputDto dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel
            {
                Success = true,
                Msg = "",
                Result = null
            };

            if (string.IsNullOrEmpty(workUser?.WxUser?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "Invalid WxUser";
                rvm.Result = null;
                return rvm;
            }
            //匹配AD白名单
            var whiteName = _rep.FirstOrDefault<BotADWhiteName>(s => s.IsDeleted != 1 && s.ADAccount == dto.UserName);
            if (whiteName == null)
            {
                rvm.Success = false;
                rvm.Msg = "您输入的账号无权限访问或账号密码不正确。";
                rvm.Result = null;
                LoggerHelper.WriteLogInfo("[CheckWhiteName]:错误------不在白名单");
                return rvm;
            }
            var verify = await GetVerifyApi(dto.UserName, dto?.Password);
            if (verify.Success == false) return verify;

            var user = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.Id == workUser.WxUser.Id);
            if (user == null)
            {
                rvm.Success = false;
                rvm.Msg = "获取用户信息失败！";
                rvm.Result = null;
                return rvm;
            }
            user.UserName = dto.UserName;
            user.IsSalesPerson = 2;//设置为内部员工
            user.IsVerify = 1;
            user.IsCompleteRegister = 1;
            _rep.Update(user);

            _rep.Insert(new DocTag
            {
                Id = Guid.NewGuid().ToString(),
                CreateTime = DateTime.UtcNow.AddHours(8),
                TagId = "3a8670c8-29e9-482a-bb54-53766b6babdb",//此处  固定内部员工标签 TagInfo id=9481d4a8-a259-474b-b912-2a61c9069a1d
            });
            _rep.SaveChanges();

            return rvm;
        }
        async Task<ReturnValueModel> GetVerifyApi(string adaccount, string password)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                byte[] b = System.Text.Encoding.UTF8.GetBytes(password);
                //转成 Base64 形式的 System.String  

                var pwd = Convert.ToBase64String(b);
                string _host = ConfigurationManager.AppSettings["ADVerifyUrl"];
                var result = await new WebClient<AdResult>()
                           .Post(_host)
                           .JsonData(new { adaccount = adaccount, password = pwd })
                           .ResultFromJsonAsync();
                //成功
                if (result?.Error_code == 0)
                {
                    rvm.Msg = "success";
                    rvm.Success = true;
                    rvm.Result = "成功";
                    return rvm;
                }
                if (result?.Error_code == -1)
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;

                }
                if (result?.Error_code == 10001001)
                {
                    rvm.Success = false;
                    rvm.Msg = "数据库操作失败";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
                if (result?.Error_code == 10031001)
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
                if (result?.Error_code == 10031004)
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
                if (result?.Error_code == 10031011)
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
                if (result?.Error_code == 10031012)
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
                else
                {
                    rvm.Success = false;
                    rvm.Msg = "fail";
                    rvm.Result = "您输入的账号无权限访问或账号密码不正确。";
                    return rvm;
                }
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = "系统忙，请稍后再试！";
            }
            return rvm;
        }

    }
}
