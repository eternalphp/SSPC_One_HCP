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
    /// 产品视图模型(保存用)
    /// </summary>
    [DataContract]
    [NotMapped]
    public class ProductDetailViewModel
    {
        /// <summary>
        /// 产品信息
        /// </summary>
        [DataMember]
        public ProductInfo Product { get; set; }

        /// <summary>
        /// 科室ID集合
        /// </summary>
        [DataMember]
        public IEnumerable<string> DeptIdList { get; set; }
        
        /// <summary>
        /// BU名称集合
        /// </summary>
        [DataMember]
        public IEnumerable<string> BuNameList { get; set; }
    }
}
