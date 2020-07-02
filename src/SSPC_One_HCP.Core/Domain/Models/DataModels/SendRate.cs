using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 发送频率
    /// </summary>
    [DataContract]
    public class SendRate: BaseEntity
    {

        /// <summary>
        /// 医生编号
        /// </summary>
        [StringLength(36)]
        [DataMember]
        public string DoctorId { get; set; }

        /// <summary>
        /// 发送周期类型
        /// 1：天 2：周 3：月
        /// </summary>
        [DataMember]
        public int? SendCycleType { get; set; }

        /// <summary>
        /// 发送数量
        /// </summary>
        [DataMember]
        public int? SendNumber { get; set; }

        /// <summary>
        /// 是否默认
        /// True:默认
        /// </summary>
        [DataMember]
        public bool? IsDefault { get; set; }
    }
}
