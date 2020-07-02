using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.ProductModels
{
    /// <summary>
    /// 产品视图模型
    /// </summary>
    [DataContract]
    [NotMapped]
    public class ProductListItemViewModel
    {
        /// <summary>
        /// 产品信息
        /// </summary>
        [DataMember]
        public ProductInfo Product { get; set; }

        /// <summary>
        /// 科室集合
        /// </summary>
        [DataMember]
        public IEnumerable<DepartmentSelectionViewModel> DeptList { get; set; }

        /// <summary>
        /// 其它科室集合
        /// </summary>
        [DataMember]
        public IEnumerable<DepartmentSelectionViewModel> OtherDeptList { get; set; }

        /// <summary>
        /// BU名称集合
        /// </summary>
        [DataMember]
        public IEnumerable<string> BuNameList { get; set; }

        /// <summary>
        /// 科室名称集合(逗号分隔)
        /// </summary>
        [DataMember]
        public string DeptNames { get; set; }

        /// <summary>
        /// 其它科室名称集合(逗号分隔)
        /// </summary>
        [DataMember]
        public string OtherDeptNames { get; set; }

        ///// <summary>
        ///// BU名称集合(逗号分隔)
        ///// </summary>
        //[DataMember]
        //public string BuNames { get; set; }
    }
}
