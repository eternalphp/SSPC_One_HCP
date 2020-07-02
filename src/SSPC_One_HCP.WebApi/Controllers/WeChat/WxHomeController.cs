using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.WebApi.CustomerAuth;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 主要信息
    /// </summary>
    public class WxHomeController : WxBaseApiController
    {
        private readonly IWxHomeService _wxHomeService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wxHomeService"></param>
        public WxHomeController(IWxHomeService wxHomeService)
        {
            _wxHomeService = wxHomeService;
        }
        /// <summary>
        /// 获取当前微信用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult GetWxCurrentUser()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间

            ReturnValueModel rvm = new ReturnValueModel
            {
                Success = true,
                Msg = "",
                Result = new
                {
                    WorkUser.WxUser
                }
            };
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return Ok(rvm);
        }


        /// <summary>
        /// 参加会议次数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAttendMeetings([FromUri]DateTime dateTime)
        {
            var ret = _wxHomeService.GetAttendMeetings(WorkUser, dateTime);
            return Ok(ret);
        }

        /// <summary>
        /// 学习时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetLeaningTimes([FromUri]DateTime dateTime)
        {
            var ret = _wxHomeService.GetLeaningTimes(WorkUser, dateTime);
            return Ok(ret);
        }


        /// <summary>
        /// 访问城市
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetVisitCitys([FromUri]DateTime dateTime)
        {
            var ret = _wxHomeService.GetVisitCitys(WorkUser, dateTime);
            return Ok(ret);
        }

        /// <summary>
        /// 提交意见反馈
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Feedback(FeedbackViewModel viewModel)
        {
            var ret = _wxHomeService.AddFeedback(viewModel, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 删除个人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteMyAccount()
        {
            var ret = _wxHomeService.DeleteMyAccount(WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 费森竞争产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult GetCompetingProduct()
        {
            var ret = _wxHomeService.GetCompetingProduct();
            return Ok(ret);
        }
    }
}
