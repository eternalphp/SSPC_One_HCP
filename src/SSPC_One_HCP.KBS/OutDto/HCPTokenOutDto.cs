using System;
using System.Collections.Generic;
using System.Text;

namespace SSPC_One_HCP.KBS.OutDto
{
    public class HCPTokenOutDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int expires_in { get; set; }

        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string res_code { get; set; }
        /// <summary>
        /// 登录成功
        /// </summary>
        public string res_msg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string expires { get; set; }
    }
}
