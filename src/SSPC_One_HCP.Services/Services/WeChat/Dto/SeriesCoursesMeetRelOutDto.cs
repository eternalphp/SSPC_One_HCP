using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WeChat.Dto
{
    public class SeriesCoursesMeetRelOutDto
    {
        /// <summary>
        /// 封面图（小）
        /// </summary>
        public string MeetCoverSmall { get; set; }
        /// <summary>
        /// 封面图（大）
        /// </summary>
        public string MeetCoverBig { get; set; }
        /// <summary>
        /// 会议开始时间
        /// </summary>
        public DateTime? MeetStartTime { get; set; }
        /// <summary>
        /// 会议结束时间
        /// </summary>
        public DateTime? MeetEndTime { get; set; }
        /// <summary>
        /// 会议ID
        /// </summary>
        public string MeetInfoId { get; set; }

        public string MeetCoverSmallId { get; set; }
        public string MeetCoverBigId { get; set; }
        /// <summary>
        /// 会议标题
        /// </summary>
        public string MeetTitle { get; set; }
        /// <summary>
        /// 审批备注
        /// </summary>
        public string ApprovalNote { get; set; }
        /// <summary>
        /// 是否推荐
        /// 0、否
        /// 1、是
        /// </summary>
        public int? IsChoiceness { get; set; }
        /// <summary>
        /// 是否精选
        /// 0、否
        /// 1、是
        /// </summary>
        public int? IsHot { get; set; }
        /// <summary>
        /// 会议二维码地址
        /// </summary>
        public string MeetCodeUrl { get; set; }
        /// <summary>
        /// 会议资料
        /// </summary>
        public string MeetData { get; set; }
        /// <summary>
        /// 会议地址或会议视频链接
        /// </summary>
        public string MeetAddress { get; set; }
        /// <summary>
        /// 回看地址
        /// </summary>
        public string ReplayAddress { get; set; }
        public int? MeetType { get; set; }
        public string Chairman { get; set; }
        public string Hospital { get; set; }
        public int Sort { get;  set; }
    }
}
