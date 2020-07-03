using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.CommonModels
{
    /// <summary>
    /// Message模型
    /// </summary>
    [DataContract]
    public class MessageResult
    {
        /// <summary>
        /// access_token 
        /// </summary>
        [DataMember(Name = "error_code ")]
        public int error_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MessageResult() {

        }

    }
}
