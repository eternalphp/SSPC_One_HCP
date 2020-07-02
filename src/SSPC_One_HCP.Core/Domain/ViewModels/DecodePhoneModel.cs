using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    public class DecodePhoneModel
    {
        public string userId { get; set; }
        public string unionId { get; set; }
        public string code { get; set; }
        public string encryptedData { get; set; }
        public string iv { get; set; }
        /// <summary>
        /// 用户绑定的手机号（国外手机号会有区号）
        /// </summary>
        public string phoneNumber { get; set; }
        /// <summary>
        /// 没有区号的手机号
        /// </summary>
        public string purePhoneNumber { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        public string countryCode { get; set; }
        /// <summary>
        /// ?
        /// </summary>
        public object watermark { get; set; }


    }
}
