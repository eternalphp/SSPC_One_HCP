using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 系统设置选项的视图实体类
    /// </summary>
    [NotMapped]
    [DataContract]
    public class ConfigurationViewModel<T>
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 配置值
        /// </summary>
        [DataMember]
        public T Value { get; set; }
    }
}
