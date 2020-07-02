using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 职位（职位的Code在同一个部门/团队下只可能存在一个）
    /// </summary>
    [DataContract]
    public class Position : BaseEntity
    {
        /// <summary>
        /// SAP编码
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [DataMember]
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 组织框架Id
        /// </summary>
        [DataMember]
        public string OrganizationId { get; set; }
        /// <summary>
        /// 汇报人Id
        /// </summary>
        [DataMember]
        public string ReporterId { get; set; }
        /// <summary>
        /// 持有人Id
        /// </summary>
        [DataMember]
        public string HolderId { get; set; }
    }
}
