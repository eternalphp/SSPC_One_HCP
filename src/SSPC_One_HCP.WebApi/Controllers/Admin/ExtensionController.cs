using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.WebApi.CustomerAuth;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 公众号推广，二维码推广管理
    /// </summary>
    public class ExtensionController : BaseApiController
    {
        private readonly IExtensionService _ExtensionService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExtensionService"></param>
        public ExtensionController(IExtensionService ExtensionService)
        {
            _ExtensionService = ExtensionService;
        }
        /// <summary>
        /// 获取公众号列表
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetPublicAccount(RowNumModel<PublicAccount> publicaccount) {
            var ret = _ExtensionService.GetPublicAccount(publicaccount, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 新增或更新公众号信息
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddorUpdatePublicAccount(PublicAccount publicaccount) {
            var ret = _ExtensionService.AddorUpdatePublicAccount(publicaccount, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 删除公众号信息
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeletePublicAccount(PublicAccount publicaccount) {
            var ret = _ExtensionService.DeletePublicAccount(publicaccount, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 新增或更新二维码推广信息
        /// </summary>
        /// <param name="qRcodeExtension"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddorUpdateQRCodeExtension(QRcodeExtension qRcodeExtension) {
            var ret = _ExtensionService.AddorUpdateQRCodeExtension(qRcodeExtension,WorkUser);
            return Ok(ret);
        }


    }
}
