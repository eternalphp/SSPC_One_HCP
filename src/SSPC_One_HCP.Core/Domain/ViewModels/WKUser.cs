using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 文库登录用户
    /// </summary>
    [NotMapped]
    public class WKUser
    {
        /// <summary>
        /// Unionid
        /// </summary>
        public string Unionid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string UserCode { get; set; }
    }
}
