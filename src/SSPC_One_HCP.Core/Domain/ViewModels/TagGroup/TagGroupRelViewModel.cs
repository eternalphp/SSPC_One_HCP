using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.TagGroup
{
    /// <summary>
    /// 标签组和标签
    /// </summary>
   
    public class TagGroupRelViewModel
    {
        /// <summary>
        /// 标签组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 标签组Id
        /// </summary>
        public string TagGroupId { get; set; }

        /// <summary>
        /// 标签的ID
        /// </summary>
        public List<string> TabInfoIds { get; set; }
    }
}
