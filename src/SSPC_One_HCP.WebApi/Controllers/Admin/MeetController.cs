using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.Approval;
using SSPC_One_HCP.Core.Domain.ViewModels.MeetModels;
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
    /// 会议管理
    /// </summary>
    public class MeetController : BaseApiController
    {
        private readonly IMeetService _meetService;

        /// <summary>
        /// 会议管理
        /// </summary>
        /// <param name="meetService"></param>
        public MeetController(IMeetService meetService)
        {
            _meetService = meetService;
        }

        /// <summary>
        /// 获取会议列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetList(RowNumModel<MeetSearchViewModel> rowNum)
        {
            var ret = _meetService.GetMeetList(rowNum, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取会议列表，用于选择过往会议的问卷
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetListOfQA(RowNumModel<MeetSearchViewModel> rowNum)
        {
            var ret = _meetService.GetMeetListOfQA(rowNum, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 新增或更新会议
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateMeetInfo(MeetInfoViewModel meetInfo)
        {
            var ret = _meetService.AddOrUpdateMeetInfo(meetInfo, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 删除会议
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteMeetInfo(MeetInfo meetInfo)
        {
            var ret = _meetService.DeleteMeetInfo(meetInfo, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取会议详情
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetInfo(MeetInfo meetInfo)
        {
            var ret = _meetService.GetMeetInfo(meetInfo, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取参会医生报告一览
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetSituation(RowNumModel<MeetInfo> rowNum)
        {
            var ret = _meetService.GetMeetSituation(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 导出参会医生报告
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportMeetSituation(MeetInfo meetInfo)
        {
            var ret = _meetService.ExportMeetSituation(meetInfo);
            return Ok(ret);
        }

        /// <summary>
        /// 获取会议签到一览
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetSignUpList(RowNumModel<MeetInfo> rowNum)
        {
            var ret = _meetService.GetMeetSignUpList(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 导出会议签到列表
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportMeetSignUpList(MeetInfo meetInfo)
        {
            var ret = _meetService.ExportMeetSignUpList(meetInfo);
            return Ok(ret);
        }

        /// <summary>
        /// 获取会议报名一览
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetOrderList(RowNumModel<MeetInfo> rowNum)
        {
            var ret = _meetService.GetMeetOrderList(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 导出会议报名列表
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportMeetOrderList(MeetInfo meetInfo)
        {
            var ret = _meetService.ExportMeetOrderList(meetInfo);
            return Ok(ret);
        }

        /// <summary>
        /// 获取会议调研报告一览
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetQAResultList(RowNumModel<MeetInfo> rowNum)
        {
            var ret = _meetService.GetMeetQAResultList(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 导出会议调研报告
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ExportMeetQAResultReport(MeetInfo meetInfo)
        {
            var ret = _meetService.ExportMeetQAResultReport(meetInfo);
            return Ok(ret);
        }

        /// <summary>
        /// 根据会议id和问卷类型获取问卷
        /// </summary>
        /// <param name="meetQA"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetQAInfo(MeetQARelationViewModel meetQA)
        {
            var ret = _meetService.GetMeetQAInfo(meetQA);
            return Ok(ret);
        }

        /// <summary>
        /// 新增或修改会议的问卷列表
        /// </summary>
        /// <param name="meetQA"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateMeetQA(MeetQARelationViewModel meetQA)
        {
            var ret = _meetService.AddOrUpdateMeetQA(meetQA, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 删除会议问卷
        /// </summary>
        /// <param name="meetQA"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteMeetQAInfo(MeetQARelationViewModel meetQA)
        {
            var ret = _meetService.DeleteMeetQAInfo(meetQA);
            return Ok(ret);
        }

        /// <summary>
        /// 提交会议的审核结果
        /// </summary>
        /// <param name="approvalResult">审核结果</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Approval(ApprovalResultViewModel approvalResult)
        {
            var ret = _meetService.Approval(approvalResult, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取单个会议可参加的人员
        /// </summary>
        /// <param name="rowNum"></param>      
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDoctorMeeting(RowNumModel<MeetInfo> rowNum)
        {
            var ret = _meetService.GetDoctorMeeting(rowNum, WorkUser);
            return Ok(ret);
        }
    }
}
