using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 费森竞争产品
    /// </summary>
    public class CompetingProduct : BaseEntity
    {
        /// <summary>
        /// 竞品编号
        /// </summary>
        [DataMember]
        public int CompeteProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// 药品名称
        /// </summary>
        [DataMember]
        public string MedicineName { get; set; }

        /// <summary>
        /// 药品来源
        /// </summary>
        [DataMember]
        public string MedicineSource { get; set; }
    }
}
