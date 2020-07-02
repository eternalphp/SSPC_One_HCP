using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    [DataContract]
    [NotMapped]
    public class DiscoveryViewModel
    {
        /// <summary>
        /// 是否已注册
        /// </summary>
        public int IsRegister { get; set; }

        /// <summary>
        /// 提示语
        /// </summary>
        public string RemindWord { get; set; }

        /// <summary>
        /// 适合你的会议主题
        /// </summary>
        public IEnumerable<MeetInfo> MeetInfos { get; set; }

        /// <summary>
        /// 适合你的学术知识
        /// </summary>
        public IEnumerable<ProductTypeInfo> Academic { get; set; }
    }
}
