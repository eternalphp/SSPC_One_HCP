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
    public class BannerInfoItem : BaseEntity
    {
        [DataMember]
        public string BannerInfoId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Describe { get; set; }
        /// <summary>
        /// 业务标签
        /// 1：多福小程序
        /// 2. H5页面 
        /// 3. 跳转其他小程序
        /// 4：公众号
        /// </summary>
        [DataMember]
        public int Type { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [DataMember]
        public string ShowPlace { get; set; }
        /// <summary>
        /// 跳转Url
        /// </summary>
        [DataMember]
        public string Src { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int Sort { get; set; }
    }
}
