using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 推广二维码
    /// </summary>
    public class SpreadQRCodeController : BaseApiController
    {
        private readonly IBaseService<SpreadQRCode> _SpreadQRCode;

        public SpreadQRCodeController(IBaseService<SpreadQRCode> spreadQRCode)
        {
            _SpreadQRCode = spreadQRCode;
        }

        /// <summary>
        /// 获取推广二维码列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetSpreadQRCodeList(RowNumModel<SpreadQRCode> rowNum)
        {
            var reuslt = _SpreadQRCode.GetList(rowNum, WorkUser);
            return Ok(reuslt);
        }

        /// <summary>
        /// 获取推广二维码详情
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetSpreadQRCode(SpreadQRCode item)
        {
            var reuslt = _SpreadQRCode.GetItem(item, WorkUser);
            return Ok(reuslt);
        }

        /// <summary>
        /// 新增推广二维码
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdate(SpreadQRCode item)
        {
            var reuslt = _SpreadQRCode.AddOrUpdateItem(item, WorkUser);
            return Ok(reuslt);
        }        
        
        /// <summary>
        /// 删除推广二维码
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteItem(SpreadQRCode item)
        {
            var reuslt = _SpreadQRCode.DeleteItem(item, WorkUser);
            return Ok(reuslt);
        }
        /// <summary>
        /// 访问数+1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public   IHttpActionResult Visitor([FromUri]string id)
        {
            SpreadQRCode item = new SpreadQRCode() { Id = id };
            var reuslt = _SpreadQRCode.AddItem(item, null);
            return Ok(reuslt);            
        }
    }
}
