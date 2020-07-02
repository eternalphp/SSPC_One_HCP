using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.FkLibSyncModels
{
    /// <summary>
    /// 人员信息同步Model
    /// </summary>
    [DataContract]
    [NotMapped]
    public class PerInfoSyncModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [DataMember]
        public string OneHCPID { get; set; }

        /// <summary>
        /// 云势ID
        /// </summary>
        [DataMember]
        public string YSID { get; set; }

        /// <summary>
        /// OneHCP理由
        /// </summary>
        [DataMember]
        public string OneHCPReason { get; set; }

        /// <summary>
        /// OneHCP验证结果
        /// </summary>
        [DataMember]
        public string OneHCPState { get; set; }
    }
}
