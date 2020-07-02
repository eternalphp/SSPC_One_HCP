using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 注册使用的协议
    /// </summary>
    [DataContract]
    public class ProtocolModel:BaseEntity
    {
        /// <summary>
        /// 协议名称
        /// </summary>
        [DataMember]
        public string ProctocolName { get; set; }

        /// <summary>
        /// 协议类型
        /// 1、隐私协议
        /// 2、用户协议
        /// </summary>
        [DataMember]
        public int? ProctocolType { get; set; }

        /// <summary>
        /// 协议存放路径
        /// </summary>
        [DataMember]
        public string ProctocolUrl { get; set; }
    }
}
