using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 浏览记录/包括停留记录
    /// </summary>
    [DataContract]
    public class BrowseRecords: BaseEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public string WxUserId { get; set; }

        /// <summary>
        /// 类型 详见 EnumRecord
        /// </summary>
        [DataMember]
        public int RecordType { get; set; }

        /// <summary>
        /// 进入时间
        /// </summary>
        [DataMember]
        public DateTime RecordStart { get; set; }

        /// <summary>
        /// 离开时间
        /// </summary>
        [DataMember]
        public DateTime RecordEnd { get; set; }

        /// <summary>
        /// 停留时间
        /// </summary>
        [DataMember]
        public int? ResidenceTime { get; set; }

    }
}
