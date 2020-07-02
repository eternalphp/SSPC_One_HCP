using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    /// <summary>
    /// 报名
    /// </summary>
    [NotMapped]
    [DataContract]
    public class MyMeetOrderViewModel : MyMeetOrder
    {
        /// <summary>
        /// 提醒时间（）
        /// </summary>
        [DataMember]
        public int? WarnMinutes { get; set; }
    }
}
