using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 访问记录表
    /// </summary>
    ///2019-05-31
  public class QRcodeRecord: BaseEntity
    {
        /// <summary>
        /// 小程序APPId
        /// </summary>
        [DataMember]
        public string AppId { get; set; }
        /// <summary>
        /// 用户UnionId
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }
        /// <summary>
        /// 是否注册，0注册，1未注册
        /// </summary>
        [DataMember]
        public int Isregistered { get; set; }
        /// <summary>
        /// 医生来源类型
        /// 0. 推广多福医生
        /// 1. 第三方公众号二维码 
        /// </summary>
        public string SourceType { get; set; }
        /// <summary>
        /// 微信场景ID
        /// </summary>
        public string WxSceneId { get; set; }

    }
}
