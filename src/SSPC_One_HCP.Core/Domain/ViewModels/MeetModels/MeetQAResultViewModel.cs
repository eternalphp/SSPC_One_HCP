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
    /// 会议参会医生调研结果一览
    /// </summary>
    [NotMapped]
    [DataContract]
    public class MeetQAResultViewModel
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
        /// 会议问卷类型
        /// 1.会前问卷
        /// 2.会后问卷
        /// </summary>
        [DisplayName("会前/会后调研")]
        [DataMember]
        public string QAType { get; set; }

        /// <summary>
        /// 调研编号
        /// </summary>
        [DisplayName("调研编号")]
        [DataMember]
        public string QuestionId { get; set; }

        /// <summary>
        /// 调研题
        /// </summary>
        [DisplayName("调研题")]
        [DataMember]
        public string Question { get; set; }

        /// <summary>
        /// 调研答案
        /// </summary>
        [DisplayName("调研答案")]
        [DataMember]
        public string Answers { get; set; }

        /// <summary>
        /// 调研类型(单选、多选、填空)
        /// </summary>
        [DisplayName("调研类型")]
        [DataMember]
        public string QuestionType { get; set; }

        /// <summary>
        /// 选择的答案
        /// </summary>
        [DisplayName("选择的答案")]
        [DataMember]
        public string UserAnswers { get; set; }

        /// <summary>
        /// 调研时间
        /// </summary>
        [DisplayName("调研时间")]
        [DataMember]
        public string AnswerTime { get; set; }
    }
}
