using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Services.WebView.Dto;
using SSPC_One_HCP.Services.Services.WebView.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// H5-参会报名表
    /// </summary>
    public class WvRegistrationFormController : ApiController
    {
        private readonly IRegistrationFormService _registrationFormService;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="registrationFormService"></param>
        public WvRegistrationFormController(IRegistrationFormService registrationFormService)
        {
            _registrationFormService = registrationFormService;
        }
        /// <summary>
        /// 通过code换取网页授权access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserInfo(string code)
        {
            var ret = _registrationFormService.GetUserInfo(code);
            return Ok(ret);
        }

        /// <summary>
        /// 新增修改 参会报名表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdate(RegistrationFormInputDto inputDto)
        {
            var ret = _registrationFormService.AddOrUpdate(inputDto);
            return Ok(ret);
        }
    }
}
