using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.CommonModels
{
    /// <summary>
    /// OAuthToken模型
    /// </summary>
    [DataContract]
    public class OAuthToken
    {
        /// <summary>
        /// client_id
        /// </summary>
        [DataMember(Name = "client_id")]
        public string client_id { get; set; }

        /// <summary>
        /// client_secret
        /// </summary>
        [DataMember(Name = "client_secret")]
        public string client_secret { get; set; }

        /// <summary>
        /// scope
        /// </summary>
        [DataMember(Name = "scope")]
        public string scope { get; set; }

        /// <summary>
        /// grant_type
        /// </summary>
        [DataMember(Name = "grant_type")]
        public string grant_type { get; set; }

        /// <summary>
        /// state
        /// </summary>
        [DataMember(Name = "state")]
        public string state { get; set; }

    }
}
