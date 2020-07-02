using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 产品资料赞同页面
    /// </summary>
    [DataContract]
    public class ProductInfoLike : BaseEntity
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        [DataMember]
        public string ProID { get; set; }

        /// <summary>
        /// 是否赞同
        /// 1赞同 2不赞同 
        /// </summary>
        [DataMember]
        public int IsLike { get; set; }
    }
}
