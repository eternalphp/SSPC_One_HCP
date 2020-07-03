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
    public class Message
    {
        /// <summary>
        /// access_token 
        /// </summary>
        [DataMember(Name = "SendType")]
        public int SendType{ get; set; }

        /// <summary>
        /// Receiver 
        /// </summary>
        [DataMember(Name = "Receiver")]
        public string Receiver{ get; set; }

        /// <summary>
        /// SignCode
        /// </summary>
        [DataMember(Name = "SignCode")]
        public string SignCode { get; set; }

        /// <summary>
        /// TemplateCode
        /// </summary>
        [DataMember(Name = "TemplateCode")]
        public string TemplateCode { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [DataMember(Name = "Content")]
        public string Content { get; set; }

    }
}
