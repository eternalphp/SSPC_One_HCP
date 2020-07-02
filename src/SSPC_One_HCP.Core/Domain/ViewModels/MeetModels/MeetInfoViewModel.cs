using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    /// <summary>
    /// 会议ViewModel
    /// </summary>
    [NotMapped]
    [DataContract]
    public class MeetInfoViewModel
    {
        /// <summary>
        /// 会议信息
        /// </summary>
        [DataMember]
        public MeetInfo Meet { get; set; }

        /// <summary>
        /// 会议对应的产品及科室
        /// </summary>
        [DataMember]
        public ProductBuDeptSelectionViewModel ProductAndDeps { get; set; }

        /// <summary>
        /// 会议日程
        /// </summary>
        [DataMember]
        public IEnumerable<MeetScheduleViewModel> Schedules { get; set; }

        /// <summary>
        /// 讲者信息
        /// </summary>
        [DataMember]
        public IEnumerable<MeetSpeaker> Speakers { get; set; }

        /// <summary>
        /// 会议资料
        /// </summary>
        [DataMember]
        public IEnumerable<MeetFile> Files { get; set; }

        /// <summary>
        /// 会议状态
        /// 0、未开始
        /// 1、进行中
        /// 2、已完成
        /// 3、删除中
        /// 4、已失效
        /// 5、禁用
        /// </summary>
        [DataMember]
        public int? IsMeetEnd { get; set; }

        /// <summary>
        /// 会议标识(暂未使用)
        /// </summary>
        [DataMember]
        public IEnumerable<TagView> Tags { get; set; }

        /// <summary>
        /// 会议封面图片
        /// </summary>
        [DataMember]
        public IEnumerable<MeetPic> CoverPictures { get; set; }

        /// <summary>
        /// 参会医生编号
        /// </summary>
        [DataMember]
        public IEnumerable<string> DoctorList { get; set; }

        /// <summary>
        /// 标签分组信息
        /// </summary>
        [DataMember]
        public IEnumerable<string> TagGroupList { get; set; }
    }
}
