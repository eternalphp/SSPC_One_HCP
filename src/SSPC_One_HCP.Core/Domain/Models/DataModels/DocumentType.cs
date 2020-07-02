using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 文档类型
    /// </summary>
    [DataContract]
    public class DocumentType:BaseEntity
    {
        /// <summary>
        /// 图标地址
        /// </summary>
        [DataMember]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [DataMember]
        public string TypeValue { get; set; }
    }
}
