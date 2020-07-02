using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.ViewModels.ProductModels
{
    /// <summary>
    /// BU-产品-科室-其他科室下拉框视图模型
    /// </summary>
    [DataContract]
    [NotMapped]
    public class ProductBuDeptOtherDeptSelectionViewModel : ProductBuDeptSelectionViewModel
    {
        /// <summary>
        /// 除Departments以外的科室列表
        /// </summary>
        [DataMember]
        public IEnumerable<DepartmentSelectionViewModel> OtherDepartments { get; set; }
    }
}
