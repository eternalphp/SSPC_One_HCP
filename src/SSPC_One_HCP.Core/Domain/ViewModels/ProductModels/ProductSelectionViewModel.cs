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
    public class ProductSelectionViewModel
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        [DataMember]
        public string ProId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember]
        public string ProName { get; set; }
    }
}
