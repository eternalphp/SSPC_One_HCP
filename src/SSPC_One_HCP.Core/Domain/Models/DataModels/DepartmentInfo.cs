using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 科室信息表
    /// </summary>
    [DataContract]
    public class DepartmentInfo : BaseEntity
    {
        /// <summary>
        /// 科室名称
        /// </summary>
        [DataMember]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 科室类型
        /// 1为普通科室
        /// 2为其它科室
        /// </summary>
        [DataMember]
        public int? DepartmentType { get; set; }
    }
}
