using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.CommonModels
{
    /// <summary>
    /// AccessToken模型
    /// </summary>
    [DataContract]
    public class AccessToken
    {
        /// <summary>
        /// access_token 
        /// </summary>
        [DataMember(Name = "access_token ")]
        public string access_token { get; set; }

        /// <summary>
        /// refresh_token 
        /// </summary>
        [DataMember(Name = "refresh_token ")]
        public string refresh_token { get; set; }

        /// <summary>
        /// scope
        /// </summary>
        [DataMember(Name = "scope")]
        public string scope { get; set; }

        /// <summary>
        /// expires_in
        /// </summary>
        [DataMember(Name = "expires_in")]
        public int expires_in { get; set; }

        /// <summary>
        /// state
        /// </summary>
        [DataMember(Name = "state")]
        public string state { get; set; }

    }
}
