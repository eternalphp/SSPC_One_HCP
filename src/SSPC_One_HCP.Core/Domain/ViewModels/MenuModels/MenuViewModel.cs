using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MenuModels
{
    /// <summary>
    /// 菜单
    /// </summary>
    [DataContract]
    public class MenuViewModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }
        /// <summary>
        /// 展示名称
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
        /// <summary>
        /// 路由地址
        /// </summary>
        [DataMember(Name = "path")]
        public string Path { get; set; }
        /// <summary>
        /// 组件地址
        /// </summary>
        [DataMember(Name = "component")]
        public string Component { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        [DataMember(Name = "iconCls")]
        public string IconCls { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        [DataMember(Name = "children")]
        public List<MenuViewModel> Children { get; set; }
        /// <summary>
        /// 单个节点
        /// </summary>
        [DataMember(Name = "leaf")]
        public bool? Leaf { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        [DataMember(Name = "hidden")]
        public bool? Hidden { get; set; }
        /// <summary>
        /// 传值
        /// </summary>
        [DataMember(Name = "props")]
        public string Props { get; set; }
        /// <summary>
        /// url跳转地址
        /// </summary>
        [DataMember(Name = "linkPath")]
        public string LinkPath { get; set; }
        /// <summary>
        /// 树形控件使用的名称
        /// </summary>
        [DataMember(Name = "label")]
        public string Label { get; set; }
    }
}
