using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
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
    /// 会议问卷接口
    /// </summary>
    public class MeetQAController : BaseApiController
    {
        private readonly IMeetQAService _meetQAService;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MeetQAController.MeetQAController(IMeetQAService)'
        public MeetQAController(IMeetQAService meetQAService)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'MeetQAController.MeetQAController(IMeetQAService)'
        {
            _meetQAService = meetQAService;
        }

        /// <summary>
        /// 获取会议问卷列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetQAList(RowNumModel<MeetQAContentViewModel> rowNum)
        {
            var ret = _meetQAService.GetMeetQAList(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetQuestionList(RowNumModel<QuestionModel> rowNum)
        {
            var ret = _meetQAService.GetQuestionList(rowNum);
            return Ok(ret);
        }

        /// <summary>
        /// 新增或修改题目和答案
        /// </summary>
        /// <param name="meetQA"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateMeetQA(MeetQAContentViewModel meetQA)
        {
            var ret = _meetQAService.AddOrUpdateMeetQA(meetQA, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 删除会议问卷
        /// </summary>
        /// <param name="meetQA"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteMeetQA(QuestionModel meetQA)
        {
            var ret = _meetQAService.DeleteMeetQA(meetQA, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取会议问卷详情
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMeetQA(QuestionModel question)
        {
            var ret = _meetQAService.GetMeetQA(question, WorkUser);
            return Ok(ret);
        }
    }
}
