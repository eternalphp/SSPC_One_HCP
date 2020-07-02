using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Implementations;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 发送频率
    /// </summary>
    public class SendRateController : BaseApiController
    {
        private readonly IBaseService<SendRate> sendRateService;        

        public SendRateController(IBaseService<SendRate> sendRate)
        {
            sendRateService = sendRate;
        }

        
        /// <summary>
        /// 指定医生配置
        /// </summary>
        /// <param name="sendRate">DoctorID</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetItem(SendRate sendRate)
        {
            sendRate.IsDefault = false;
            var reuslt = sendRateService.GetItem(sendRate, WorkUser);

            return Ok(reuslt);
        }

        /// <summary>
        /// 获取默认配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GeDefaultItem()
        {
            var sendRate = new SendRate();
            sendRate.IsDefault = true;
            var reuslt = sendRateService.GetItem(sendRate, WorkUser);
            return Ok(reuslt);
        }

        /// <summary>
        /// 增改
        /// </summary>
        /// <param name="sendRate"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SetSendRate(SendRate sendRate)
        {
            var reuslt = sendRateService.AddOrUpdateItem(sendRate, WorkUser);
            return Ok(reuslt);
        }
    }
}
