using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 公司管理
    /// </summary>
    [DataContract]
    public class CompanyInfo : BaseEntity
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [DataMember]
        public string CompanyName { get; set; }
        /// <summary>
        /// 公司号
        /// </summary>
        [DataMember]
        public string CompanyNum { get; set; }
    }
}
