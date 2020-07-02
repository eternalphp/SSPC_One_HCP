using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 销售 用户信息
    /// </summary>
    public class BotSaleUserInfo : BaseEntity
    {
        /// <summary>
        /// 域帐号
        /// </summary>
        [DataMember]
        public string ADAccount { get; set; }
        /// <summary>
        /// 开放平台中的唯一标识
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }
        /// <summary>
        /// 小程序或公众号中的唯一标识
        /// </summary>
        [DataMember]
        public string OpenId { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        [DataMember]
        public string WxName { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        [DataMember]
        public string WxPicture { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public string WxGender { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        [DataMember]
        public string WxCountry { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        [DataMember]
        public string WxCity { get; set; }
        /// <summary>
        /// 所在省份
        /// </summary>
        [DataMember]
        public string WxProvince { get; set; }
    }
}
