using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 标签
    /// </summary>
    [DataContract]
    public class TagInfo : BaseEntity
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        [DataMember]
        public string TagName { get; set; }
        /// <summary>
        /// 标签类型：
        /// M1:会议自动
        /// M2:会议手动
        /// D1:医生自动
        /// D2:医生手动
        /// </summary>
        [DataMember]
        public string TagType { get; set; }
        /// <summary>
        /// 标签规则（自动标签有规则）
        /// </summary>
        [DataMember]
        public string TagRule { get; set; }
        /// <summary>
        /// 中英文KEY
        /// </summary>
        [DataMember]
        public string TextKey { get; set; }
    }
}
