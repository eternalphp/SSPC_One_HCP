using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 名片盒
    /// </summary>
    [DataContract]
    public class BusinessCard : BaseEntity
    {
        /// <summary>
        /// 医生的Id
        /// </summary>
        [DataMember]
        public string WxUserId { get; set; }

        /// <summary>
        /// 所收藏的医生的Id
        /// </summary>
        [DataMember]
        public string OwnerWxUserId { get; set; }
    }
}
