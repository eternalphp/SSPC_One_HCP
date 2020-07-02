using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议标签
    /// </summary>
    [DataContract]
    public class MeetTag : BaseEntity
    {
        /// <summary>
        /// 会议Id
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }
        /// <summary>
        /// 标签Id
        /// </summary>
        [DataMember]
        public string TagId { get; set; }
    }
}
