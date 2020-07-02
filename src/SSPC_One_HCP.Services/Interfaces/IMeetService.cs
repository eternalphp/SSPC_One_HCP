using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.Approval;
using SSPC_One_HCP.Core.Domain.ViewModels.MeetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 会议管理
    /// </summary>
    public interface IMeetService
    {
        ReturnValueModel GetMeetInfoTest(MeetInfo meetInfo);
        /// <summary>
        /// 获取会议列表
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetMeetList(RowNumModel<MeetSearchViewModel> rowNum, WorkUser workUser);

        /// <summary>
        /// 获取会议列表，用于选择过往会议的问卷
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetMeetListOfQA(RowNumModel<MeetSearchViewModel> rowNum, WorkUser workUser);

        /// <summary>
        /// 新增或更新会议
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateMeetInfo(MeetInfoViewModel meetInfo, WorkUser workUser);

        /// <summary>
        /// 删除会议
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteMeetInfo(MeetInfo meetInfo, WorkUser workUser);

        /// <summary>
        /// 获取会议详情
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetInfo(MeetInfo meetInfo, WorkUser workUser);

        /// <summary>
        /// 获取会议签到一览
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        ReturnValueModel GetMeetSignUpList(RowNumModel<MeetInfo> rowNum);

        /// <summary>
        /// 获取会议报名一览
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        ReturnValueModel GetMeetOrderList(RowNumModel<MeetInfo> rowNum);

        /// <summary>
        /// 导出会议签到列表
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        ReturnValueModel ExportMeetSignUpList(MeetInfo meetInfo);

        /// <summary>
        /// 导出会议报名列表
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        ReturnValueModel ExportMeetOrderList(MeetInfo meetInfo);

        /// <summary>
        /// 获取参会医生报告一览
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        ReturnValueModel GetMeetSituation(RowNumModel<MeetInfo> rowNum);

        /// <summary>
        /// 导出参会医生报告
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        ReturnValueModel ExportMeetSituation(MeetInfo meetInfo);

        /// <summary>
        /// 获取会议问卷情况一览表
        /// </summary>
        /// <param name="rowNum">会议信息（只需会议ID）</param>
        /// <returns></returns>
        ReturnValueModel GetMeetQAResultList(RowNumModel<MeetInfo> rowNum);

        /// <summary>
        /// 导出会议调研报告
        /// </summary>
        /// <param name="meetInfo">会议信息（只需会议ID）</param>
        /// <returns></returns>
        ReturnValueModel ExportMeetQAResultReport(MeetInfo meetInfo);

        /// <summary>
        /// 新建或修改会议问卷
        /// </summary>
        /// <param name="meetQA"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateMeetQA(MeetQARelationViewModel meetQA, WorkUser workUser);

        /// <summary>
        /// 根据会议id和问卷类型获取问卷
        /// </summary>
        /// <param name="meetQA"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetQAInfo(MeetQARelationViewModel meetQA);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="meetQA"></param>
        /// <returns></returns>
        ReturnValueModel DeleteMeetQAInfo(MeetQARelationViewModel meetQA);

        /// <summary>
        /// 提交会议的审核结果
        /// </summary>
        /// <param name="approvalResult">审核结果</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel Approval(ApprovalResultViewModel approvalResult, WorkUser workUser);

        /// <summary>
        /// 获取参会人员
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetDoctorMeeting(RowNumModel<MeetInfo> meetInfo, WorkUser workUser);
    }
}
