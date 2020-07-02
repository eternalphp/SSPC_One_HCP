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
    public class ProductTypeViewModel
    {
        /// <summary>
        /// 科室Id集合
        /// </summary>
        [DataMember]
        public IEnumerable<DepartmentInfo> Departments { get; set; }

        /// <summary>
        /// 产品信息
        /// </summary>
        [DataMember]
        public ProductTypeInfo ProductInfo { get; set; }

        /// <summary>
        /// BU名称
        /// </summary>
        [DataMember]
        public BuInfo BU { get; set; }
    }
}
