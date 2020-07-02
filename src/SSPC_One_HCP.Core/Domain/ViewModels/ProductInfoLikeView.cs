using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [NotMapped]
    public class ProductInfoLikeView
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public string ProID { get; set; }

        /// <summary>
        /// 是否赞同
        /// </summary>
        [DataMember]
        public int IsLike { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [DataMember]
        public string UserID { get; set; }

    }
}
