using System.Runtime.Serialization;

namespace SSPC_One_HCP.Services.Services.WeChat.Dto
{
    public class LoginInputDto
    {
        /// <summary>
        /// 域登录账号
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string Password { get; set; }
    }
}
