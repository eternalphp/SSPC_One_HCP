using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 注册流程模型
    /// </summary>
    [DataContract]
    public class RegisterModel:BaseEntity
    {
        /// <summary>
        /// UnionId
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }

        /// <summary>
        /// 手写签名
        /// </summary>
        [DataMember]
        public string SignUpName { get; set; }

        /// <summary>
        /// 微信用户（医生）的Id
        /// --以后会取代 UnionId
        /// </summary>
        [DataMember]
        public string WxUserId { get; set; }
    }
}
