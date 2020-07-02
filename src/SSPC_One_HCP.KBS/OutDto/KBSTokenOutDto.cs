using System;
using System.Collections.Generic;
using System.Text;

namespace SSPC_One_HCP.KBS.OutDto
{
    public class KBSTokenOutDto
    {

        /// <summary>
        /// 
        /// </summary>
        public string Auth_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Refresh_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Expires_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Token_type { get; set; }

    }
}
