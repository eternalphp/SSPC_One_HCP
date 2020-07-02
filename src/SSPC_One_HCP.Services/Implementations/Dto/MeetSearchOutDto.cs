using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Enums;

namespace SSPC_One_HCP.Services.Implementations.Dto
{
    public class MeetSearchOutDto
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        public string Id { get;  set; }
        /// <summary>
        /// 会议标题
        /// </summary>
        public string MeetTitle { get;  set; }
        /// <summary>
        /// 会议主题
        /// </summary>
        public string MeetSubject { get;  set; }
        /// <summary>
        /// 会议对应的科室
        /// </summary>
        public string MeetDep { get;  set; }
        /// <summary>
        /// 会议类型
        /// 1、线上会议
        /// 2、线下会议（科室会）
        /// 3、线下会议（城市会）
        /// 4、全国会议
        /// </summary>
        public int? MeetType { get;  set; }
        /// <summary>
        /// 会议简介
        /// </summary>
        public string MeetIntroduction { get;  set; }
        /// <summary>
        /// 会议开始时间
        /// </summary>
        public DateTime? MeetStartTime { get;  set; }
        /// <summary>
        /// 会议结束时间
        /// </summary>
        public DateTime? MeetEndTime { get;  set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get;  set; }
        /// <summary>
        /// 会议日程
        /// </summary>
        public DateTime? MeetDate { get;  set; }
        /// <summary>
        /// 会议讲者姓名
        /// </summary>
        public string Speaker { get;  set; }

        /// <summary>
        /// 讲者简历
        /// </summary>
        public string SpeakerDetail { get;  set; }
        /// <summary>
        /// 会议地址或会议视频链接
        /// </summary>
        public string MeetAddress { get;  set; }
        /// <summary>
        /// 回看地址
        /// </summary>
        public string ReplayAddress { get;  set; }
        /// <summary>
        /// 会议资料
        /// </summary>
        public string MeetData { get;  set; }
        /// <summary>
        /// 会议二维码地址
        /// </summary>
        public string MeetCodeUrl { get;  set; }
        /// <summary>
        /// 开会城市
        /// </summary>
        public string MeetCity { get;  set; }
        /// <summary>
        /// 开会人数
        /// </summary>
        public int MeetingNumber { get;  set; }
        /// <summary>
        /// 开会位置(经纬度)
        /// </summary>
        public string MeetSite { get;  set; }
        /// <summary>
        /// 封面图（小）
        /// </summary>
        public string MeetCoverSmall { get;  set; }
        /// <summary>
        /// 封面图（大）
        /// </summary>
        public string MeetCoverBig { get;  set; }
        /// <summary>
        /// 是否完成审核
        /// 1、已审核
        /// 2、新增未审核
        /// 3、审核拒绝
        /// 4、已锁定
        /// 5、已作废
        /// 6、删除未审核
        /// 7、已删除
        /// 8、编辑未审核
        /// </summary>
        public EnumComplete IsCompleted { get;  set; }
        /// <summary>
        /// 审批备注
        /// </summary>
        public string ApprovalNote { get;  set; }
        /// <summary>
        /// 是否推荐
        /// 0、否
        /// 1、是
        /// </summary>
        public int? IsChoiceness { get;  set; }
        /// <summary>
        /// 是否精选
        /// 0、否
        /// 1、是
        /// </summary>
        public int? IsHot { get;  set; }
        /// <summary>
        /// 主席
        /// </summary>
        public string Chairman { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        public string Hospital { get; set; }

        /// <summary>
        /// 会议状态
        /// 0、待开播
        /// 1、进行中
        /// 2、已完成
        /// </summary>
        public int? MeetState { get; set; }
        /// <summary>
        /// 状态说明
        /// </summary>
        public string MeetStateName { get; set; }
        public int? DepartmentType { get; internal set; }
        public string DepartmentId { get; internal set; }
    }
}
