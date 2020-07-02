using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 关系表
    /// </summary>
    [NotMapped]
    [DataContract]
    public class BuProDeptRelViewModel
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        [DataMember]
        public string ProId { get; set; }
        /// <summary>
        /// BU名称集合（逗号分隔）
        /// </summary>
        [DataMember]
        public string BuNames { get; set; }
        /// <summary>
        /// BU名称集合（搜索使用）
        /// </summary>
        [DataMember]
        public IEnumerable<string> BuNameList { get; set; }
        /// <summary>
        /// 部门名称（逗号分隔）
        /// </summary>
        [DataMember]
        public string DeptNames { get; set; }
        /// <summary>
        /// 部门名称集合（搜索使用）
        /// </summary>
        [DataMember]
        public IEnumerable<string> DeptNameList { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }
        /// <summary>
        /// 搜索传入的科室关键字
        /// </summary>
        [DataMember]
        public string SearchDept { get; set; }
        /// <summary>
        /// 搜索传入的BU关键字
        /// </summary>
        [DataMember]
        public string SearchBU { get; set; }
    }
}
