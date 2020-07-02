using SSPC_One_HCP.Core.Domain.Enums;
using System;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议模型
    /// </summary>
    [DataContract]
    public class MeetInfo : BaseEntity
    {
        /// <summary>
        /// 会议标题
        /// </summary>
        [DataMember]
        public string MeetTitle { get; set; }
        /// <summary>
        /// 会议主题
        /// </summary>
        [DataMember]
        public string MeetSubject { get; set; }

        /// <summary>
        /// 会议类型
        /// 1、线上会议
        /// 2、线下会议（科室会）
        /// 3、线下会议（城市会）
        /// 4、全国会议
        /// </summary>
        [DataMember]
        public int? MeetType { get; set; }

        /// <summary>
        /// 会议对应的科室
        /// </summary>
        [DataMember]
        public string MeetDep { get; set; }

        /// <summary>
        /// 会议简介
        /// </summary>
        [DataMember]
        public string MeetIntroduction { get; set; }

        /// <summary>
        /// 会议开始时间
        /// </summary>
        [DataMember]
        public DateTime? MeetStartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        [DataMember]
        public DateTime? MeetEndTime { get; set; }

        /// <summary>
        /// 会议日程
        /// </summary>
        [DataMember]
        public DateTime? MeetDate { get; set; }

        /// <summary>
        /// 会议讲者姓名
        /// </summary>
        [DataMember]
        public string Speaker { get; set; }

        /// <summary>
        /// 讲者简历
        /// </summary>
        [DataMember]
        public string SpeakerDetail { get; set; }

        /// <summary>
        /// 会议地址或会议视频链接
        /// </summary>
        [DataMember]
        public string MeetAddress { get; set; }

        /// <summary>
        /// 回看地址
        /// </summary>
        [DataMember]
        public string ReplayAddress { get; set; }

        /// <summary>
        /// 会议资料
        /// </summary>
        [DataMember]
        public string MeetData { get; set; }

        /// <summary>
        /// 会议二维码地址
        /// </summary>
        [DataMember]
        public string MeetCodeUrl { get; set; }
        /// <summary>
        /// 开会城市
        /// </summary>
        [DataMember]
        public string MeetCity { get; set; }
        /// <summary>
        /// 开会人数
        /// </summary>
        [DataMember]
        public int MeetingNumber { get; set; }
        /// <summary>
        /// 开会位置(经纬度)
        /// </summary>
        [DataMember]
        public string MeetSite { get; set; }
        /// <summary>
        /// 封面图（小）
        /// </summary>
        [DataMember]
        public string MeetCoverSmall { get; set; }
        /// <summary>
        /// 封面图（大）
        /// </summary>
        [DataMember]
        public string MeetCoverBig { get; set; }
        /// <summary>
        /// 直播图 375*210
        /// </summary>
        [DataMember]
        public string LivePicture { get; set; }
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
        [DataMember]
        public EnumComplete? IsCompleted { get; set; }

        /// <summary>
        /// 是否推荐
        /// 0、否
        /// 1、是
        /// </summary>
        [DataMember]
        public int? IsChoiceness { get; set; }

        /// <summary>
        /// 是否精选
        /// 0、否
        /// 1、是
        /// </summary>
        [DataMember]
        public int? IsHot { get; set; }

        /// <summary>
        /// 修改前原来的记录Id
        /// </summary>
        [DataMember]
        public string OldId { get; set; }

        /// <summary>
        /// 审批备注
        /// </summary>
        [DataMember]
        public string ApprovalNote { get; set; }

        /// <summary>
        /// 第三方app创建的会议，数据来源
        /// null 或者 第三方微信公众号的AppId
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// 第三方app创建的会议，会议编号
        /// </summary>
        [DataMember]
        public string SourceId { get; set; }

        /// <summary>
        /// 第三方app创建的会议，医院
        /// </summary>
        [DataMember]
        public string SourceHospital { get; set; }

        /// <summary>
        /// 第三方app创建的会议，科室
        /// </summary>
        [DataMember]
        public string SourceDepartment { get; set; }

        /// <summary>
        /// 是否邮件提醒
        /// </summary>
        [DataMember]
        public int HasReminded { get; set; }
        /// <summary>
        /// 邀请函详情
        /// </summary>
        [DataMember]
        public string InvitationDetail { get; set; }
        /// <summary>
        /// 内外会议（测试会议）
        /// 0：对外
        /// 1：对内
        /// </summary>
        [DataMember]
        public int WithinExternalType { get; set; }
        /// <summary>
        /// 登录权限设置
        /// 0：登录观看
        /// 1：不登录观看
        /// </summary>
        [DataMember]
        public int WatchType { get; set; }

        /// <summary>
        /// 在线总数（pv数）
        /// </summary>
        [DataMember]
        public int? PVCount { get; set; }
        /// <summary>
        /// 是否开启聊天
        /// 0、否
        /// 1、是
        /// </summary>
        [DataMember]
        public int? IsChat { get; set; }

        /// <summary>
        /// 是否禁言
        /// 0、否
        /// 1、是
        /// </summary>
        [DataMember]
        public int? IsForbiddenWords { get; set; }
    }
}
