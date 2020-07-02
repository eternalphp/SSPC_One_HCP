using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels.MeetModels;
using SSPC_One_HCP.Services.Implementations.Dto;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWxMeetService
    {
        /// <summary>
        /// 会议详情
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetInfo(MeetInfo meetInfo, WorkUser workUser);
        /// <summary>
        /// 获取会议
        /// </summary>
        /// <param name="meetInfo">会议信息，之传入Id</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetDetail(MeetInfo meetInfo, WorkUser workUser);

        /// <summary>
        /// 获取历史参会列表
        /// </summary>
        /// <param name="rowMeet">分页</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        ReturnValueModel GetHistoryMeeting(RowNumModel<MeetInfo> rowMeet, WorkUser workUser);

        /// <summary>
        /// 获取会议列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetList(RowNumModel<MeetInfo> rowNum, WorkUser workUser);



        /// <summary>
        /// 获取所有会议列表（推荐会和历史会）
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetLists(RowNumModel<MeetInfo> rowNum, WorkUser workUser);

         ReturnValueModel GetDepartmentInfo(int type, WorkUser workUser);
        /// <summary>
        /// 推荐会议搜索
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetRecommendSearch(RowNumModel<MeetSearchInputDto> row, WorkUser workUser);
        /// <summary>
        /// 历史会议搜索
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetHistorySearch(RowNumModel<MeetSearchInputDto> row, WorkUser workUser);
        /// <summary>
        /// 获取会议日历
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetCalendar(WorkUser workUser, DateTime? dateTime = null);

        /// <summary>
        /// 会议报名
        /// </summary>
        /// <param name="mmo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel MeetingOrder(MyMeetOrderViewModel mmo, WorkUser workUser);

        /// <summary>
        /// 会议签到
        /// </summary>
        /// <returns></returns>
        ReturnValueModel MeetingSignUp(MeetSignUpViewModel viewModel, WorkUser workUser);

        /// <summary>
        /// 更新状态：已查看会议详情 (导出会议报告时需要)
        /// </summary>
        /// <returns></returns>
        ReturnValueModel KnownMeetingDetail(MeetSignUp model, WorkUser workUser);

        /// <summary>
        /// 未开始的会议
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel NotStartMeet(WorkUser workUser);

        /// <summary>
        /// 根据会议id和问卷类型获取问卷
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetMeetQAInfo(MeetQARelationViewModel meetQA, WorkUser workUser);

        /// <summary>
        /// 提交会议问卷结果
        /// </summary>
        /// <returns></returns>
        ReturnValueModel CommitQAResult(WxMeetQAResultViewModel meetQA, WorkUser workUser);
        /// <summary>
        /// 跟单据日期获取会议列表
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        ReturnValueModel GetMeetByDate(RowNumModel<MeetInfo> meet, WorkUser workUser);
    }
}
