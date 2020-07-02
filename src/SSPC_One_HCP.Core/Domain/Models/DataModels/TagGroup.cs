using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 标签组
    /// </summary>
    public class TagGroup : BaseEntity
    {
        /// <summary>
        /// 标签组名称
        /// </summary>
        [DataMember]
        public string TagGroupName { get; set; }
    }
}
