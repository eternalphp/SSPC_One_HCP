using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    [DataContract]
    [NotMapped]
    public class ScheduleView
    {

        /// <summary>
        /// 会议日程
        /// </summary>
        [DataMember]
        public MeetScheduleView Schedule { get; set; }

        /// <summary>
        /// 讲者
        /// </summary>
        [DataMember]
        public MeetSpeaker Speaker { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Speakers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Topic { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Sort { get; set; }


    }
}
