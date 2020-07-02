using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议和产品和科室关系表
    /// </summary>
    [DataContract]
    public class MeetAndProAndDepRelation : BaseEntity
    {
        /// <summary>
        /// 会议Id
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// 科室Id
        /// </summary>
        [DataMember]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 所属BU
        /// </summary>
        [DataMember]
        public string BuName { get; set; }

        [DataMember]
        public int? DepartmentType { get; set; }
    }
}
