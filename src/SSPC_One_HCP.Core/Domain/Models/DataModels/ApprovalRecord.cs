using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 审批记录
    /// </summary>
    [DataContract]
    public class ApprovalRecord : BaseEntity
    {
        /// <summary>
        /// 单据主键
        /// </summary>
        [DataMember]
        public string AssetsMainId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [DataMember]
        public string OperationUser { get; set; }
        /// <summary>
        /// 动作：提交、同意、驳回
        /// </summary>
        [DataMember]
        public string OperationAction { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [DataMember]
        public DateTime? OperationDate { get; set; }
    }
}
