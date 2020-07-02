using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.ViewModels.ProductModels
{
    /// <summary>
    /// BU-产品-科室下拉框视图模型
    /// </summary>
    [DataContract]
    [NotMapped]
    public class ProductBuDeptSelectionViewModel
    {
        /// <summary>
        /// BU名称列表
        /// </summary>
        [DataMember]
        public IEnumerable<string> BuNameList { get; set; }

        /// <summary>
        /// 产品列表(去重复)
        /// </summary>
        [DataMember]
        public IEnumerable<ProductSelectionViewModel> Products { get; set; }

        /// <summary>
        /// 科室列表(去重复)
        /// </summary>
        [DataMember]
        public IEnumerable<DepartmentSelectionViewModel> Departments { get; set; }

    }
}
