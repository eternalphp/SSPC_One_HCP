using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.Services.Implementations
{
    public class SmsService : ISmsService
    {
        private readonly IEfRepository _rep;

        public SmsService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="wxUserModel">传入手机号码</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> SendSms(WxUserModel wxUserModel)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            //var docModel = await _rep.FirstOrDefaultAsync<WxUserModel>(s =>
            //    s.UnionId == wxUserModel.UnionId || s.OpenId == wxUserModel.OpenId);
            //if (docModel != null)
            //{
            //    var code = RandomUtil.GenerateRandomCode(6);
            //    SendSmsModel sm = new SendSmsModel
            //    {
            //        CompanyCode = "4033",
            //        ParamName = JsonConvert.SerializeObject(new
            //        {
            //            code= code
            //        }).Base64Encoding(),
            //        PhoneNumbers = wxUserModel.Mobile,
            //        SignName = "",
            //        SystemId = "",
            //        TemplateId = ""
            //    };
            //    docModel.Mobile = wxUserModel.Mobile;
            //    docModel.Code = code;
            //    docModel.CodeTime = DateTime.Now;
               
            //    var smsResult = SmsUtil.SendMessage(sm);
            //    rvm.Msg = smsResult.Message;
            //    rvm.Success = smsResult.ResultFlag;
            //    rvm.Result = new
            //    {
            //        smsResult
            //    };
            //    _rep.Update(docModel);
            //    _rep.SaveChanges();
            //}
            return rvm;
        }
        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="wxUserModel">传入Code、Mobile</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public async Task<ReturnValueModel> VerifySmsCode(WxUserModel wxUserModel)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            //var wxUser = await _rep.FirstOrDefaultAsync<WxUserModel>(s => s.UnionId == wxUserModel.UnionId);

            //var timeLimit = (DateTime.Now - wxUser.CodeTime)?.Minutes;
            ////五分钟超时
            //if (timeLimit > 5)
            //{
            //    rvm.Success = false;
            //    rvm.Msg = "验证码过期，请重新发送验证码！";
            //    rvm.Result = timeLimit;
            //}
            //else
            //{
            //    if (wxUser.Code == wxUserModel.Code && wxUser.Mobile == wxUserModel.Mobile)
            //    {
            //        rvm.Success = true;
            //        rvm.Msg = "验证成功！";
            //        rvm.Result = wxUser.Code;
            //    }
            //    else
            //    {
            //        rvm.Success = false;
            //        rvm.Msg = "验证码错误，请重新输入！";
            //        rvm.Result = wxUserModel.Code;
            //    }
                
            //}

            return rvm;
        }
    }
}
