using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议成员及分组信息
    ///
    /// </summary>
    [DataContract]
    public class DoctorMeeting : BaseEntity
    {
        /// <summary>
        /// 会议编号
        /// </summary>
        [DataMember]
        public string MeetingID { get; set; }

        /// <summary>
        /// 会议医生 二选一
        /// </summary>
        [DataMember]
        public string DoctorID { get; set; }

        /// <summary>
        /// 标签分组信息 二选一
        /// </summary>
        [DataMember]
        public string TagGroupID { get;set; }

        /// <summary>
        /// 搜索 科室
        /// </summary>
        [DataMember]
        [NotMapped]
        public List<string> DepartmentList { get; set; }

        /// <summary>
        /// 搜索 标签
        /// </summary>
        [DataMember]
        [NotMapped]
        public List<string> TagGroupList { get; set; }
    }
}
