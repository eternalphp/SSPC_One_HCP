using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 下拉选项设置
    /// </summary>
    [DataContract]
    public class DropDownConfig : BaseEntity
    {
        /// <summary>
        /// 下拉值
        /// </summary>
        [DataMember]
        public string DropDownValue { get; set; }
        /// <summary>
        /// 下拉文本
        /// </summary>
        [DataMember]
        public string DorpDownText { get; set; }
        /// <summary>
        /// 下拉类型
        /// </summary>
        [DataMember]
        public string DropDownType { get; set; }
    }
}
