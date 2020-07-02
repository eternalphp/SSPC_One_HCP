using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.TagGroup
{
    /// <summary>
    /// 标签以及标签组
    /// </summary>
    public class TagGroupTagViewModel
    {
        [DataMember]
        public string TagGroupId { set; get; }
        /// <summary>
        /// 标签名称数组
        /// </summary>
        [DataMember]
        public string[] TagsId { get; set; }
    }
}
