using System;
using System.Collections.Generic;
using System.Text;

namespace SSPC_One_HCP.KBS.OutDto
{
   public class SSOUserInfoOutDto
    {
        /// <summary>
        /// 
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public User_info user_info { get; set; }
    }
    public class User_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ADAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ChineseName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EnglishName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CompanyCode { get; set; }
    }

}
