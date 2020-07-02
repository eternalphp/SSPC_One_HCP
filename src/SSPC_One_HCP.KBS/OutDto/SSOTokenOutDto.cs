using System;
using System.Collections.Generic;
using System.Text;

namespace SSPC_One_HCP.KBS.OutDto
{
    public class SSOTokenOutDto
    {
        /// <summary>
        /// 
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string state { get; set; }
    }
}
