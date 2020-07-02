using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 多付医生推广，推广其他公众号
    /// </summary>
    /// 2019-06-03
   [DataContract]
  public  class QRcodeExtension: BaseEntity
    {
        /// <summary>
        /// 小程序或公众号唯一标识
        /// </summary>
        [DataMember]
        public string AppId { get; set; }
        /// <summary>
        /// 小程序或公众号名称
        /// </summary>
        [DataMember]
        public string AppName { get; set; }
        /// <summary>
        /// 推广类型 0:推广多福医生，1:推广其他公众号
        /// </summary>
        [DataMember]
        public string AppType { get; set; }
        /// <summary>
        /// 小程序路径
        /// </summary>
        [DataMember]
        public string AppUrl { get; set; }
        /// <summary>
        /// 二维码图片路径
        /// </summary>
        [DataMember]
        public string AppImangeUrl { get; set; }
        /// <summary>
        /// 二维码图片名称
        /// </summary>
        [DataMember]
        public string AppImangeName { get; set; }
    }
}
