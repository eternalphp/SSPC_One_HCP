using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 菜单信息
    /// </summary>
    [DataContract]
    public class MenuInfo : BaseEntity
    {
        /// <summary>
        /// 菜单名称
        ///    如果为多语言则为多语言表中的key
        /// </summary>
        [DataMember]
        public string MenuName { get; set; }
        /// <summary>
        /// 菜单前的图标
        /// </summary>
        [DataMember]
        public string MenuIcons { get; set; }
        /// <summary>
        /// 菜单地址
        /// </summary>
        [DataMember]
        public string MenuPath { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [DataMember]
        public string LinkPath { get; set; }
        /// <summary>
        /// 父级菜单id，默认为guid-empty
        /// </summary>
        [DataMember]
        public string ParentId { get; set; }
        /// <summary>
        /// 组件地址
        /// </summary>
        [DataMember]
        public string Component { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        [DataMember]
        public bool? Hidden { get; set; }
        /// <summary>
        /// 是否为单级目录
        /// </summary>
        [DataMember]
        public bool? Leaf { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int? Sort { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        [DataMember]
        public string Props { get; set; }
        /// <summary>
        /// 是否普通
        /// </summary>
        [DataMember]
        public bool? IsNormal { get; set; }
    }
}
