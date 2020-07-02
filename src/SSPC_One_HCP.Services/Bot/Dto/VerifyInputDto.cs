using SSPC_One_HCP.Core.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Bot.Dto
{

    public class VerifyBase
    {   /// <summary>
        /// 域登录账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

    }
    public class VerifyAdminInputDto : VerifyBase
    {
        public string Value { get; set; }
    }

    public class VerifyInputDto
    {
        public string ADAccount { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        public string openId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string code { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string encryptedData { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string iv { set; get; }


        public string AppId { get; set; }

        public string WxSceneId { get; set; }

        public DecodedUserInfoModel userInfo { get; set; }

    }



    public class AuthorizedOrNotInputDto
    {
        public string code { get; set; }

        public string appid { get; set; }

        public string username { get; set; }
    }
}
