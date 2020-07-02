using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// bu信息
    /// </summary>
    [DataContract]
    public class BuInfo : BaseEntity
    {
        /// <summary>
        /// Bu名称
        /// </summary>
        [DataMember]
        public string BuName { get; set; }
    }
}
