using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    [NotMapped]
    [DataContract]
    public class MeetSignUpViewModel
    {
        /// <summary>
        /// 生成签到二维码的公众号（如：费卡文库）的AppId , 默认是空或者0
        /// </summary>
        [DataMember]
        public string AppId { get; set; }

        /// <summary>
        /// 会议ID
        /// </summary>
        [DataMember]
        public string MeetId { get; set; }
    }
}
