using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议签到报名表
    /// </summary>
    [DataContract]
    public class MeetSignUp : BaseEntity
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }

        /// <summary>
        /// 报名人员Id
        /// </summary>
        [DataMember]
        public string SignUpUserId { get; set; }

        /// <summary>
        /// 是否已签到
        /// 0、未签到
        /// 1、已签到
        /// </summary>
        [DataMember]
        public int? IsSignIn { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>
        [DataMember]
        public DateTime? SignInTime { get; set; }

        /// <summary>
        /// 是否查看过会议详情
        /// 0、未查看
        /// 1、已查看
        /// </summary>
        [DataMember]
        public int? IsKnewDetail { get; set; }
    }
}
