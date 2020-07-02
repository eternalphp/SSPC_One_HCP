using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 横幅信息
    /// </summary>
    [DataContract]
    public class BannerInfo : BaseEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// 标题图标
        /// </summary>
        [DataMember]
        public string Icon { get; set; }
        /// <summary>
        /// 业务场景
        /// </summary>
        [DataMember]
        public string Scene { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Describe { get; set; }
        /// <summary>
        /// 是否显示
        /// 0: 不实现
        /// 1: 显示
        /// </summary>
        [DataMember]
        public int? IsShow { get; set; }
    }
}
