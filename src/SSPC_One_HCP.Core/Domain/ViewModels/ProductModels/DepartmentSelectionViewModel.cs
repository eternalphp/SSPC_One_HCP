using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.ProductModels
{
    /// <summary>
    /// 产品下拉框选项视图模型
    /// </summary>
    public class DepartmentSelectionViewModel
    {
        /// <summary>
        /// 科室Id
        /// </summary>
        [DataMember]
        public string DeptId { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        [DataMember]
        public string DeptName { get; set; }

        /// <summary>
        /// 科室类型
        /// 1为普通科室
        /// 2为其它科室
        /// </summary>
        [DataMember]
        public int? DeptType { get; set; }

        [DataMember]
        public int? DepartmentType { get; set; }
    }
}
