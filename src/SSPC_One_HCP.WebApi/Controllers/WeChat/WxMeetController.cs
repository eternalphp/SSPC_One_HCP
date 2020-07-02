using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels.MeetModels;
using SSPC_One_HCP.Services.Implementations.Dto;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.WebApi.CustomerAuth;

namespace SSPC_One_HCP.WebApi.Controllers.WeChat
{
    /// <summary>
    /// 微信端会议接口
    /// </summary>
    public class WxMeetController : WxBaseApiController
    {

        private readonly IWxMeetService _wxMeetService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wxMeetService"></param>
        public WxMeetController(IWxMeetService wxMeetService)
        {
            _wxMeetService = wxMeetService;
        }

        /// <summary>
        /// 会议详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetMeetInfo(MeetInfo meetInfo)
        {
            var ret = _wxMeetService.GetMeetInfo(meetInfo, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取会议日历
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult GetMeetCalendar([FromUri]DateTime dateTime)
        {
            var ret = _wxMeetService.GetMeetCalendar(WorkUser, dateTime);
            return Ok(ret);
        }

        /// <summary>
        /// 获取会议
        /// </summary>
        /// <param name="meetInfo">会议信息，之传入Id</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetDetail(MeetInfo meetInfo)
        {
            var ret = _wxMeetService.GetMeetDetail(meetInfo, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 获取历史参会列表
        /// </summary>
        /// <param name="rowMeet">分页</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetHistoryMeeting(RowNumModel<MeetInfo> rowMeet)
        {
            var ret = _wxMeetService.GetHistoryMeeting(rowMeet, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取会议列表
        /// </summary>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult GetMeetList(RowNumModel<MeetInfo> rowMeet)
        {
            var ret = _wxMeetService.GetMeetList(rowMeet, WorkUser);

            return Ok(ret);
        }
        /// <summary>
        /// 获取所有会议列表（推荐会和历史会）
        /// </summary>
        /// <param name="rowMeet"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult GetMeetLists(RowNumModel<MeetInfo> rowMeet)
        {
            var ret = _wxMeetService.GetMeetLists(rowMeet, WorkUser);

            return Ok(ret);
        }
        /// <summary>
        /// 搜索 条件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowUnregistered]
        public IHttpActionResult GetDepartmentInfo(int type)
        {
            var ret = _wxMeetService.GetDepartmentInfo(type, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 推荐会议搜索
        /// </summary>
        /// <param name="rowMeet"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        // [AllowAnonymous]
        public IHttpActionResult GetMeetRecommendSearch(RowNumModel<MeetSearchInputDto> rowMeet)
        {
            var ret = _wxMeetService.GetMeetRecommendSearch(rowMeet, WorkUser);

            return Ok(ret);
        }
        /// <summary>
        /// 历史会议搜索
        /// </summary>
        /// <param name="rowMeet"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowUnregistered]
        public IHttpActionResult GetMeetHistorySearch(RowNumModel<MeetSearchInputDto> rowMeet)
        {
            var ret = _wxMeetService.GetMeetHistorySearch(rowMeet, WorkUser);

            return Ok(ret);
        }


        /// <summary>
        /// 会议报名
        /// </summary>
        /// <param name="mmo">传入MeetId,IsRemind,WarnMinutes</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult MeetingOrder(MyMeetOrderViewModel mmo)
        {
            var ret = _wxMeetService.MeetingOrder(mmo, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 会议签到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult MeetingSignUp(MeetSignUpViewModel viewModel)
        {
            var ret = _wxMeetService.MeetingSignUp(viewModel, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 更新状态：已查看会议详情 (导出会议报告时需要)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult KnownMeetingDetail(MeetSignUp model)
        {
            var ret = _wxMeetService.KnownMeetingDetail(model, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 未开始的会议
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult NotStartMeet()
        {
            var ret = _wxMeetService.NotStartMeet(WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 根据会议id和问卷类型获取问卷
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetQAInfo(MeetQARelationViewModel meetQA)
        {
            var ret = _wxMeetService.GetMeetQAInfo(meetQA, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 提交会议问卷结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CommitQAResult(WxMeetQAResultViewModel meetQA)
        {
            var ret = _wxMeetService.CommitQAResult(meetQA, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 根据日期和用户查询Id
        /// </summary>
        /// <param name="meet"></param>
        /// <param name="wxuser"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetByDate(RowNumModel<MeetInfo> meet)
        {
            var ret = _wxMeetService.GetMeetByDate(meet, WorkUser);
            return Ok(ret);
        }
    }
}
