using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.ProductInfoModels
{
    /// <summary>
    /// 产品信息(知识库)
    /// </summary>
    public class ProductInfoViewModel
    {
        /// <summary>
        /// 知识库信息
        /// </summary>
        [DataMember]
        public DataInfo dataInfo { get; set; }

        /// <summary>
        /// 知识库对应的产品及科室
        /// </summary>

        [DataMember]
        public ProductBuDeptSelectionViewModel ProductAndDeps { get; set; }
    }
}
