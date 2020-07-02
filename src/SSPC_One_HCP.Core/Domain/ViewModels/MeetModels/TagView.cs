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
    public class TagView
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        [DataMember]
        public string TagName { get; set; }
        /// <summary>
        /// 标签文本资源键值
        /// </summary>
        [DataMember]
        public string TextKey { get; set; }
    }
}
