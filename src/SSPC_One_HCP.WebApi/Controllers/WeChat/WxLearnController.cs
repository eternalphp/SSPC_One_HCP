using System;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 学习相关
    /// </summary>
    public class WxLearnController : WxBaseApiController
    {
        private readonly IWxLearnService _wxLearnService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wxLearnService"></param>
        public WxLearnController(IWxLearnService wxLearnService)
        {
            _wxLearnService = wxLearnService;
        }
        /// <summary>
        /// 记录学习时间
        /// </summary>
        /// <param name="myLRecord">学习记录信息</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddLearn(MyLRecord myLRecord)
        {
            var ret = _wxLearnService.AddLearn(myLRecord, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="NowDate"></param>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult AddLearnForTest(string userid, DateTime NowDate)
        {
            var ret = _wxLearnService.AddLearnForTest(userid, NowDate);
            return Ok(ret);
        }
    }
}