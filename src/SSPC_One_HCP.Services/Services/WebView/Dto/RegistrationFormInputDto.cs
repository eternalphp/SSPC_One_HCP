using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WebView.Dto
{
    public class RegistrationFormInputDto
    {
        /// <summary>
        /// 用户的唯一标识
        /// </summary>
        [DataMember]
        public string Openid { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        [DataMember]
        public string WxNickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        [DataMember]
        public string WxSex { get; set; }
        /// <summary>
        /// 用户个人资料填写的省份
        /// </summary>
        [DataMember]
        public string WxProvince { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        [DataMember]
        public string WxCity { get; set; }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        [DataMember]
        public string WxCountry { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        [DataMember]
        public string WxPicture { get; set; }
  
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        [DataMember]
        public string Unionid { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [DataMember]
        public string RegistrationAge { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public string RegistrationGender { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        [DataMember]
        public string HospitalName { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        [DataMember]
        public string DepartmentName { get; set; }
        /// <summary>
        /// 是否基层
        /// </summary>
        [DataMember]
        public int? RegistrationIsBasicLevel { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [DataMember]
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        [DataMember]
        public string Area { get; set; }
        /// <summary>
        /// 医生电话
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }
        /// <summary>
        /// 医生来源AppId
        /// </summary>
        [DataMember]
        public string SourceAppId { get; set; }
        /// <summary>
        /// 医生来源微信场景ID
        /// </summary>
        [DataMember]
        public string WxSceneId { get; set; }
    }
}
