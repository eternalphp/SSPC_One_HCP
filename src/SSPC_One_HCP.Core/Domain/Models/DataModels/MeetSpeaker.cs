using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 会议讲者简历
    /// </summary>
    [DataContract]
    public class MeetSpeaker:BaseEntity
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }

        /// <summary>
        /// 讲者姓名
        /// </summary>
        [DataMember]
        public string SpeakerName { get; set; }

        /// <summary>
        /// 讲者简历
        /// </summary>
        [DataMember]
        public string SpeakerDetail { get; set; }

    }
}
