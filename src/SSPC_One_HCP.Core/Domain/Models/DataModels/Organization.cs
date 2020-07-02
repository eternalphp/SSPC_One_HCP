using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 部门
    /// </summary>
    [DataContract]
    public class Organization : BaseEntity
    {
        /// <summary>
        /// SAP编码
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 上级部门ID
        /// </summary>
        [DataMember]
        public string ParentId { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [DataMember]
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 部门经理Id
        /// </summary>
        [DataMember]
        public string ManagerId { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [DataMember]
        public int Level { get; set; }
        /// <summary>
        /// 按部门关系生成的访问路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 部门（BU）名称
        /// </summary>
        [DataMember]
        public string BuName { get => this.Remark; set => this.Remark = value; }
    }
}
