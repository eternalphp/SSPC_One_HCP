using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.FkLibSyncModels
{
    /// <summary>
    /// 科室会同步Model
    /// </summary>
    [DataContract]
    [NotMapped]
    public class MeetingSyncModel
    {
        /// <summary>
        /// 科室会议ID
        /// </summary>
        [DataMember]
        public string ActivityID { get; set; }

        /// <summary>
        /// 科室会编号
        /// </summary>
        [DataMember]
        public string MeetingNumber { get; set; }

        /// <summary>
        /// 科室会标题
        /// </summary>
        [DataMember]
        public string ActivityName { get; set; }

        /// <summary>
        /// 活动状态1:有效0：无效
        /// </summary>
        [DataMember]
        public string Stutas { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CreatTime { get; set; }

        /// <summary>
        /// 召开时间
        /// </summary>
        [DataMember]
        public DateTime HoldTime { get; set; }

        /// <summary>
        /// 医院
        /// </summary>
        [DataMember]
        public string Hospital { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        [DataMember]
        public string HospitalID { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        [DataMember]
        public string KeShi { get; set; }

        /// <summary>
        /// 科室ID
        /// </summary>
        [DataMember]
        public string KeShiID { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// 参与人数
        /// </summary>
        [DataMember]
        public int PartInNum { get; set; }
    }
}
