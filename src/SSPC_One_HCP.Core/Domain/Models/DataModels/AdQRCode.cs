using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 推广二维码
    /// </summary>
    [DataContract]
    public class AdQRCode : BaseEntity
    {
        /// <summary>
        /// BU名称
        /// </summary>
        [DataMember]
        public string BuName { get; set; }

        /// <summary>
        /// 公众号或者小程序名称
        /// </summary>
        [DataMember]
        public string AppName { get; set; }

        /// <summary>
        /// 公众号或者小程序地址
        /// </summary>
        [DataMember]
        public string AppUrl { get; set; }

        /// <summary>
        /// 二维码图片地址
        /// </summary>
        [DataMember]
        public string QRCodePicUrl { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        [DataMember]
        public int VisitAmount { get; set; }
      
    }
}
