using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.Approval
{
    /// <summary>
    /// 审批结果
    /// </summary>
    [DataContract]
    public class ApprovalResultViewModel
    {
        /// <summary>
        /// 被审批数据的主键Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// true 通过/ false 拒绝
        /// </summary>
        [DataMember]
        public bool? Approved { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Note { get; set; }
    }
}
