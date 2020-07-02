using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.DepartmentModels
{
    /// <summary>
    /// 产品下拉框选项视图模型
    /// </summary>
    public class DepartmentViewModel
    {
        /// <summary>
        /// 科室Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

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
