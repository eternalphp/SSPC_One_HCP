using System;
using System.Collections.Generic;
using System.Text;

namespace SSPC_One_HCP.KBS.InputDto
{

    public class LoginADInputDto
    {
        public string Sign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
    }
    public class LoginInputDto
    {

        public string Sign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string ADAccount { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string ChineseName { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CompanyCode { get; set; }
    }
}
