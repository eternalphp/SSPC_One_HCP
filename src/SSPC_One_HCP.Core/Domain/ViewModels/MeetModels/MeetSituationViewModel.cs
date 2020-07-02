using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    /// <summary>
    /// 会议报告-医生参会情况一览
    /// </summary>
    [NotMapped]
    [DataContract]
    public class MeetSituationViewModel
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        [DisplayName("会议ID")]
        [DataMember]
        public string MeetId { get; set; }

        /// <summary>
        /// 会议名称
        /// </summary>
        [DisplayName("会议名称")]
        [DataMember]
        public string MeetTitle { get; set; }

        /// <summary>
        /// 会议地点
        /// </summary>
        [DisplayName("会议地点")]
        [DataMember]
        public string MeetAddress { get; set; }

        /// <summary>
        /// 会议开始时间
        /// </summary>
        [DisplayName("开始时间")]
        [DataMember]
        public DateTime? MeetStartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        [DisplayName("结束时间")]
        public DateTime? MeetEndTime { get; set; }

        /// <summary>
        /// 医生三方ID
        /// </summary>
        [DisplayName("医生三方ID")]
        [DataMember]
        public string DoctorId { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        [DisplayName("医生姓名")]
        [DataMember]
        public string DoctorName { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [DisplayName("职称")]
        [DataMember]
        public string DoctorTitle { get; set; }

        /// <summary>
        /// 所属医院
        /// </summary>
        [DisplayName("医院")]
        [DataMember]
        public string HospitalName { get; set; }
        
        /// <summary>
        /// 所属科室
        /// </summary>
        [DisplayName("科室")]
        [DataMember]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 医生手机号
        /// </summary>
        [DataMember]
        [DisplayName("医生手机号")]
        public string Mobile { get; set; }

        /// <summary>
        /// UNIONID
        /// </summary>
        [DisplayName("UNIONID")]
        [DataMember]
        public string UNIONID { get; set; }

        /// <summary>
        /// 代表姓名
        /// </summary>
        [DisplayName("代表姓名")]
        [DataMember]
        public string CreaterName { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>
        [DisplayName("签到时间")]
        [DataMember]
        public DateTime? SignInTime { get; set; }

        /// <summary>
        /// 报名时间
        /// </summary>
        [DisplayName("报名时间")]
        [DataMember]
        public DateTime? OrderTime { get; set; }

        /// <summary>
        /// 浏览邀请函（是否查看过会议详情）: Y/N
        /// </summary>
        [DisplayName("浏览邀请函")]
        [DataMember]
        public string IsKnewDetail { get; set; }
    }
}
