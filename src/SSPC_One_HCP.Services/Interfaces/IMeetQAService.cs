using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.MeetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 会议问卷
    /// </summary>
    public interface IMeetQAService
    {
        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetQAList(RowNumModel<MeetQAContentViewModel> rowNum);

        /// <summary>
        /// 获取问卷列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        ReturnValueModel GetQuestionList(RowNumModel<QuestionModel> rowNum);

        /// <summary>
        /// 新增或修改问卷
        /// </summary>
        /// <param name="meetQA"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateMeetQA(MeetQAContentViewModel meetQA, WorkUser workUser);

        /// <summary>
        /// 删除会议问卷
        /// </summary>
        /// <param name="meetQA"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteMeetQA(QuestionModel meetQA, WorkUser workUser);

        /// <summary>
        /// 获取会议问卷详情
        /// </summary>
        /// <param name="question"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetQA(QuestionModel question, WorkUser workUser);
    }
}
