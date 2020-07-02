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
    /// 产品搜索视图模型
    /// </summary>
    [DataContract]
    [NotMapped]
    public class SearchProductViewModel
    {
        /// <summary>
        /// 产品信息
        /// </summary>
        [DataMember]
        public ProductInfo Product { get; set; }

        /// <summary>
        /// 搜索传入的科室关键字
        /// </summary>
        [DataMember]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 搜索传入的BU关键字
        /// </summary>
        [DataMember]
        public string BuName { get; set; }
    }
}
