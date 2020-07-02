using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.DoctorModels
{
    /// <summary>
    /// 医生认证模型
    /// </summary>
    public class DoctorVerifyViewModel
    {
        /// <summary>
        /// wxuserid
        /// </summary>
        [DataMember]
        public string wxuserid { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        [DataMember]
        public string remarks { get; set; }

        /// <summary>
        /// 审核状态 通过(1)/拒绝(3)
        /// </summary>
        [DataMember]
        public string verifyStatus { get; set; }
    }
}
