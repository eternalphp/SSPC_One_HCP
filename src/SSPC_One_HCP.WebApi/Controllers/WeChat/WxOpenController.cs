using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 微信API
    /// </summary>
    public class WxOpenController : ApiController
    {
        #region 声明
        private readonly IWxOpenService _wxOpenService;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wxOpenService"></param>
        public WxOpenController(IWxOpenService wxOpenService)
        {
            _wxOpenService = wxOpenService;
        }
        #endregion

        #region 方法

        /// <summary>
        /// 获取UnionID
        /// </summary>
        /// <param name="wxUserInfoRequestModel">传入微信获取的相关信息</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetUnionId( [FromBody]WxUserInfoRequestModel wxUserInfoRequestModel)
        {
            var ret = _wxOpenService.GetUnionId(wxUserInfoRequestModel);
            return Ok(ret);
        }

        /// <summary>
        /// 文库登录验证
        /// </summary>
        /// <param name="user">传入微信获取的相关信息</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult WKLogin([FromBody]WKUser user)
        {
            var ret = _wxOpenService.WKLogin(user);
            return Ok(ret);
        }

        /// <summary>
        /// 保存FormID
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult FormID([FromBody]TemplateForm form)
        {
            var ret = _wxOpenService.FormID(form);
            return Ok(ret);
        }


        /// <summary>
        /// 解析手机号
        /// </summary>
        /// <param name="phoneModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetWXPhone([FromBody]DecodePhoneModel phoneModel)
        {
            var ret = _wxOpenService.GetWXPhone(phoneModel);
            return Ok(ret);
        }
        #endregion
    }
}
