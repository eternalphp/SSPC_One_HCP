using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels.UserModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using SSPC_One_HCP.WebApi.CustomerAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 微信医生注册流程
    /// </summary>
    public class WxRegisterController : WxBaseApiController
    {
        private readonly IWxRegisterService _wxRegisterService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wxRegisterService"></param>
        public WxRegisterController(IWxRegisterService wxRegisterService)

        {
            _wxRegisterService = wxRegisterService;
        }

        ///// <summary>
        ///// 创建微信用户--医生
        ///// </summary>
        ///// <returns></returns>
        //public IHttpActionResult AddWxUser(WxUserModel wxUserModel)
        //{
        //    //用户授权之后获取微信的相关用户信息，插入到表中
        //    var ret = _wxRegisterService.AddDocUser(wxUserModel);
        //    return Ok(ret);
        //}

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult SendCode(WxUserModel wxUserModel)
        {
            string mobile = wxUserModel?.Mobile;
            var ret = _wxRegisterService.SendVerifyCode(WorkUser, mobile);
            return Ok(ret);
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult VerifyCode(WxUserModel wxUserModel)
        {
            string code = wxUserModel?.Code;
            var ret = _wxRegisterService.VerifyCode(WorkUser, code);
            return Ok(ret);
        }

        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult CheckMoblie(string mobile)
        {
            var result = _wxRegisterService.CheckMoblie(mobile);
            return Ok(result);
        }

        /// <summary>
        /// 注册医生
        /// </summary>
        /// <param name="wxUserModel">
        /// 微信用户信息：
        /// HospitalName，
        /// DepartmentName
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult RegisterDoctor(WxUserModel wxUserModel)
        {
            var ret = _wxRegisterService.UpdateDocHospitalOrDept(WorkUser, wxUserModel, true);
            return Ok(ret);
        }

        /// <summary>
        /// 更新医生医院及科室
        /// </summary>
        /// <param name="wxUserModel">
        /// 微信用户信息：
        /// HospitalName，
        /// DepartmentName
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult UpdateDocHospitalOrDept(WxUserModel wxUserModel)
        {
            var ret = _wxRegisterService.UpdateDocHospitalOrDept(WorkUser, wxUserModel, false);
            return Ok(ret);
        }

        /// <summary>
        /// 上传医生个人信息证明图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult UploadPicture()
        {
            var ret = _wxRegisterService.UploadPicture(WorkUser, HttpContext.Current.Request);
            return Ok(ret);
        }

        /// <summary>
        /// 上传签名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult UploadSign()
        {
           
            var ret = _wxRegisterService.UploadSign(WorkUser, HttpContext.Current.Request);
            return Ok(ret);
        }
        /// <summary>
        /// 判断输入的姓名是否合格
        /// </summary>
        /// <param name="wxUserModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult IsQualified(WxUserModel wxUserModel) {
            var ret = _wxRegisterService.IsQualified(wxUserModel.UserName);
            return Ok(ret);
        }
     
    }
}
