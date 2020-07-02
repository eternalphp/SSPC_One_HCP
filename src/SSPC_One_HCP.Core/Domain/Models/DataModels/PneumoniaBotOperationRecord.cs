using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{

    /// <summary>
    /// 肺炎BOT操作记录
    /// </summary>
    [DataContract]
    public class PneumoniaBotOperationRecord : BaseEntity
    {
        /// <summary>
        /// 注册用户ID
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// Unionid
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }
        /// <summary>
        /// OpenId
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
        /// <summary>
        /// 点击的模块
        /// </summary>
        [DataMember]
        public string ModulesClicked { get; set; }
        /// <summary>
        /// 控件的ID
        /// </summary>
        [DataMember]
        public string ControlId { get; set; }
        /// <summary>
        /// 控件名称
        /// </summary>
        [DataMember]
        public string ControlName { get; set; }
        /// <summary>
        /// 点击时间
        /// </summary>
        [DataMember]
        public DateTime ClickTime { get; set; }

        /// <summary>
        /// 离开时间
        /// </summary>
        [DataMember]
        public DateTime? LeaveTime { get; set; }
        /// <summary>
        /// 停留时间
        /// </summary>
        [DataMember]
        public string  ResidenceTime { get; set; }
        /// <summary>
        /// 播放时间
        /// </summary>
        [DataMember]
        public string PlayTime { get; set; }
        /// <summary>
        /// 类型
        /// 0：未注册 
        /// 1：已登录未注册
        /// 2：已登录已注册
        /// </summary>
        [DataMember]
        public int Type { get; set; }
}
}
